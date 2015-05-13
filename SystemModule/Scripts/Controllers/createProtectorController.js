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
        //todo
        $scope.viewModel.Protector.OrganizationID = $scope.current.Organization.OrganizationID;

        $scope.errorMessage = '';
        var result = UserFactory.registerOperator($scope.current);

        result.then(function (data) {
            if (!data.Succeeded) {
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
            $scope.current.Organization = undefined;
            return;
        }
        $scope.current.Organization = selected;
    }
}

createProtectorController.$inject = ['$scope', '$http', '$modal', 'UserFactory', 'UtilitiesFactory'];