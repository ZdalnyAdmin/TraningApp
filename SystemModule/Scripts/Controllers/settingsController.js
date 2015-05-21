function settingsController($scope, $http, $modal, UserFactory, UtilitiesFactory) {
    $scope.current = {};

    $scope.loadDate = function () {
        UtilitiesFactory.showSpinner();
        $http.post('/api/Settings', $scope.current).success(function (data) {
            $scope.current = data;
            $scope.success = "";
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.loadDate();

    $scope.save = function (obj) {
        if (!obj) {
            return;
        }
        UtilitiesFactory.showSpinner();
        $http.put('/api/Settings', obj).success(function (data) {
            $scope.success = "Dane zapisane!";
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            UtilitiesFactory.hideSpinner();
        });

    }
}

settingsController.$inject = ['$scope', '$http', '$modal', 'UserFactory', 'UtilitiesFactory'];