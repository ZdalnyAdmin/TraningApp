var LoginController = function ($scope, $routeParams, $location, UserFactory) {
    $scope.loginForm = {
        emailAddress: '',
        password: '',
        returnUrl: $routeParams.returnUrl,
        loginFailure: false
    };

    $scope.login = function () {
        $scope.processing = true;
        var result = UserFactory.login($scope.loginForm.emailAddress, $scope.loginForm.password);
        result.then(function (result) {
            if (result.success) {
                $scope.processing = false;
                if ($scope.loginForm.returnUrl !== undefined) {
                    $location.path($scope.loginForm.returnUrl).search('');
                } else {
                    $location.path('/').search('');
                }
            } else {
                $scope.loginForm.loginFailure = true;
                $scope.processing = false;
            }
        });
    }
};

LoginController.$inject = ['$scope', '$routeParams', '$location', 'UserFactory'];
