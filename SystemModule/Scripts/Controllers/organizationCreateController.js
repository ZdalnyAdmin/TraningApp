function organizationCreateController($scope, $http, $modal, UserFactory, UtilitiesFactory) {
    $scope.current = {};

    $scope.loadDate = function () {
        $scope.current = {};
        UtilitiesFactory.showSpinner();

        $scope.current.OrganizationID = -1;
        $http.post('/api/Organizations', $scope.current).success(function (data) {
            $scope.current = data;
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.loadDate();

    $scope.save = function () {
        if (!$scope.current || !$scope.current.Name) {
            return;
        }

        UtilitiesFactory.showSpinner();

        $scope.current.OrganizationID = 0;
        $http.post('/api/Organizations', $scope.current).success(function (data) {

            var result = UserFactory.organizationCreateMail($scope.current);

            UtilitiesFactory.hideSpinner();

            result.then(function (data) {
                if (data.Succeeded) {
                    $scope.loadDate();
                } else {
                    if (data.Errors) {
                        $scope.errorMessage = '';
                        angular.forEach(data.Errors, function (val) {
                            $scope.errorMessage += ' ' + val;
                        });
                    } else {
                        $scope.errorMessage = 'Wystąpił nieoczekiwany błąd podczas rejestracji organizacji';
                    }
                }
            });           
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            UtilitiesFactory.hideSpinner();
        });
    }
}

organizationCreateController.$inject = ['$scope', '$http', '$modal', 'UserFactory', 'UtilitiesFactory'];