function editProtectorController($scope, $http, $modal, UserFactory, UtilitiesFactory) {
    $scope.list = [];
    $scope.editable = {};


    $scope.loadData = function () {
        UtilitiesFactory.showSpinner();
        $http.get('/api/Protector')
        .success(function (data) {
            angular.forEach(data, function (val) {
                val.showDetails = true;
            });

            $scope.list = data;
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.loadData();

    $scope.showDetails = function (item, showDetails) {
        item.showDetails = showDetails;
    }


    $scope.save = function (item) {
        UtilitiesFactory.showSpinner();
        $http.put('/api/Protector', item)
        .success(function (data) {
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.delete = function (item) {
        UtilitiesFactory.showSpinner();
        item.IsDeleted = true;
        $http.put('/api/Protector', item)
        .success(function (data) {
            var index = 0;

            for (var i = 0; i < $scope.list.length; i++) {
                if ($scope.list[i].IsDeleted) {
                    index = i;
                    break;
                }
            }
            $scope.list.splice(index, 1);
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            UtilitiesFactory.hideSpinner();
        });
    }
}

editProtectorController.$inject = ['$scope', '$http', '$modal', 'UserFactory', 'UtilitiesFactory'];