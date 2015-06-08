var LoginController = function ($scope, $routeParams, $location, UserFactory, UtilitiesFactory, $rootScope) {
    $scope.loginForm = {
        emailAddress: '',
        password: '',
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
            angular.element('#content')
                   .removeClass('col-md-12')
                   .addClass('col-md-8 col-lg-9');

            if (result.success) {
                    $location.path('/').search('');
            } else {
                $scope.loginForm.loginFailure = true;
                $scope.processing = false;
            }

        });
    }
};

LoginController.$inject = ['$scope', '$routeParams', '$location', 'UserFactory', 'UtilitiesFactory', '$rootScope'];
