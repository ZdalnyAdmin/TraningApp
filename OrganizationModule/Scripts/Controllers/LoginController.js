var LoginController = function ($scope, $routeParams, $location, UserFactory, $rootScope) {
    $scope.loginForm = {
        emailAddress: '',
        password: ''
    };

    var result = UserFactory.getLoggedUser();

    result.then(function (user) {
        if (user && user.Id) {
            $location.path('/')
        }
    });

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

                var returnUrl = $location.search().returnUrl;

                if (returnUrl && ['/signin', '/resetPasswordConfirmation', '/register', '/logoff'].indexOf(returnUrl) === -1) {
                    $location.path(returnUrl).search('');
                } else {
                    $location.path('/').search('');
                }
            } else {
                $scope.errorMessage = result.Errors.join();
                $scope.processing = false;
            }
        });
    }
};

LoginController.$inject = ['$scope', '$routeParams', '$location', 'UserFactory', '$rootScope'];
