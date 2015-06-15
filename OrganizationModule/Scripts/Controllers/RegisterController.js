var RegisterController = function ($scope, $routeParams, $location, UserFactory) {
    var search = $location.search();

    $scope.registrationData = {
        Token: search.code,
        UserId: search.id
    };

    $scope.errorMessage = '';

    $scope.register = function () {
        $scope.errorMessage = '';
        var result = UserFactory.register($scope.registrationData);

        result.then(function (data) {
            if (data.Succeeded) {
                $location.path('/signin').search({registered: true});
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

RegisterController.$inject = ['$scope', '$routeParams', '$location', 'UserFactory'];
