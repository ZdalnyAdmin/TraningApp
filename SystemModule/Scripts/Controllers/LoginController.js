var LoginController = function ($scope, $routeParams, $location, UserFactory, UtilitiesFactory, $rootScope) {
    $scope.loginForm = {
        emailAddress: '',
        password: '',
        returnUrl: $routeParams.returnUrl,
        loginFailure: false
    };

    $scope.login = function () {
        $scope.processing = true;
        UtilitiesFactory.showSpinner();
        var result = UserFactory.login($scope.loginForm.emailAddress, $scope.loginForm.password);
        result.then(function (result) {
            UtilitiesFactory.hideSpinner();
            $scope.processing = false;
            $rootScope.$broadcast('userChanged');

            if (result.success) {
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

LoginController.$inject = ['$scope', '$routeParams', '$location', 'UserFactory', 'UtilitiesFactory', '$rootScope'];
