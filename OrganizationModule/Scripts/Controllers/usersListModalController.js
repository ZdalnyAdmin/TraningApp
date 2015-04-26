var usersListModalController = function ($scope, $modalInstance) {
    $scope.users = [
        {
            selected: false,
            UserName: 'User 1',
            Email: 'User1@gmail.com'
        },
        {
            selected: true,
            UserName: 'User 2',
            Email: 'User2@gmail.com'
        }
    ];

    $scope.close = function () {
        var selectedUsers = new Array();
        angular.forEach($scope.users, function (user) {
            if (user.selected) {
                selectedUsers.push(user);
            }
        });

        $modalInstance.close(selectedUsers);
    };
};

usersListModalController.$inject = ['$scope', '$modalInstance'];