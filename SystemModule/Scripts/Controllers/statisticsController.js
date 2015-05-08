function statisticsController($scope, $http, $modal, UserFactory, UtilitiesFactory) {
    $scope.list = [];
    $scope.current = {};
    $scope.index = 0;

    $scope.loadDate = function (type) {
        UtilitiesFactory.showSpinner();
        var stats = {};
        stats.Type = type;
        $http.post('/api/Statistics', stats).success(function (data) {
            if (!data) {
                return;
            }
            if (type == 3) {
                if (!!data.length && data.length >= 0) {
                    $scope.current = data[0];
                }

                $scope.current
            } else {
                $scope.list = data;
            }
            $scope.index = type;
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            UtilitiesFactory.hideSpinner();
        });
    }

}

statisticsController.$inject = ['$scope', '$http', '$modal', 'UserFactory', 'UtilitiesFactory'];