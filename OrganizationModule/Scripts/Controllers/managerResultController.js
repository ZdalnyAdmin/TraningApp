function managerResultController($scope, $http, $modal, UserFactory, UtilitiesFactory) {

    $scope.loadTrainings = function () {
        UtilitiesFactory.showSpinner();
        $http.get('/api/Training').success(function (data) {
            $scope.Trainings = data;
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.loadPeople = function () {
        UtilitiesFactory.showSpinner();
        $http.get('/api/SimplePerson').success(function (data) {
            $scope.Users = data;
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.loadResults = function () {
        UtilitiesFactory.showSpinner();
        $http.get('/api/TraningsResult').success(function (data) {
            $scope.Results = data;
            $scope.DbResult = data;
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.loadResults();
    $scope.loadTrainings();
    $scope.loadPeople();


    $scope.changeTraning = function(obj)
    {
        UtilitiesFactory.showSpinner();
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
        UtilitiesFactory.hideSpinner();
    }

    $scope.changePerson = function (obj)
    {
        UtilitiesFactory.showSpinner();
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
        UtilitiesFactory.hideSpinner();
    }
}

managerResultController.$inject = ['$scope', '$http', '$modal', 'UserFactory', 'UtilitiesFactory'];
