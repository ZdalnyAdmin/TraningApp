function creatorTraningListController($scope, $http, $modal) {
    $scope.loading = true;
    //Used to display the data 

    $scope.loadData = function () {

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

    $scope.loadData();


}

creatorTraningListController.$inject = ['$scope', '$http', '$modal'];