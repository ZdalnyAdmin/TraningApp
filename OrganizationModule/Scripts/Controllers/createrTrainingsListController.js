function createrTrainingsListController($scope, $http, $modal, $location, UserFactory, UtilitiesFactory) {
    $scope.viewModel = {};

    $scope.loadTrainings = function () {
        UtilitiesFactory.showSpinner();
        $scope.viewModel.ActionType = 9;
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
            return;
        }

        //call view 
        $location.path('/' + 'creatorTrainings/').search({ trainingID: item.TrainingID });
    }
}

createrTrainingsListController.$inject = ['$scope', '$http', '$modal', '$location', 'UserFactory', 'UtilitiesFactory'];