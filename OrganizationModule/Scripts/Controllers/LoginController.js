var LoginController = function ($scope, $routeParams, $location, UserFactory, $rootScope) {
    $scope.loginForm = {
        emailAddress: '',
        password: ''
    };

    $scope.errorMessage = '';
    $scope.message = $routeParams.registered == true ? 'Twoje konto zostało utworzone. Możesz się zalogować' : '';

    $scope.login = function () {
        $scope.processing = true;
        var result = UserFactory.login($scope.loginForm.emailAddress, $scope.loginForm.password);
        result.then(function (result) {
            if (result.Succeeded) {
                $scope.processing = false;
                $rootScope.$broadcast('userChanged');
                angular.element('#content')
                       .removeClass('col-md-12')
                       .addClass('col-md-8 col-lg-9');

                 $location.path('/').search('');
            } else {
                $scope.errorMessage = result.Errors.join();
                $scope.processing = false;
            }
        });
    }
};

LoginController.$inject = ['$scope', '$routeParams', '$location', 'UserFactory', '$rootScope'];
