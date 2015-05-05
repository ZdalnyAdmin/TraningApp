function managerTrainingsController($scope, $http, $modal, $location) {
    $scope.loading = true;
    $scope.Trainings = {};

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
            return;
        }

        //call view 
        $location.path('/' + 'creatorTrainings/').search({ trainingID: item.TrainingID });
    }
}

managerTrainingsController.$inject = ['$scope', '$http', '$modal', '$location'];