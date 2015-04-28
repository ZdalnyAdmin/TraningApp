function protectorPermissionsController($scope, $http, $modal) {
    $scope.loading = true;
    $scope.currentItem = {};
    //Used to display the data 
    $scope.loadData = function () {

        $scope.currentItem = {};
        $scope.currentItem.ProtectorID = 1;
        $http.post('/api/Settings', $scope.currentItem).success(function (data) {
            $scope.currentItem = data;
            $scope.loading = false;
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });

        //Used to display the data 

    }

    $scope.loadData();

    $scope.edit = function (obj) {
        if (!obj) {
            return;
        }
        $http.put('/api/Settings', obj).success(function (data) {
            $scope.loading = false;
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });

    }
}

protectorPermissionsController.$inject = ['$scope', '$http', '$modal'];