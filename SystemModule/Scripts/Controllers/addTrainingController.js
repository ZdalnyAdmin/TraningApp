function addTrainingController($scope, $http, $modal, UserFactory, UtilitiesFactory) {
    $scope.currentTraining = {};
    //training question elements
    $scope.trainingQuestion = [];
    //training details elements
    $scope.trainingDetails = [];

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

    $scope.loadGroups();

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
        $http.post('/api/Training', $scope.currentTraining)
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

addTrainingController.$inject = ['$scope', '$http', '$modal', 'UserFactory', 'UtilitiesFactory'];