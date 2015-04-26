function adminStatisticsController($scope, $http, $modal) {
    $scope.loading = true;


    $scope.loadData = function () {
        //Used to display the data 
        $http.get('/api/Statistics').success(function (data) {
            $scope.Statistic = data;
            $scope.loading = false;
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });
    }

    $scope.loadTrainings = function () {
        //Used to display the data 
        $http.get('/api/Training').success(function (data) {
            $scope.Trainings = data;
            $scope.loading = false;
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });
    }

    $scope.loadData();
    $scope.loadTrainings();
}

