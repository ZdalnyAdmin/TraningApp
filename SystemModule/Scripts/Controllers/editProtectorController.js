function editProtectorController($scope, $http, $modal) {
    $scope.loading = true;
    $scope.list = [];
    $scope.editable = {};


    $scope.loadData = function () {
        $http.get('/api/Protector')
        .success(function (data) {
            angular.forEach(data, function (val) {
                val.showDetails = true;
            });

            $scope.list = data;
            $scope.loading = false;
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });
    }

    $scope.loadData();

    $scope.showDetails = function (item, showDetails) {
        item.showDetails = showDetails;
    }


    $scope.save = function (item) {

        $http.put('/api/Protector', item)
        .success(function (data) {
            $scope.loading = false;
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });
    }

    $scope.delete = function (item) {
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
            $scope.loading = false;
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });
    }
}

editProtectorController.$inject = ['$scope', '$http', '$modal'];