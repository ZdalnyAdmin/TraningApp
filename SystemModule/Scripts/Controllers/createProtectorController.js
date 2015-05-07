function createProtectorController($scope, $http, $modal, UserFactory, UtilitiesFactory) {
    $scope.loading = true;
    $scope.organizations = [];
    $scope.current = {};

    $scope.loadOrganization = function () {

        UtilitiesFactory.showSpinner();


        $http.get('/api/NotProtectedOrganization')
        .success(function (data) {
            if (!data) {
                return;
            }
            $scope.organizations = data;
            $scope.loading = false;

            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.errorMessage = "Wystąpił nieoczekiwany błąd podczas pobierania organizacji!";
            $scope.loading = false;

            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.loadOrganization();

    $scope.save = function () {
        if (!$scope.current.UserName || !$scope.current.Email || !$scope.current.Organization) {
            return;
        }

        UtilitiesFactory.showSpinner();

        $scope.current.OrganizationID = $scope.current.Organization.OrganizationID;

        $scope.errorMessage = '';
        var result = UserFactory.registerOperator($scope.current);

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