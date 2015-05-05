function creatorEditTrainingController($scope, $http, $modal, $location) {
    $scope.loading = true;
    $scope.currentTraining = {};

    var searchObj = $location.search();


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
        var obj = {};
        if (!!searchObj && !!searchObj.trainingID) {
            obj.TrainingID = searchObj.trainingID;
        }
        else {
            //hak
            obj.TrainingID = -1;
        }
        $http.post('/api/Training', obj)
        .success(function (data) {
            $scope.currentTraining = data;
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
        var modalInstance = $modal.open({
            templateUrl: '/Templates/Modals/commonMarksModal.html',
            controller: 'commonMarksModalController',
            size: 'sm',
            resolve: {
                selectedMark: function () {
                    return $scope.selectedMark;
                }
            }
        });

        modalInstance.result.then(function (selectedMark) {
            if (!!selectedMark) {
                $scope.currentTraining.PassResources = selectedMark;
            }
        });
    }

    $scope.save = function () {
        //check conditions
        if (!$scope.currentTraining.Name) {
            return;
        }
        $scope.currentTraining.CreateUserID = 1;
        $scope.currentTraining.Details = $scope.trainingDetails;
        $scope.currentTraining.Questions = $scope.trainingQuestion;


        $scope.currentTraining.Groups = [];
        angular.forEach($scope.Groups, function (val) {
            if (val.selected) {
                $scope.currentTraining.Groups.push(val);
            }
        });

        $http.put('/api/Training', $scope.currentTraining)
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

creatorEditTrainingController.$inject = ['$scope', '$http', '$modal', '$location'];