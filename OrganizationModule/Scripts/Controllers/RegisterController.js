var RegisterController = function ($scope, $routeParams, $location, LoginFactory) {
    $scope.registrationData = {
        Email: 'email@mail.pl',
        Role: 'xxx'
    };

    $scope.company = {
        name: 'nazwa',
        displayName: 'xxx'
    };

    $scope.errorMessage = '';

    $scope.register = function () {
        $scope.errorMessage = '';
        var result = LoginFactory.register($scope.registrationData);

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
                    $scope.errorMessage = 'Wystąpił nieoczekiwany błąd podczas rejestracji';
                }
            }
        });
    }
};

RegisterController.$inject = ['$scope', '$routeParams', '$location', 'LoginFactory'];