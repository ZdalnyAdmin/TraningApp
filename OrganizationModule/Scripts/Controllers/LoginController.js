var LoginController = function ($scope, $routeParams, $location, UserFactory, $rootScope) {
    $scope.loginForm = {
        emailAddress: '',
        password: '',
        returnUrl: $routeParams.returnUrl
    };

    $scope.errorMessage = '';

    $scope.login = function () {
        $scope.processing = true;
        var result = UserFactory.login($scope.loginForm.emailAddress, $scope.loginForm.password);
        result.then(function (result) {
            if (result.Succeeded) {
                $scope.processing = false;
                $rootScope.$broadcast('userChanged');
                if ($scope.loginForm.returnUrl !== undefined) {
                    var search = $location.search();
                    delete search.returnUrl;

                    $location.path($scope.loginForm.returnUrl).search(search);
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
