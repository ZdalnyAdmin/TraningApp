function adminSettingsController($scope, $http, $modal) {
    $scope.loading = true;
    $scope.Setting = {};
    $scope.Setting.AllowUserToChangeName = true;
    $scope.Setting.AllowUserToChangeMail = true;

    $scope.loadData = function () {
        //Used to display the data 
        $http.get('/api/Settings').success(function (data) {
            $scope.Setting = data;
            if (data !== 'null') {
                $scope.Setting = data;
            }
            $scope.loading = false;
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });
    }

    $scope.allowToChangeName = function () {
        $scope.save($scope.Setting);
    }

    $scope.allowToChangeMail = function () {
        $scope.save($scope.Setting);
    }

    $scope.save = function (setting) {
        if (!$scope.Setting) {
            return;
        }

        $http.put('/api/Settings', setting).success(function (data) {
            $scope.loading = false;
            $scope.loadData();
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });
    }

    $scope.loadData();
}