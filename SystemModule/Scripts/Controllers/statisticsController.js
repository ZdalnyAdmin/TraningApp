function statisticsController($scope, $rootScope, $http, $modal, UserFactory, UtilitiesFactory) {
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
            $scope.success = "";

            if ($scope.viewModel && $scope.viewModel.Success) {
                $rootScope.$broadcast('showGlobalMessage', {
                    success: true,
                    messageText: $scope.viewModel.Success
                });
            }
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

}

statisticsController.$inject = ['$scope', '$rootScope', '$http', '$modal', 'UserFactory', 'UtilitiesFactory'];