function historyController($scope, $rootScope, $http, $modal, UserFactory, UtilitiesFactory) {
    $scope.viewModel = {};


    $scope.loadLogs = function () {
        UtilitiesFactory.showSpinner();
        $http.get('/api/Logs/').success(function (data) {
            $scope.viewModel = data[0];

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

    $scope.loadLogs();


    $scope.change = function (obj) {
        $scope.viewModel.DisplayLogs = [];
        UtilitiesFactory.showSpinner();
        if (!obj) {
            $scope.viewModel.DisplayLogs = $scope.viewModel.Logs;
            UtilitiesFactory.hideSpinner();
            return;
        }

        angular.forEach($scope.viewModel.Logs, function (item) {
            if (item.SystemType == obj.Type) {
                $scope.viewModel.DisplayLogs.push(item);
            }
        });
        UtilitiesFactory.hideSpinner();
    }
}

historyController.$inject = ['$scope', '$rootScope', '$http', '$modal', 'UserFactory', 'UtilitiesFactory'];