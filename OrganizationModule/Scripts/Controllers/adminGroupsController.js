function adminGroupsController($scope, $http, $modal) {
    $scope.loading = true;
    $scope.currentGroup = {};

    //Used to display the data 
    $http.get('/api/Group').success(function (data) {
        $scope.Groups = data;
        $scope.loading = false;
    })
    .error(function () {
        $scope.error = "An Error has occured while loading posts!";
        $scope.loading = false;
    });

    //get people
    $http.get('/api/SimplePerson').success(function (data) {
        $scope.People = data;
        $scope.loading = false;
    })
    .error(function () {
        $scope.error = "An Error has occured while loading posts!";
        $scope.loading = false;
    });

    $scope.add = function () {
        $scope.loading = true;

        //if ($scope.currentGroup)

        $http.post('/api/Group/', $scope.currentGroup).success(function (data) {
            person = data;
            $scope.loading = false;
            //move to other methods
            $http.get('/api/Group').success(function (data) {
                $scope.Groups = data;
                $scope.loading = false;
            })
           .error(function () {
               $scope.error = "An Error has occured while loading posts!";
               $scope.loading = false;
           });



        }).error(function (data) {
            $scope.error = "An Error has occured while creating group! " + data;
            $scope.loading = false;
        });
    };

    $scope.edit = function (group) {
        if (!group) {
            return;
        }
        $scope.loading = true;

        $http.put('/api/Group/', group).success(function (data) {
            group = data;


            $scope.loading = false;
        }).error(function (data) {
            $scope.error = "An Error has occured while saving group! " + data;
            $scope.loading = false;
        });
    };

    //Used to save a record after edit 
    $scope.cancel = function (group) {
        if (!group) {
            return;
        }
        $scope.loading = true;
        //

        $http.get('/api/Group/', group).success(function (data) {
            person = data;
            $scope.loading = false;
        }).error(function (data) {
            $scope.error = "An Error has occured while restore group data! " + data;
            $scope.loading = false;
        });
    };

    $scope.delete = function (group) {
        if (!group) {
            return;
        }
        $scope.loading = true;
        group.IsDeleted = true;
        //person.DeletedDate = Date.UTC;
        //get from score - logged user id
        //person.DeleteUserID

        $http.put('/api/Group/', group).success(function (data) {
            group = data;
            var index = 0;

            for (var i = 0; i < $scope.Groups.length; i++) {
                if ($scope.Groups[i].IsDeleted) {
                    index = i;
                    break;
                }
            }
            $scope.Groups.splice(index, 1);

            $scope.loading = false;
        }).error(function (data) {
            $scope.error = "An Error has occured while deleting group! " + data;
            $scope.loading = false;
        });
    };

    $scope.chooseUsers = function () {
        var modalInstance = $modal.open({
            templateUrl: '/Templates/usersListModal.html',
            controller: 'usersListModalController',
            size: 'sm',
            resolve: {
                selectedUsers: function () {
                    return $scope.selectedUsers;
                }
            }
        });

        modalInstance.result.then(function (selectedUsers) {
            $scope.selectedUsers = selectedUsers;
        });
    };
}

adminGroupsController.$inject = ['$scope', '$http', '$modal'];