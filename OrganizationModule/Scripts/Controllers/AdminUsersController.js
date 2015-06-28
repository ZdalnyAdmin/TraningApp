function adminUsersController($scope, $http, $modal, UserFactory, UtilitiesFactory) {
    $scope.viewModel = {};
    $scope.editablePerson = {};
    $scope.availableStatus = ['Aktywny', 'Zablokowany'];
    $scope.availableProfiles = ['Administrator', 'Manager', 'Twórca', 'Opiekun', 'Uzytkownik'];
    //temp solution

    //Used to display the data 
    $scope.loadData = function () {
        UtilitiesFactory.showSpinner();
        $scope.viewModel.ActionType = 0;
        $http.post('/api/Person/', $scope.viewModel).success(function (data) {
            $scope.viewModel = data;

            angular.forEach($scope.viewModel.People, function (item) {
                if (item.Profile == 1) {
                    item.ProfileName = 'Administrator';
                } else if (item.Profile == 2) {
                    item.ProfileName = 'Manager';
                } else if (item.Profile == 3) {
                    item.ProfileName = 'Twórca';
                } else if (item.Profile == 4) {
                    item.ProfileName = 'Opiekun';
                } else if (item.Profile == 5) {
                    item.ProfileName = 'Uzytkownik';
                }

                if (item.Status == 1) {
                    item.selectedStatus = 'Aktywny';
                } else if (item.Status == 2) {
                    item.selectedStatus = 'Zablokowany';
                }
            });

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
        person.Profile = $scope.editablePerson.Profile;
        person.DisplayName = $scope.editablePerson.DisplayName;

        if (person.Profile == 1) {
            person.ProfileName = 'Administrator';
        } else if (person.Profile == 2) {
            person.ProfileName = 'Manager';
        } else if (person.Profile == 3) {
            person.ProfileName = 'Twórca';
        } else if (person.Profile == 4) {
            person.ProfileName = 'Opiekun';
        } else if (person.Profile == 5) {
            person.ProfileName = 'Uzytkownik';
        }

        if (person.Status == 1) {
            item.selectedStatus = 'Aktywny';
        } else if (person.Status == 2) {
            item.selectedStatus = 'Zablokowany';
        }

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

        var modalInstance = $modal.open({
            templateUrl: '/Templates/Modals/editUserModal.html',
            controller: 'editUserModalController',
            size: 'sm',
            resolve: {
                user: function () {
                    return person;
                }
            }
        });

        modalInstance.result.then(function (obj) {
            if (!!obj) {
                $scope.save(obj);
            }
            else
            {
                $scope.cancel(person);
            }
        });

    };

    $scope.changeStatus = function (person) {
        if (!person) {
            return;
        }
        person.Status = person.selectedStatus == 'Aktywny' ? 1 : 2;

    };

    $scope.changeProfile = function (person) {
        if (!person) {
            return;
        }

        if (person.ProfileName == 'Administrator') {
            person.Profile = 1;
        } else if (person.ProfileName == 'Manager') {
            person.Profile = 2;
        } else if (person.ProfileName == 'Twórca') {
            person.Profile = 3;
        } else if (person.ProfileName == 'Opiekun') {
            person.Profile = 4;
        } else if (person.ProfileName == 'Uzytkownik') {
            person.Profile = 5;
        }


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

            angular.forEach($scope.viewModel.People, function (item) {
                if (item.Profile == 1) {
                    item.ProfileName = 'Administrator';
                } else if (item.Profile == 2) {
                    item.ProfileName = 'Manager';
                } else if (item.Profile == 3) {
                    item.ProfileName = 'Twórca';
                } else if (item.Profile == 4) {
                    item.ProfileName = 'Opiekun';
                } else if (item.Profile == 5) {
                    item.ProfileName = 'Uzytkownik';
                }

                if (item.Status == 1) {
                    item.selectedStatus = 'Aktywny';
                } else if (item.Status == 2) {
                    item.selectedStatus = 'Zablokowany';
                }
            });

            UtilitiesFactory.hideSpinner();
        }).error(function (data) {
            $scope.viewModel.ErrorMessage = 'Wystąpił nieoczekiwany błąd podczas usuniecia uzytkownika' + data;
            UtilitiesFactory.hideSpinner();
        });
    }
}

adminUsersController.$inject = ['$scope', '$http', '$modal', 'UserFactory', 'UtilitiesFactory'];