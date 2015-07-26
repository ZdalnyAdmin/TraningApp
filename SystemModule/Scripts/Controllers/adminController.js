function adminController($scope, $rootScope, $http, $modal, UserFactory, UtilitiesFactory) {
    $scope.viewModel = {};

    $scope.loadDate = function () {
        UtilitiesFactory.showSpinner();
        $scope.viewModel.ActionType = 5;

        $http.post('/api/User/', $scope.viewModel).success(function (data) {
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
}

adminController.$inject = ['$scope', '$rootScope', '$http', '$modal', 'UserFactory', 'UtilitiesFactory'];