function trainingsController($scope, $http, $modal, $location, UserFactory, UtilitiesFactory) {
    $scope.viewModel = {};

    $scope.loadTrainings = function () {
        UtilitiesFactory.showSpinner();
        $scope.viewModel.ActionType = 4;
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

    $scope.loadTrainings();

    $scope.edit = function (item) {
        if (!item) {

        }
        //call view 
        $location.path('/currentTraining/').search({ trainingID: item.TrainingID });
    }
}

trainingsController.$inject = ['$scope', '$http', '$modal', '$location', 'UserFactory', 'UtilitiesFactory'];