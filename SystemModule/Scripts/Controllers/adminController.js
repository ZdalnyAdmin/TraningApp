function adminController($scope, $http, $modal) {
    $scope.loading = true;
    $scope.list = data;

    $scope.loadDate = function () {
        $http.get('/api/Admin').success(function (data) {
            $scope.list = data;
            $scope.loading = false;
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });
    }

    $scope.loadDate();
}

adminController.$inject = ['$scope', '$http', '$modal'];