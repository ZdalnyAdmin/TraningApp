var RegisterController = function ($scope, $routeParams, $location, UserFactory, UtilitiesFactory) {
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
        UtilitiesFactory.showSpinner();
        $scope.errorMessage = '';
        var result = UserFactory.register($scope.registrationData);

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
                    $scope.errorMessage = 'Wystąpił nieoczekiwany błąd podczas rejestracji';
                }
            }
        });
    }
};

RegisterController.$inject = ['$scope', '$routeParams', '$location', 'UserFactory', 'UtilitiesFactory'];
