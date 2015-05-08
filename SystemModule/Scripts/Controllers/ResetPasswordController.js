var ResetPasswordController = function ($scope, $routeParams, $location, UserFactory, UtilitiesFactory) {
    $scope.resetForm = {
        Email: '',
        UserName: ''
    };

    $scope.errorMessage = '';

    $scope.resetPassword = function () {
        UtilitiesFactory.showSpinner();
        $scope.errorMessage = '';
        var result = UserFactory.reset($scope.resetForm);

        result.then(function (data) {
            UtilitiesFactory.hideSpinner();
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

ResetPasswordController.$inject = ['$scope', '$routeParams', '$location', 'UserFactory', 'UtilitiesFactory'];
