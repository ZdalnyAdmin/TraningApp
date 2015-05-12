function adminGroupsController($scope, $http, $modal, UserFactory, UtilitiesFactory) {
    $scope.isCreated = false;
    $scope.viewModel = {};
    $scope.editableGroup = {};
    //Used to display the data 


    $scope.loadData = function () {

        UtilitiesFactory.showSpinner();
        $scope.viewModel.ActionType = 0;
        $http.post('/api/Group/', $scope.viewModel).success(function (data) {
            $scope.viewModel = data;
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.viewModel.ErrorMessage = 'Wystąpił nieoczekiwany błąd podczas pobierania grup';
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.loadData();

    $scope.add = function () {
        UtilitiesFactory.showSpinner();
        $scope.viewModel.ActionType = 3;
        $http.post('/api/Group/', $scope.viewModel).success(function (data) {
            $scope.viewModel = data;
            $scope.isCreated = false;
            $scope.editableGroup = {};
            UtilitiesFactory.hideSpinner();

        }).error(function (data) {
            $scope.viewModel.ErrorMessage = " Wystąpił nieoczekiwany błąd podczas zapisu grupy " + data;
            $scope.loadData();
            UtilitiesFactory.hideSpinner();
        });
    };

    $scope.showCreate = function () {
        $scope.isCreated = true;
    };

    $scope.edit = function (group) {
        if (!group) {
            return;
        }
        UtilitiesFactory.showSpinner();
        $scope.viewModel.ActionType = 2;
        $http.post('/api/Group/', $scope.viewModel).success(function (data) {
            $scope.viewModel = data;
            $scope.editableGroup = {};
            UtilitiesFactory.hideSpinner();
        }).error(function (data) {
            $scope.viewModel.ErrorMessage = "Wystąpił nieoczekiwany błąd podczas edycji grupy " + data;
            UtilitiesFactory.hideSpinner();
        });

        $scope.assignedPeopleToGroup(group, false)
    };

    $scope.showEdit = function (group) {
        if (!group) {
            return;
        }
        group.isEditable = true;
        $scope.editableGroup = angular.copy(group);
    };

    //Used to save a record after edit 
    $scope.cancel = function (group) {
        if (!group) {
            return;
        }
        group.Name = $scope.editableGroup.Name;
        group.AssignedPeopleDisplay = $scope.editableGroup.AssignedPeopleDisplay;
        $scope.editableGroup = {};
        group.isEditable = false;
    };

    $scope.delete = function (group) {
        if (!group) {
            return;
        }
        UtilitiesFactory.showSpinner();
        $scope.viewModel.ActionType = 1;

        $http.post('/api/Group/', $scope.viewModel).success(function (data) {
            $scope.viewModel = data;
            UtilitiesFactory.hideSpinner();
        }).error(function (data) {
            $scope.viewModel.ErrorMessage = "Wystąpił nieoczekiwany błąd podczas usuwanie grupy ";
            UtilitiesFactory.hideSpinner();
        });
    };

    $scope.chooseUsers = function (group) {
        var modalInstance = $modal.open({
            templateUrl: '/Templates/Modals/usersListModal.html',
            controller: 'usersListModalController',
            size: 'sm',
            resolve: {
                selectedUsers: function () {
                    return $scope.selectedUsers;
                }
            }
        });

        modalInstance.result.then(function (selectedUsers) {
            if (!!selectedUsers) {
                group.AssignedPeople = selectedUsers;
                group.AssignedPeopleDisplay = '';
                for (var i = 0; i < selectedUsers.length; i++) {
                    if (group.AssignedPeopleDisplay != '') {
                        group.AssignedPeopleDisplay += ', ';
                    }
                    group.AssignedPeopleDisplay += selectedUsers[i].UserName;
                }
            }
        });
    };
}

adminGroupsController.$inject = ['$scope', '$http', '$modal', 'UserFactory', 'UtilitiesFactory'];
