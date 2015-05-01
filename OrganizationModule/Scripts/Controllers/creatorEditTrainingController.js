function creatorEditTrainingController($scope, $http, $modal) {
    $scope.loading = true;
    $scope.currentTraining = {};
    $scope.trainingDetails = [];


    //Used to display the data 
    $scope.loadGroups = function () {
        $http.get('/api/Group')
        .success(function (data) {
            if (!data) {
                return;
            }
            $scope.Groups = data;
            $scope.loading = false;
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });
    }

    $scope.loadTraining = function () {
        $http.get('/api/Training')
        .success(function (data) {
            if (data.length)
            {
                $scope.currentTraining = data[0];
            }
            $scope.loading = false;
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });
    }

    $scope.loadGroups();
    $scope.loadTraining();

    $scope.loadImage = function () {

    }

    $scope.loadIcon = function () {

    }

    $scope.showIcon = function () {

    }

    $scope.save = function () {
        //check conditions
        if (!$scope.currentTraining.Name) {
            return;
        }
        $scope.currentTraining.CreateUserID = 1;
        $scope.currentTraining.Details = $scope.trainingDetails;
        $scope.currentTraining.Questions = $scope.trainingQuestion;
        $http.post('/api/Training', $scope.currentTraining)
        .success(function (data) {
            $scope.loading = false;

            $scope.currentTraining = {};
            $scope.trainingDetails = [];
            $scope.trainingQuestion = [];
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });
    }

}

creatorEditTrainingController.$inject = ['$scope', '$http', '$modal'];