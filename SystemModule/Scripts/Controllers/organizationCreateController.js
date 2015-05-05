function organizationCreateController($scope, $http, $modal) {
    $scope.loading = true;
    $scope.current = {};

    $scope.loadDate = function () {
        $scope.current.OrganizationID = -1;
        $http.post('/api/Organizations', $scope.current).success(function (data) {
            $scope.current = data;
            $scope.loading = false;
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });
    }

    $scope.loadDate();

    $scope.save = function () {
        if (!$scope.current || !$scope.current.Name) {
            return;
        }
        $scope.current.OrganizationID = 0;
        $http.post('/api/Organizations', $scope.current).success(function (data) {
            $scope.current = {};
            $scope.loading = false;
            $scope.loadDate();
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });
    }
}

organizationCreateController.$inject = ['$scope', '$http', '$modal'];