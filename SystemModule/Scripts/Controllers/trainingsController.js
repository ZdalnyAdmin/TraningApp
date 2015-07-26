function trainingsController($scope, $rootScope, $http, $modal, $location, UserFactory, UtilitiesFactory) {
    $scope.viewModel = {};

    $scope.loadTrainings = function () {
        UtilitiesFactory.showSpinner();
        $scope.viewModel.ActionType = 4;
        $http.post('/api/Training/', $scope.viewModel)
        .success(function (data) {
            $scope.viewModel = data;

            if ($scope.viewModel && $scope.viewModel.Success) {
                $rootScope.$broadcast('showGlobalMessage', {
                    success: true,
                    messageText: $scope.viewModel.Success
                });
            }

            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $rootScope.$broadcast('showGlobalMessage', {
                success: false,
                messageText: 'Wystąpił nieoczekiwany błąd podczas inicjalizacji danych'
            });

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

trainingsController.$inject = ['$scope', '$rootScope', '$http', '$modal', '$location', 'UserFactory', 'UtilitiesFactory'];