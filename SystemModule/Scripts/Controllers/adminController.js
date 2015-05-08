function adminController($scope, $http, $modal, UserFactory, UtilitiesFactory) {
    $scope.list = [];

    $scope.loadDate = function () {
        UtilitiesFactory.showSpinner();
        $http.get('/api/Admin').success(function (data) {
            $scope.list = data;
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.loadDate();
}

adminController.$inject = ['$scope', '$http', '$modal', 'UserFactory', 'UtilitiesFactory'];