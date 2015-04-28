function adminManagmentController($scope, $http) {
    $scope.loading = true;
    $scope.addMode = false;
    $scope.selectedGroups = [];

    $scope.loadData = function () {
        $http.get('/api/Training').success(function (data) {
            var internalTrainings = [];
            var kenproTrainings = [];
            angular.forEach(data, function (item) {
                if (item.TrainingTypeID == 1) {
                    internalTrainings.push(item);
                }
                else {
                    kenproTrainings.push(item);
                }
            });
            $scope.InternalTrainings = internalTrainings;
            $scope.KenproTrainings = kenproTrainings;
            $scope.loading = false;
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });
    }

    $scope.loadGroups = function () {
        $http.get('/api/Group').success(function (data) {
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

    $scope.changeStatus = function (training) {
        if (!training) {
            return;
        }

        training.IsActive = !training.IsActive;

        $http.put('/api/Training', training).success(function (data) {
            $scope.loading = false;
            $scope.loadData();
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });
    }

    $scope.edit = function (training) {
        if (!training) {
            return;
        }

        if (!$scope.Groups) {
            return;
        }

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
            $scope.loading = false;
            $scope.loadData();
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });
    }

    $scope.loadData();
    $scope.loadGroups();
}