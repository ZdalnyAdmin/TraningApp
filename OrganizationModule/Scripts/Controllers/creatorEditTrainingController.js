function creatorEditTrainingController($scope, $http, $modal, $location, UserFactory, UtilitiesFactory) {
    $scope.loading = true;
    $scope.currentTraining = {};

    var searchObj = $location.search();


    //Used to display the data 
    $scope.loadGroups = function () {
        UtilitiesFactory.showSpinner();
        $http.get('/api/Group')
        .success(function (data) {
            if (!data) {
                return;
            }
            $scope.Groups = data;
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.loadTraining = function () {
        var obj = {};
        UtilitiesFactory.showSpinner();
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
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            UtilitiesFactory.hideSpinner();
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
        UtilitiesFactory.showSpinner();
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

            $scope.currentTraining = {};
            $scope.trainingDetails = [];
            $scope.trainingQuestion = [];
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            UtilitiesFactory.hideSpinner();
        });
    }

}

creatorEditTrainingController.$inject = ['$scope', '$http', '$modal', '$location', 'UserFactory', 'UtilitiesFactory'];