function adminUsersController($scope, $http, $modal, UserFactory, UtilitiesFactory) {
    $scope.viewModel = {};
    $scope.editablePerson = {};
    $scope.availableStatus = ['Aktywny', 'Zablokowany'];
    //temp solution

    //Used to display the data 
    $scope.loadData = function () {
        UtilitiesFactory.showSpinner();
        $scope.viewModel.ActionType = 0;
        $http.post('/api/Person/', $scope.viewModel).success(function (data) {
            $scope.viewModel = data;
            UtilitiesFactory.hideSpinner();
        })
        .error(function (data) {
            $scope.viewModel.ErrorMessage = 'Wystąpił nieoczekiwany błąd podczas pobierania uzytkownikow';
            $scope.loading = false;
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.loadData();

    //Used to save a record after edit 
    $scope.save = function (person) {
        if (!person) {
            return;
        }
        UtilitiesFactory.showSpinner();
        $scope.viewModel.Current = person;
        $scope.viewModel.ActionType = 2;
        $http.post('/api/Person/', $scope.viewModel).success(function (data) {
            $scope.viewModel.Success = 'Dane zostaly zapisane';
            //shoudl get only by id
            person.isEditable = false;
            $scope.editablePerson = {};
            UtilitiesFactory.hideSpinner();
        }).error(function (data) {
            $scope.viewModel.ErrorMessage = 'Wystąpił nieoczekiwany błąd podczas zapisywania uzytkownika' + data;
            UtilitiesFactory.hideSpinner();
        });
    };

    //Used to save a record after edit 
    $scope.cancel = function (person) {
        if (!person) {
            return;
        }
        person.UserName = $scope.editablePerson.UserName;
        person.Email = $scope.editablePerson.Email;
        person.Status = $scope.editablePerson.Status;
        person.selectedStatus = $scope.editablePerson.selectedStatus;
        $scope.editablePerson = {};
        //shoudl get only by id
        person.isEditable = false;
    };

    $scope.delete = function (person) {
        var modalInstance = $modal.open({
            templateUrl: '/Templates/Modals/deleteConfirmModal.html',
            controller: 'confirmModalController',
            size: 'sm',
            resolve: {
                modalResult: function () {
                    return $scope.modalResult;
                }
            }
        });

        modalInstance.result.then(function (modalResult) {
            if (modalResult !== undefined) {
                if (modalResult) {
                    $scope.deleteUser(person);

                }
            }
        });
    };

    $scope.showEdit = function (person) {
        if (!person) {
            return;
        }
        person.isEditable = true;
        $scope.editablePerson = angular.copy(person);

    };

    $scope.changeStatus = function (person) {
        if (!person) {
            return;
        }
        person.Status = person.selectedStatus == 'Aktywny' ? 1 : 2;

    };

    $scope.deleteUser = function (person) {
        if (!person) {
            return;
        }

        UtilitiesFactory.showSpinner();
        person.IsDeleted = true;
        $scope.viewModel.Current = person;
        $scope.viewModel.ActionType = 1;
        $http.post('/api/Person/', $scope.viewModel).success(function (data) {
            $scope.viewModel = data;
            UtilitiesFactory.hideSpinner();
        }).error(function (data) {
            $scope.viewModel.ErrorMessage = 'Wystąpił nieoczekiwany błąd podczas usuniecia uzytkownika' + data;
            UtilitiesFactory.hideSpinner();
        });
    }
}

adminUsersController.$inject = ['$scope', '$http', '$modal', 'UserFactory', 'UtilitiesFactory'];