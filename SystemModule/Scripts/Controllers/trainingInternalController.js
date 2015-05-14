function trainingInternalController($scope, $http, $modal, UserFactory, UtilitiesFactory) {
    $scope.viewModel = {};

    $scope.loadDate = function () {
        UtilitiesFactory.showSpinner();

        $scope.viewModel.ActionType = 7;

        $http.post('/api/Training/', $scope.viewModel)
        .success(function (data) {
            $scope.viewModel = data;
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.viewModel.ErrorMessage = 'Wystąpił nieoczekiwany błąd podczas inicjalizacji danych';
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.loadDate();
    
    $scope.showMore = function () {
        $scope.loadDate();
    }
}

trainingInternalController.$inject = ['$scope', '$http', '$modal', 'UserFactory', 'UtilitiesFactory'];