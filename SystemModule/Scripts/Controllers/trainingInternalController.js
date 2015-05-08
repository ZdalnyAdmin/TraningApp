function trainingInternalController($scope, $http, $modal, UserFactory, UtilitiesFactory) {
    $scope.Trainings = data;

    $scope.loadTrainings = function () {
        UtilitiesFactory.showSpinner();
        $http.get('/api/InternalTrainings').success(function (data) {
            $scope.Trainings = data;
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.loadTrainings();

    $scope.showMore = function (item) {
        UtilitiesFactory.showSpinner();
        UtilitiesFactory.hideSpinner();
    }
}

trainingInternalController.$inject = ['$scope', '$http', '$modal', 'UserFactory', 'UtilitiesFactory'];