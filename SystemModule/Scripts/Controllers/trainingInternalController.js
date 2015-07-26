function trainingInternalController($scope, $rootScope, $http, $modal, UserFactory, UtilitiesFactory) {
    $scope.viewModel = {};

    $scope.loadDate = function () {
        UtilitiesFactory.showSpinner();

        $scope.viewModel.ActionType = 7;

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

    $scope.loadDate();
    
    $scope.showMore = function () {
        $scope.loadDate();
    }
}

trainingInternalController.$inject = ['$scope', '$rootScope', '$http', '$modal', 'UserFactory', 'UtilitiesFactory'];