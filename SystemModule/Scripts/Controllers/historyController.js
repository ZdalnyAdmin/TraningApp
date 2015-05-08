function historyController($scope, $http, $modal, UserFactory, UtilitiesFactory) {
    //Used to display the data 
    $scope.loadType = function () {
        UtilitiesFactory.showSpinner();
        $scope.currentItem = {};
        $scope.currentItem.ProtectorID = 1;
        $http.get('/api/SystemLog').success(function (data) {
            $scope.types = data;
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.loadLogs = function () {
        UtilitiesFactory.showSpinner();
        $http.get('/api/Logs').success(function (data) {
            $scope.Logs = data;
            $scope.DbLogs = data;
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.loadLogs();
    $scope.loadType();


    $scope.change = function (obj) {
        $scope.Logs = [];
        UtilitiesFactory.showSpinner();
        if (!obj) {
            $scope.Logs = $scope.DbLogs
            return;
        }

        angular.forEach($scope.DbLogs, function (item) {
            if (item.SystemType == obj.Type) {
                $scope.Logs.push(item);
            }
        });
        UtilitiesFactory.hideSpinner();
    }
}

historyController.$inject = ['$scope', '$http', '$modal', 'UserFactory', 'UtilitiesFactory'];