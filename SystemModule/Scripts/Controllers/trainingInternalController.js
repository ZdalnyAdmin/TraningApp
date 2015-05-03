function trainingInternalController($scope, $http, $modal) {
    $scope.loading = true;
    $scope.Trainings = data;

    $scope.loadTrainings = function () {
        $http.get('/api/InternalTrainings').success(function (data) {
            $scope.Trainings = data;
            $scope.loading = false;
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });
    }

    $scope.loadTrainings();

    $scope.showMore = function (item) {

    }
}

trainingInternalController.$inject = ['$scope', '$http', '$modal'];