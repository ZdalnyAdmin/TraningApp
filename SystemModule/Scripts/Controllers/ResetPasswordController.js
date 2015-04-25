var ResetPasswordController = function ($scope, $routeParams, $location, LoginFactory) {
    $scope.resetForm = {
        Email: '',
        UserName: ''
    };

    $scope.errorMessage = '';

    $scope.resetPassword = function () {
        $scope.errorMessage = '';
        var result = LoginFactory.reset($scope.resetForm);

        result.then(function (data) {
            if (data.Succeeded) {
                $location.path('/').search('');
            } else {
                if (data.Errors) {
                    $scope.errorMessage = '';
                    angular.forEach(data.Errors, function (val) {
                        $scope.errorMessage += ' ' + val;
                    });
                } else {
                    $scope.errorMessage = 'Wystąpił nieoczekiwany błąd podczas resetowania hasła';
                }
            }
        });
    }
};

ResetPasswordController.$inject = ['$scope', '$routeParams', '$location', 'LoginFactory'];