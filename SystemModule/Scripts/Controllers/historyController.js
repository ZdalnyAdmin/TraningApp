function historyController($scope, $http, $modal) {
    $scope.loading = true;
    //Used to display the data 
    $scope.loadType = function () {

        $scope.currentItem = {};
        $scope.currentItem.ProtectorID = 1;
        $http.get('/api/SystemLog').success(function (data) {
            $scope.types = data;
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
    $scope.loadType();


    $scope.change = function (obj) {
        $scope.Logs = [];


    }
}

historyController.$inject = ['$scope', '$http', '$modal'];