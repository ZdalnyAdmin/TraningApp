function settingsController($scope, $http, $modal) {
    $scope.loading = true;
    $scope.current = {};

    $scope.loadDate = function () {
        $http.post('/api/Settings', $scope.current).success(function (data) {
            $scope.current = data;
            $scope.loading = false;
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });
    }

    $scope.loadDate();

    $scope.save = function (obj) {
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

settingsController.$inject = ['$scope', '$http', '$modal'];