var LoginController = function ($scope, $routeParams, $location, LoginFactory) {
    $scope.loginForm = {
        emailAddress: '',
        password: '',
        returnUrl: $routeParams.returnUrl,
        loginFailure: false
    };

    $scope.login = function () {
        var result = LoginFactory.login($scope.loginForm.emailAddress, $scope.loginForm.password);
        result.then(function (result) {
            if (result.success) {
                if ($scope.loginForm.returnUrl !== undefined) {
                    $location.path($scope.loginForm.returnUrl).search('');
                } else {
                    $location.path('/').search('');
                }
            } else {
                $scope.loginForm.loginFailure = true;
            }
        });
    }
};

LoginController.$inject = ['$scope', '$routeParams', '$location', 'LoginFactory'];