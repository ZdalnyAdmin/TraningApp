var usersListModalController = function ($scope, $http, $modalInstance) {

    $scope.loadPeople = function () {
        $http.get('/api/SimplePerson').success(function (data) {
            $scope.users = data;
            $scope.loading = false;
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });
    }

    $scope.loadPeople();

    $scope.close = function () {
        $modalInstance.close();
    };

    $scope.save = function () {
        var selectedUsers = new Array();
        angular.forEach($scope.users, function (user) {
            if (user.selected) {
                selectedUsers.push(user);
            }
        });

        $modalInstance.close(selectedUsers);
    };
};

usersListModalController.$inject = ['$scope', '$http', '$modalInstance'];