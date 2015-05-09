function adminManagmentController($scope, $http, $modal, UserFactory, UtilitiesFactory) {
    $scope.addMode = false;
    $scope.selectedGroups = [];
    $scope.logs = [];
    $scope.index = 0;

    $scope.loadData = function () {

        UtilitiesFactory.showSpinner();
        var training = {};
        training.TrainingType = 1;

        $http.post('/api/ManageTraining', training).success(function (data) {
            var internalTrainings = [];
            var kenproTrainings = [];
            angular.forEach(data, function (item) {
                if (item.TrainingTypeID == 1) {
                    item.showLog = false;
                    internalTrainings.push(item);
                }
                else {
                    kenproTrainings.push(item);
                }
            });
            $scope.InternalTrainings = internalTrainings;
            $scope.KenproTrainings = kenproTrainings;
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.error = "Wystapil problem z pobraniem danych!";
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.loadGroups = function () {

        UtilitiesFactory.showSpinner();

        $http.get('/api/Group').success(function (data) {
            if (!data) {
                return;
            }
            $scope.Groups = data;
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.error = "Wystapil problem z pobraniem danych!";
            UtilitiesFactory.hideSpinner();
        });
    }

    loadLogs = function (training) {
        var log = {};
        log.OperationType = 6;
        log.TrainingID = training.TrainingID;


        UtilitiesFactory.showSpinner();

        $http.post('/api/Logs'. log).success(function (data) {
            if (!data) {
                return;
            }
            $scope.logs = data;
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.error = "Wystapil problem z pobraniem danych!";
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.changeStatus = function (training) {
        if (!training) {
            return;
        }

        training.IsActive = !training.IsActive;

        UtilitiesFactory.showSpinner();

        $http.put('/api/Training', training).success(function (data) {
            UtilitiesFactory.hideSpinner();
            $scope.loadData();
        })
        .error(function () {
            $scope.error = "Wystapil problem z pobraniem danych!";
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

        var trainigInGroups = [];
        angular.forEach($scope.Groups, function (item) {
            var obj = {};
            obj.ProfileGroupID = item.ProfileGroupID;
            obj.TrainingID = training.TrainingID
            obj.IsDeleted = false;
            trainigInGroups.push(obj);

            if (training.AssignedGroups != '') {
                training.AssignedGroups += ', ';
            }
            training.AssignedGroups += item.Name;
        });

        $http.post('/api/TrainingsInGroup', trainigInGroups).success(function (data) {
            UtilitiesFactory.hideSpinner();
            $scope.loadData();
        })
        .error(function () {
            $scope.error = "Wystapil problem z zapisem danych!";
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.showLogs = function (training) {
        training.showLog = true;
        loadLogs(training);
    }

    $scope.hideLogs = function (training) {
        training.showLog = false;
    }

    $scope.showTab = function (index) {
        $scope.index = index;
    }
    
    $scope.loadData();
    $scope.loadGroups();
}

adminManagmentController.$inject = ['$scope', '$http', '$modal', 'UserFactory', 'UtilitiesFactory'];