var ResetPasswordController = function ($scope, $routeParams, $location, UserFactory) {
    $scope.resetForm = {
        Email: '',
        UserName: ''
    };

    $scope.errorMessage = '';

    $scope.resetPassword = function () {
        $scope.errorMessage = '';
        var result = UserFactory.reset($scope.resetForm);

        result.then(function (data) {
            if (data.Succeeded) {
                $scope.errorMessage = 'Wiadomość z linkiem aktywacyjnym została wysłana pod Twój adres e­mail.';
            } else {
                if (data.Errors) {
                    $scope.errorMessage = '';
                    angular.forEach(data.Errors, function (val) {
                        $scope.errorMessage += ' ' + val;
                    });
                } else {
                    $scope.errorMessage = 'Coś poszło nie tak... spróbuj ponownie później';
                }
            }
        });
    }
};

ResetPasswordController.$inject = ['$scope', '$routeParams', '$location', 'UserFactory'];
