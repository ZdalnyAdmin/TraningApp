function statisticsController($scope, $http, $modal) {
    $scope.loading = true;
    $scope.list = [];
    $scope.current = {};
    $scope.index = 0;

    $scope.loadDate = function (type) {
        var stats = {};
        stats.Type = type;
        $http.post('/api/Statistics', stats).success(function (data) {
            if (!data) {
                return;
            }

            if (type == 3) {
                if (!!data.length && data.length >= 0) {
                    $scope.current = data[0];
                }

                $scope.current
            } else {
                $scope.list = data;
            }
            $scope.index = type;
            $scope.loading = false;
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });
    }

}

statisticsController.$inject = ['$scope', '$http', '$modal'];