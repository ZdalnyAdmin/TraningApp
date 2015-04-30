function adminSettingsController($scope, $http, $modal) {
    $scope.loading = true;
    $scope.currentItem = {};
    //Used to display the data 
    $scope.loadData = function () {

        $scope.currentItem = {};
        $scope.currentItem.ProtectorID = 1;
        $http.post('/api/Settings', $scope.currentItem).success(function (data) {
            $scope.currentItem = data;
            $scope.currentItem.ChangeMail = $scope.currentItem.AllowUserToChangeMail ? "1" : "0";
            $scope.currentItem.ChangeName = $scope.currentItem.AllowUserToChangeName ? "1" : "0";
            $scope.loading = false; 
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });

        //Used to display the data 

    }

    $scope.loadData();

    $scope.edit = function (obj) {
        if (!obj) {
            return;
        }
        
        $scope.currentItem.AllowUserToChangeMail = $scope.currentItem.ChangeMail == "1";
        $scope.currentItem.AllowUserToChangeName = $scope.currentItem.ChangeName == "1";
       
        $http.put('/api/Settings', obj).success(function (data) {
            $scope.loading = false;
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });

    }
}

adminSettingsController.$inject = ['$scope', '$http', '$modal'];