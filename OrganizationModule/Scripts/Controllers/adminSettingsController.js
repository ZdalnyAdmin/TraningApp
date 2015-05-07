function adminSettingsController($scope, $http, $modal, UserFactory, UtilitiesFactory) {
    $scope.loading = true;
    $scope.currentItem = {};
    //Used to display the data 
    $scope.loadData = function () {
        UtilitiesFactory.showSpinner();
        $scope.currentItem = {};
        $scope.currentItem.ProtectorID = -1;
        $http.post('/api/Settings', $scope.currentItem).success(function (data) {
            $scope.currentItem = data;
            $scope.currentItem.ChangeMail = $scope.currentItem.AllowUserToChangeMail ? "1" : "0";
            $scope.currentItem.ChangeName = $scope.currentItem.AllowUserToChangeName ? "1" : "0";
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            UtilitiesFactory.hideSpinner();
        });

        //Used to display the data 

    }

    $scope.loadData();

    $scope.edit = function (obj) {
        if (!obj) {
            return;
        }
        UtilitiesFactory.showSpinner();
        $scope.currentItem.AllowUserToChangeMail = $scope.currentItem.ChangeMail == "1";
        $scope.currentItem.AllowUserToChangeName = $scope.currentItem.ChangeName == "1";
       
        $http.put('/api/Settings', obj).success(function (data) {
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            UtilitiesFactory.hideSpinner();
        });

    }
}

adminSettingsController.$inject = ['$scope', '$http', '$modal', 'UserFactory', 'UtilitiesFactory'];