function protectorAppLogsController($scope, $rootScope, $http, $modal, UserFactory, UtilitiesFactory) {
    //Used to display the data 
    $scope.loadOperationType = function () {
        UtilitiesFactory.showSpinner();
        $scope.currentItem = {};
        $scope.currentItem.ProtectorID = 1;
        $http.get('/api/OperationLog/').success(function (data) {

            $scope.OperationTypes = data;
            $scope.success = "";
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $rootScope.$broadcast('showGlobalMessage', {
                success: false,
                messageText: 'Wystąpił nieoczekiwany błąd podczas inicjalizacji danych'
            });
            UtilitiesFactory.hideSpinner();
        });

        //Used to display the data 

    }

    $scope.loadLogs = function () {
        UtilitiesFactory.showSpinner();
        $http.get('/api/Logs').success(function (data) {
            $scope.Logs = data;
            $scope.DbLogs = data;
            $scope.success = "";
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $rootScope.$broadcast('showGlobalMessage', {
                success: false,
                messageText: 'Wystąpił nieoczekiwany błąd podczas inicjalizacji danych'
            });
            UtilitiesFactory.hideSpinner();
        });

        //Used to display the data 

    }

    $scope.loadLogs();
    $scope.loadOperationType();


    $scope.change = function (obj) {
        UtilitiesFactory.showSpinner();
        $scope.Logs = [];

        if (!obj) {
            $scope.Logs = $scope.DbLogs
            UtilitiesFactory.hideSpinner();
            return;
        }

        angular.forEach($scope.DbLogs, function (item) {
            if (item.OperationDesc == obj) {
                $scope.Logs.push(item);
            }
        });
        UtilitiesFactory.hideSpinner();
    }
}

protectorAppLogsController.$inject = ['$scope', '$rootScope', '$http', '$modal', 'UserFactory', 'UtilitiesFactory'];