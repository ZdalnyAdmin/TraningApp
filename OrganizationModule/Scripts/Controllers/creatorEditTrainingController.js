function creatorEditTrainingController($scope, $http, $modal, $location, UserFactory, UtilitiesFactory) {
    $scope.viewModel = {};

    var searchObj = $location.search();


    //Used to display the data 
    $scope.loadGroups = function () {
        UtilitiesFactory.showSpinner();
        $scope.viewModel.ActionType = 5;
        $http.post('/api/Group/', $scope.viewModel)
        .success(function (data) {
            $scope.viewModel = data;
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.viewModel.ErrorMessage = 'Wystąpił nieoczekiwany błąd podczas inicjalizacji danych';
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.loadTraining = function () {
       
        if (!!searchObj && !!searchObj.trainingID) {
            $scope.viewModel.TrainingID = searchObj.trainingID;
        }

        UtilitiesFactory.showSpinner();
        $scope.viewModel.ActionType = 6;
        $http.post('/api/Training/', $scope.viewModel)
        .success(function (data) {
            $scope.viewModel = data;
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.viewModel.ErrorMessage = 'Wystąpił nieoczekiwany błąd podczas inicjalizacji danych';
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
                $scope.viewModel.Current.PassResources = selectedMark;
            }
        });
    }

    $scope.save = function () {
        //check conditions
        if (!$scope.viewModel.Current.Name) {
            return;
        }
        UtilitiesFactory.showSpinner();
        $scope.viewModel.ActionType = 2;
        angular.forEach($scope.viewModel.Groups, function (val) {
            if (val.selected) {
                $scope.viewModel.Current.Groups.push(val);
            }
        });

        $http.post('/api/Training', $scope.viewModel)
        .success(function (data) {
            $scope.viewModel = data;
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.viewModel.ErrorMessage = 'Wystąpił nieoczekiwany błąd podczas zapisu szkolenia';
            UtilitiesFactory.hideSpinner();
        });
    }

}

creatorEditTrainingController.$inject = ['$scope', '$http', '$modal', '$location', 'UserFactory', 'UtilitiesFactory'];