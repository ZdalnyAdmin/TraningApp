function editProtectorController($scope, $http, $modal) {
    $scope.loading = true;
    $scope.list = [];
    $scope.editable = {};


    $scope.loadData = function () {
        $http.get('/api/Protecter')
        .success(function (data) {
            if (!data) {
                return;
            }
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

    $scope.edit = function (item) {
        item.isEdit = true;
    }

    $scope.save = function (item) {
      
        $http.put('/api/Protecter')
        .success(function (data) {
            $scope.loading = false;
            item.isEdit = false;
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });
    }

    $scope.delete = function (item) {
        item.isEdit = false;


        item.IsDeleted = true;
        item.DeletedDate = new Date();
        //get from score - logged user id
        item.DeleteUserID = 1;

        $http.put('/api/Protecter')
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