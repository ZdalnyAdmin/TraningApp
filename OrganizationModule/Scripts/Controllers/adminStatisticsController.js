function adminStatisticsController($scope, $http, $modal, UserFactory, UtilitiesFactory) {

    $scope.loadData = function () {
        UtilitiesFactory.showSpinner();
        //Used to display the data 
        $http.get('/api/Statistics').success(function (data) {
            $scope.Statistic = data;
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.loadTrainings = function () {
        UtilitiesFactory.showSpinner();
        //Used to display the data 
        $http.get('/api/Training').success(function (data) {
            $scope.Trainings = data;
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.loadData();
    $scope.loadTrainings();
}

adminStatisticsController.$inject = ['$scope', '$http', '$modal', 'UserFactory', 'UtilitiesFactory'];

