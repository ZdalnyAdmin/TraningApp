function protectorAppLogsController($scope, $http, $modal) {
    $scope.loading = true;
    //Used to display the data 
    $scope.loadOperationType = function () {

        $scope.currentItem = {};
        $scope.currentItem.ProtectorID = 1;
        $http.get('/api/OperationLog').success(function (data) {
            $scope.OperationTypes = data;
            $scope.loading = false;
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });

        //Used to display the data 

    }

    $scope.loadLogs = function () {

        $http.get('/api/Logs').success(function (data) {
            $scope.Logs = data;
            $scope.DbLogs = data;
            $scope.loading = false;
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });

        //Used to display the data 

    }

    $scope.loadLogs();
    $scope.loadOperationType();


    $scope.change = function (obj) {
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
    }
}

protectorAppLogsController.$inject = ['$scope', '$http', '$modal'];