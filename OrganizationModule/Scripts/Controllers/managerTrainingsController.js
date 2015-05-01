function managerTrainingsController($scope, $http, $modal) {
    $scope.loading = true;
    $scope.Trainings = data;

    $scope.loadTrainings = function () {
        $http.get('/api/SimpleTraining').success(function (data) {
            $scope.Trainings = data;
            $scope.loading = false;
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });
    }

    $scope.loadTrainings();

    $scope.edit = function (item) {
        if (!item) {

        }

        //call view
    }
}

managerTrainingsController.$inject = ['$scope', '$http', '$modal'];