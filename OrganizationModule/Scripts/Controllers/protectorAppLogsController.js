function protectorAppLogsController($scope, $http, $modal, UserFactory, UtilitiesFactory) {
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
            $scope.error = "An Error has occured while loading posts!";
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
            $scope.error = "An Error has occured while loading posts!";
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

protectorAppLogsController.$inject = ['$scope', '$http', '$modal', 'UserFactory', 'UtilitiesFactory'];