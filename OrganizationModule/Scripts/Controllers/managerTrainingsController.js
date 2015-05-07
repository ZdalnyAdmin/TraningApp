function managerTrainingsController($scope, $http, $modal, $location, UserFactory, UtilitiesFactory) {
    $scope.loading = true;
    $scope.Trainings = {};

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
            return;
        }

        //call view 
        $location.path('/' + 'creatorTrainings/').search({ trainingID: item.TrainingID });
    }
}

managerTrainingsController.$inject = ['$scope', '$http', '$modal', '$location', 'UserFactory', 'UtilitiesFactory'];