function managerResultController($scope, $http, $modal) {
    $scope.loading = true;

    //Used to display the data 


    $scope.loadTrainings = function () {
        $http.get('/api/Training').success(function (data) {
            $scope.Trainings = data;
            $scope.loading = false;
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });
    }

    $scope.loadPeople = function () {
        $http.get('/api/SimplePerson').success(function (data) {
            $scope.Users = data;
            $scope.loading = false;
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });
    }

    $scope.loadResults = function () {
        $http.get('/api/TraningsResult').success(function (data) {
            $scope.Results = data;
            $scope.DbResult = data;
            $scope.loading = false;
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });
    }

    $scope.loadResults();
    $scope.loadTrainings();
    $scope.loadPeople();


    $scope.changeTraning = function(obj)
    {
        $scope.Results = [];

        if (!obj)
        {
            $scope.Results = $scope.DbResult
            return;
        }

        angular.forEach($scope.DbResult, function (item) {
            if (item.TrainingID == obj.TrainingID) {
                $scope.Results.push(item);
            }
        });
    }

    $scope.changePerson = function (obj)
    {
        $scope.Results = [];

        if (!obj) {
            $scope.Results = $scope.DbResult
            return;
        }

        angular.forEach($scope.DbResult, function (item) {
            if (item.PersonID == obj.PersonID) {
                $scope.Results.push(item);
            }
        });
    }
}

managerResultController.$inject = ['$scope', '$http', '$modal'];
