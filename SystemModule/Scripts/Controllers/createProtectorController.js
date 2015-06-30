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

        if ($scope.viewModel.Protector.UserName.length < 3 || $scope.viewModel.Protector.UserName.length > 30)
        {
            $scope.viewModel.ErrorMessage = "Nazwa opiekuna musi zawierac miedzy 3 a 30 znakow";
            return;
        }

        var re = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;
        if (!re.test($scope.viewModel.Protector.Email))
        {
            $scope.viewModel.ErrorMessage = "Bledny format adresu email";
            return;
        }

        if (!$scope.viewModel.Protector.OrganizationID || $scope.viewModel.Protector.OrganizationID == 0)
        {
            $scope.viewModel.ErrorMessage = "Nalezy wybrac organizacje";
            return;
        }

        $scope.OrganizationID = $scope.viewModel.Protector.OrganizationID;

        UtilitiesFactory.showSpinner();

        $scope.viewModel.ErrorMessage = '';
        $scope.viewModel.Success = '';
        var result = UserFactory.registerOperator($scope.viewModel.Protector);

        result.then(function (data) {
            if (data.Succeeded) {
                $scope.viewModel.Protector = {};
                $scope.viewModel.Protector.Profile = 4;
                $scope.viewModel.Success = 'Użytkownik został zaproszony';
                
                var index = 0;
                for (var i = 0; i < $scope.viewModel.NotAssigned.length; i++) {
                    if ($scope.viewModel.NotAssigned[i].Id == $scope.OrganizationID) {
                        index = i;
                        break;
                    }
                }
                $scope.viewModel.NotAssigned.splice(index, 1);
                

            }
            else {
                if (data.Errors) {
                    $scope.viewModel.errorMessage = '';
                    angular.forEach(data.Errors, function (val) {
                        $scope.viewModel.errorMessage += ' ' + val;
                    });
                } else {
                    $scope.viewModel.errorMessage = 'Wystąpił nieoczekiwany błąd podczas rejestracji operatora';
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