function managerResultController($scope, $http, $modal, UserFactory, UtilitiesFactory) {

    $scope.loadTrainings = function () {
        UtilitiesFactory.showSpinner();

        viewModel = {};
        viewModel.ActionType = 4;
        $http.post('/api/Training/', viewModel).success(function (data) {
            $scope.Trainings = data.Trainings;
            $scope.success = "Dane wczytane!";
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.error = 'Wystąpił nieoczekiwany błąd podczas pobierania szkolen';
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.loadPeople = function () {
        UtilitiesFactory.showSpinner();

        viewModel = {};
        viewModel.ActionType = 0;
        $http.post('/api/Person/', viewModel).success(function (data) {
            $scope.success = "Dane wczytane!";
            $scope.viewModel = data.People;
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.error = 'Wystąpił nieoczekiwany błąd podczas pobierania uzytkownikow';
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.loadResults = function () {
        UtilitiesFactory.showSpinner();
        $http.get('/api/TraningsResult/').success(function (data) {
            $scope.success = "Dane wczytane!";
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
            $scope.Results = $scope.DbResult;
            UtilitiesFactory.hideSpinner();
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
            $scope.Results = $scope.DbResult;
            UtilitiesFactory.hideSpinner();
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
