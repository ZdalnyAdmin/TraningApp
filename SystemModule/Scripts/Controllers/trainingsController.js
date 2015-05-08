function trainingsController($scope, $http, $modal, UserFactory, UtilitiesFactory) {
    $scope.Trainings = [];

    $scope.loadTrainings = function () {
        UtilitiesFactory.showSpinner();
        $http.get('/api/SimpleTraining').success(function (data) {
            $scope.Trainings = data;
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.loadTrainings();

    $scope.edit = function (item) {
        if (!item) {

        }
        UtilitiesFactory.showSpinner();
        //call view
        UtilitiesFactory.hideSpinner();
    }
}

trainingsController.$inject = ['$scope', '$http', '$modal', 'UserFactory', 'UtilitiesFactory'];