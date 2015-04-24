var RegisterController = function ($scope, $routeParams, $location, LoginFactory) {
    $scope.registrationData = {
        emailAddress: 'email@mail.pl',
        login: '',
        password: '',
        repeatedPassword: '',
        role: 'xxx'
    };

    $scope.company = {
        name: 'nazwa',
        displayName: 'xxx'
    };

    $scope.register = function () {
        var result = LoginFactory.login($scope.registrationData);
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

RegisterController.$inject = ['$scope', '$routeParams', '$location', 'LoginFactory'];