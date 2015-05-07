function adminGroupsController($scope, $http, $modal, UserFactory, UtilitiesFactory) {
    $scope.isCreated = false;
    $scope.currentGroup = {};
    $scope.editableGroup = {};
    //Used to display the data 


    $scope.loadData = function () {
        UtilitiesFactory.showSpinner();

        $http.get('/api/Group').success(function (data) {
            $scope.Groups = data;
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.loadData();

    $scope.add = function () {
        UtilitiesFactory.showSpinner();

        $http.post('/api/Group/', $scope.currentGroup).success(function (data) {

            $scope.isCreated = false;
            $scope.editableGroup = {};
            $scope.currentGroup = {};

            $scope.loadData();

            UtilitiesFactory.hideSpinner();

        }).error(function (data) {
            $scope.error = "An Error has occured while creating group! " + data;
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
        
        $http.put('/api/Group/', group).success(function (data) {
            group.isEditable = false;
            $scope.editableGroup = {};
            UtilitiesFactory.hideSpinner();
        }).error(function (data) {
            $scope.error = "An Error has occured while saving group! " + data;
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
        group.IsDeleted = true;

        $http.put('/api/Group/', group).success(function (data) {
            var index = 0;

            for (var i = 0; i < $scope.Groups.length; i++) {
                if ($scope.Groups[i].IsDeleted) {
                    index = i;
                    break;
                }
            }
            $scope.Groups.splice(index, 1);
            UtilitiesFactory.hideSpinner();
        }).error(function (data) {
            $scope.error = "An Error has occured while deleting group! " + data;
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
