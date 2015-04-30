function invitationController($scope, $http, $modal, UserFactory) {
    $scope.user = {
        Role: 5
    };

    $scope.invite = function () {
        UserFactory.inviteUser($scope.user);
    };
    
    $scope.reload = function () {

    }
}

invitationController.$inject = ['$scope', '$http', '$modal', 'UserFactory'];