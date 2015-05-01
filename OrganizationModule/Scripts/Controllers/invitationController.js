function invitationController($scope, $http, $modal, UserFactory, $route, $templateCache) {
    $scope.user = {
        Role: 5
    };

    $scope.errorMessage = '';

    $scope.invite = function () {
        var result = UserFactory.inviteUser($scope.user);
        result.then(processResponse);
    };

    $scope.deleteUser = function (id) {
        var result = UserFactory.removeInvitation({ Id: id });
        result.then(processResponse);
    };
    
    function processResponse (data){
        if (data.Succeeded) {
            reload();
        } else {
            if (data.Errors) {
                $scope.errorMessage = '';
                angular.forEach(data.Errors, function (val) {
                    $scope.errorMessage += ' ' + val;
                });
            } else {
                $scope.errorMessage = 'Wystąpił nieoczekiwany błąd podczas rejestracji';
            }
        }
    }

    function reload() {
        var currentPageTemplate = $route.current.templateUrl;
        $templateCache.remove(currentPageTemplate);
        $route.reload();
    }
}

invitationController.$inject = ['$scope', '$http', '$modal', 'UserFactory', '$route', '$templateCache'];