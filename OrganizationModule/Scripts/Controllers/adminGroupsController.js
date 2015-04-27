function adminGroupsController($scope, $http, $modal) {
    $scope.loading = true;
    $scope.currentGroup = {};

    //Used to display the data 


    $scope.loadData = function () {
        $http.get('/api/Group').success(function (data) {
            $scope.Groups = data;
            $scope.loading = false;
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });
    }

    $scope.loadData();

    $scope.add = function () {
        $scope.loading = true;

        //if ($scope.currentGroup)

        $http.post('/api/Group/', $scope.currentGroup).success(function (data) {
            group = data;
            $scope.loading = false;
            //move to other methods
            $scope.assignedPeopleToGroup(group, true);
        }).error(function (data) {
            $scope.error = "An Error has occured while creating group! " + data;
            $scope.loadData();
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

        $scope.assignedPeopleToGroup(group, false)
    };

    $scope.assignedPeopleToGroup = function (group, refresh) {
        if (!!$scope.selectedUsers) {
            var list = [];
            for (var i = 0; i < $scope.selectedUsers.length; i++) {
                var obj = {};
                obj.ProfileGroupID = group.ProfileGroupID;
                obj.PersonID = $scope.selectedUsers[i].PersonID;
                obj.IsDeleted = false;
                list.push(obj);
            }
            $http.post('/api/PeopleInGroup/', list).success(function (data) {
                group = data;
                $scope.loading = false;
                if(refresh)
                {
                    $scope.loadData();
                }

            }).error(function (data) {
                $scope.error = "An Error has occured while saving group! " + data;
                $scope.loading = false;
            });
        }
    }

    //Used to save a record after edit 
    $scope.cancel = function (group) {
        if (!group) {
            return;
        }
        $scope.loading = true;
        $scope.loadData();
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

    $scope.chooseUsers = function (group) {
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
            if (!!selectedUsers) {
                $scope.selectedUsers = selectedUsers;
                for (var i = 0; i < selectedUsers.length; i++) {
                    if (group.AssignedPeopleDisplay != '') {
                        group.AssignedPeopleDisplay += ', ';
                    }
                    group.AssignedPeopleDisplay += selectedUsers[i].Name;
                }
            }
        });
    };
}

adminGroupsController.$inject = ['$scope', '$http', '$modal'];
