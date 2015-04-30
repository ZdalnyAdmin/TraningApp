function adminUsersController($scope, $http, $modal) {
    $scope.loading = true;
    $scope.addMode = false;
    $scope.showEdit = false;
    $scope.editablePerson = {};
    $scope.availableStatus = ['Aktywny', 'Zablokowany'];
    //temp solution

    //Used to display the data 
    $scope.loadData = function () {
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
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });
    }

    $scope.loadData();

    $scope.loadDict = function () {
        $http.get('/api/Status').success(function (data) {
            $scope.status = data;
            $scope.loading = false;
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });
    }

    $scope.loadDict();
    //Used to save a record after edit 
    $scope.save = function (person) {
        if (!person) {
            return;
        }
        $scope.loading = true;
        $http.put('/api/Person/', person).success(function (data) {
            //shoudl get only by id
            $scope.loading = false;
            person.isEditable = false;

            $scope.editablePerson = {};
        }).error(function (data) {
            $scope.error = "An Error has occured while saving person! " + data;
            $scope.loading = false;
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
            templateUrl: '/Templates/deleteConfirmModal.html',
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

        person.IsDeleted = true;
        person.DeletedDate = new Date();
        //get from score - logged user id
        person.DeleteUserID = 1;
        $scope.person = person;

        $scope.loading = true;
        $http.put('/api/Person/', person).success(function (data) {
            $scope.loading = false;
            var index = 0;

            for (var i = 0; i < $scope.People.length; i++) {
                if ($scope.People[i].IsDeleted) {
                    index = i;
                    break;
                }
            }
            $scope.People.splice(index, 1);
            //$scope.People. = people;
            $scope.DeletePeople.push($scope.person);
        }).error(function (data) {
            $scope.error = "An Error has occured while deleting person! " + data;
            $scope.loading = false;
        });
    }
}

adminUsersController.$inject = ['$scope', '$http', '$modal'];