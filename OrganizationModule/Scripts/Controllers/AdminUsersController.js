function adminUsersController($scope, $http, $modal, UserFactory, UtilitiesFactory) {
    $scope.loading = true;
    $scope.addMode = false;
    $scope.showEdit = false;
    $scope.editablePerson = {};
    $scope.availableStatus = ['Aktywny', 'Zablokowany'];
    //temp solution

    //Used to display the data 
    $scope.loadData = function () {
        UtilitiesFactory.showSpinner();

        $http.get('/api/Person').success(function (data) {
            var people = [];
            var deletePeople = [];
            angular.forEach(data, function (item) {
                if (item.IsDeleted) {
                    deletePeople.push(item);
                }
                else {
                    item.isEditable = false;
                    item.selectedStatus = item.Status == 1 ? 'Aktywny' : 'Zablokowany';
                    people.push(item);
                }
            });
            $scope.People = people;
            $scope.DeletePeople = deletePeople;
            $scope.loading = false;
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.error = 'Wystąpił nieoczekiwany błąd podczas pobierania uzytkownikow';
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
        $scope.loading = true;
        $http.put('/api/Person/', person).success(function (data) {
            //shoudl get only by id
            $scope.loading = false;
            person.isEditable = false;

            $scope.editablePerson = {};

            UtilitiesFactory.hideSpinner();
        }).error(function (data) {
            $scope.error = 'Wystąpił nieoczekiwany błąd podczas zapisywania uzytkownika' + data;
            $scope.loading = false;
            UtilitiesFactory.hideSpinner();
        });
    };

    //Used to save a record after edit 
    $scope.cancel = function (person) {
        if (!person) {
            return;
        }
        $scope.loading = true;
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
        person.DeletedDate = new Date();
        //get from score - logged user id
        person.DeleteUserID = 1;
        $scope.person = person;

        $scope.loading = true;
        $http.put('/api/Person/', person).success(function (data) {
            $scope.loading = false;
            var index = 0;

            var result = UserFactory.deleteUser(person);

            result.then(function (data) {
                if (!data.Succeeded) {
                    if (data.Errors) {
                        $scope.errorMessage = '';
                        angular.forEach(data.Errors, function (val) {
                            $scope.errorMessage += ' ' + val;
                        });
                    } else {
                        $scope.error = 'Wystąpił nieoczekiwany błąd podczas usuniecia uzytkownika';
                    }
                }
            });

            for (var i = 0; i < $scope.People.length; i++) {
                if ($scope.People[i].IsDeleted) {
                    index = i;
                    break;
                }
            }
            $scope.People.splice(index, 1);
            //$scope.People. = people;
            $scope.DeletePeople.push($scope.person);

            UtilitiesFactory.hideSpinner();
        }).error(function (data) {
            $scope.error = 'Wystąpił nieoczekiwany błąd podczas usuniecia uzytkownika' + data;
            $scope.loading = false;
            UtilitiesFactory.hideSpinner();
        });
    }
}

adminUsersController.$inject = ['$scope', '$http', '$modal', 'UserFactory', 'UtilitiesFactory'];