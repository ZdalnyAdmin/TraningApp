var ResetPasswordConfirmation = function ($scope, $routeParams, $location, LoginFactory) {
    var search = $location.search();
    if(!search.code) {
        $location.path('/').search('');
        return;
    }

    $scope.resetPasswordData = {
            code: search.code
        };

    $scope.errorMessage = '';

    $scope.confirmResetPassword = function () {
        $scope.errorMessage = '';
        var result = LoginFactory.resetPasswordConfirmation($scope.resetPasswordData);

        result.then(function (data) {
            if (data.Succeeded) {
                $location.path('/login').search('');
            } else {
                if (data.Errors) {
                    $scope.errorMessage = '';
                    angular.forEach(data.Errors, function (val) {
                        $scope.errorMessage += ' ' + val;
                    });
                } else {
                    $scope.errorMessage = 'Wystąpił nieoczekiwany błąd podczas zmiany hasła';
                }
            }
        });
    }
};

ResetPasswordConfirmation.$inject = ['$scope', '$routeParams', '$location', 'LoginFactory'];