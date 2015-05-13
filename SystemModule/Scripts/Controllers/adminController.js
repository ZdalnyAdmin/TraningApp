function adminController($scope, $http, $modal, UserFactory, UtilitiesFactory) {
    $scope.viewModel = {};

    $scope.loadDate = function () {
        UtilitiesFactory.showSpinner();
        $scope.viewModel.ActionType = 5;

        $http.post('/api/User/', $scope.viewModel).success(function (data) {
            $scope.viewModel = data;
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.viewModel.ErrorMessage = 'Wystąpił nieoczekiwany błąd podczas inicjalizacji danych';
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.loadDate();
}

adminController.$inject = ['$scope', '$http', '$modal', 'UserFactory', 'UtilitiesFactory'];