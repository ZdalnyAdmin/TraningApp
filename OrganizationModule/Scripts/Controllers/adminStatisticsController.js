function adminStatisticsController($scope, $rootScope, $http, $modal, UserFactory, UtilitiesFactory) {

    $scope.loadData = function () {
        UtilitiesFactory.showSpinner();
        //Used to display the data 
        $http.get('/api/Statistics').success(function (data) {
            $scope.Statistic = data;
            $scope.success = "";
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $rootScope.$broadcast('showGlobalMessage', {
                success: false,
                messageText: "An Error has occured while loading posts!"
            });
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.loadTrainings = function () {
        UtilitiesFactory.showSpinner();
        //Used to display the data 
        $http.get('/api/Training').success(function (data) {
            $scope.Trainings = data;
            $scope.success = "";
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $rootScope.$broadcast('showGlobalMessage', {
                success: false,
                messageText: "An Error has occured while loading posts!"
            });
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.loadSetting = function () {
        UtilitiesFactory.showSpinner();
        var loggedUser = {};
        //Used to display the data 
        $http.get('/api/Settings').success(function (data) {
            $scope.Trainings = data;
            $scope.success = "";
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $rootScope.$broadcast('showGlobalMessage', {
                success: false,
                messageText: "An Error has occured while loading posts!"
            });
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.loadData();
    $scope.loadTrainings();
}

adminStatisticsController.$inject = ['$scope', '$rootScope', '$http', '$modal', 'UserFactory', 'UtilitiesFactory'];

