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

    $scope.loadOperationType();


    $scope.change = function (selected) {

    }
}

protectorAppLogsController.$inject = ['$scope', '$http', '$modal'];