function settingsController($scope, $rootScope, $http, $modal, UserFactory, UtilitiesFactory) {
    $scope.current = {};

    $scope.loadDate = function () {
        UtilitiesFactory.showSpinner();
        $http.post('/api/Settings', $scope.current).success(function (data) {
            $scope.current = data;
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

    $scope.loadDate();

    $scope.save = function (obj) {
        if (!obj) {
            return;
        }
        UtilitiesFactory.showSpinner();
        $http.put('/api/Settings', obj).success(function (data) {
           
            $rootScope.$broadcast('showGlobalMessage', {
                success: true,
                messageText: "Dane zapisane!"
            });

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

settingsController.$inject = ['$scope', '$rootScope', '$http', '$modal', 'UserFactory', 'UtilitiesFactory'];