function historyController($scope, $http, $modal, UserFactory, UtilitiesFactory) {
    $scope.viewModel = {};


    $scope.loadLogs = function () {
        UtilitiesFactory.showSpinner();
        $http.get('/api/Logs/').success(function (data) {
            $scope.viewModel = data[0];
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.viewModel.ErrorMessage = 'Wystąpił nieoczekiwany błąd podczas inicjalizacji danych';
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.loadLogs();


    $scope.change = function (obj) {
        $scope.viewModel.DisplayLogs = [];
        UtilitiesFactory.showSpinner();
        if (!obj) {
            $scope.viewModel.DisplayLogs = $scope.viewModel.Logs;
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

historyController.$inject = ['$scope', '$http', '$modal', 'UserFactory', 'UtilitiesFactory'];