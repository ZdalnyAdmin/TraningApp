function adminManagmentController($scope, $http, $modal, UtilitiesFactory) {
    $scope.addMode = false;
    $scope.viewModel = {};
    $scope.index = 1;

    $scope.loadData = function () {

        UtilitiesFactory.showSpinner();
        $scope.viewModel.ActionType = 0;

        $http.post('/api/TrainingManagment/', $scope.viewModel).success(function (data) {
            $scope.viewModel = data;
            $scope.index = 1;
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.viewModel.ErrorMessage = "Wystapil problem z pobraniem danych!";
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.loadData();

    $scope.changeStatus = function (training) {
        if (!training) {
            return;
        }

        training.IsActive = !training.IsActive;

        UtilitiesFactory.showSpinner();

        $scope.viewModel.Current = training;

        $http.put('/api/TrainingManagment/', $scope.viewModel).success(function (data) {
            $scope.loadData();
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.viewModel.ErrorMessage = "Wystapil problem z pobraniem danych!";
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.edit = function (training) {
        if (!training) {
            return;
        }

        if (!$scope.Groups) {
            return;
        }

        UtilitiesFactory.showSpinner();

        $scope.viewModel.Current = training;

        $scope.viewModel.Current.Groups = [];

        angular.forEach($scope.viewModel.Groups, function (val) {
            if (val.selected) {
                $scope.viewModel.Current.Groups.push(val);
            }
        });

        $scope.viewModel.ActionType = 3;

        $http.post('/api/TrainingManagment/', $scope.viewModel).success(function (data) {
            $scope.viewModel = data;
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.viewModel.ErrorMessage = "Wystapil problem z zapisem danych!";
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.showLogs = function (training) {
        training.showLog = true;
    }

    $scope.hideLogs = function (training) {
        training.showLog = false;
    }

    $scope.showTab = function (index) {
        $scope.index = index;

        if ($scope.index == 2) {
            UtilitiesFactory.showSpinner();
            $scope.viewModel.ActionType = 1;

            $http.post('/api/TrainingManagment', $scope.viewModel).success(function (data) {
                $scope.viewModel = data;
                UtilitiesFactory.hideSpinner();
            })
            .error(function () {
                $scope.viewModel.ErrorMessage = "Wystapil problem z pobraniem danych!";
                UtilitiesFactory.hideSpinner();
            });

        }
    }
    
}

adminManagmentController.$inject = ['$scope', '$http', '$modal', 'UtilitiesFactory'];