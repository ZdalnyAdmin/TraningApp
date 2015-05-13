function createProtectorController($scope, $http, $modal, UserFactory, UtilitiesFactory) {
    $scope.viewModel = {};

    $scope.loadOrganization = function () {

        UtilitiesFactory.showSpinner();

        UtilitiesFactory.showSpinner();
        $scope.viewModel.ActionType = 7;

        $http.post('/api/Organizations/', $scope.viewModel).success(function (data) {
            $scope.viewModel = data;
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.viewModel.ErrorMessage = 'Wystąpił nieoczekiwany błąd podczas inicjalizacji danych';
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.loadOrganization();

    $scope.save = function () {
        if (!$scope.viewModel.Protector.UserName || !$scope.viewModel.Protector.Email) {
            return;
        }

        UtilitiesFactory.showSpinner();

        $scope.errorMessage = '';
        var result = UserFactory.registerOperator($scope.viewModel.Protector);

        result.then(function (data) {
            if (!data.Succeeded) {
                $scope.viewModel.Protector = {};
                $scope.viewModel.Protector.Profile = 4;
            }
            else {
                if (data.Errors) {
                    $scope.errorMessage = '';
                    angular.forEach(data.Errors, function (val) {
                        $scope.errorMessage += ' ' + val;
                    });
                } else {
                    $scope.errorMessage = 'Wystąpił nieoczekiwany błąd podczas rejestracji operatora';
                }
            }

            UtilitiesFactory.hideSpinner();
        });

    }

    $scope.changeOrganization = function (selected) {
        if (!selected) {
            $scope.viewModel.Protector.OrganizationID = 0;
            return;
        }
        $scope.viewModel.Protector.OrganizationID = selected.Id;
    }
}

createProtectorController.$inject = ['$scope', '$http', '$modal', 'UserFactory', 'UtilitiesFactory'];