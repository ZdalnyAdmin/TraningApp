var commonMarksModalController = function ($scope, $rootScope, $http, $modalInstance) {
    $scope.selectedImage = '';


    $scope.loadMarks = function () {
        var obj = {};
        obj.ImageType = 0;
        obj.LoadData = true;

        //display data in modal and save selected
        $http.post('/api/LocalFile', obj)
        .success(function (data) {
            if (!data) {
                return;
            }
            $scope.marks = data;
            $scope.loading = false;
        })
        .error(function () {
            $rootScope.$broadcast('showGlobalMessage', {
                success: false,
                messageText: "An Error has occured while loading posts!"
            });
            $scope.loading = false;
        });
    }

    $scope.loadMarks();

    $scope.close = function () {
        $modalInstance.close();
    };

    $scope.select = function (item) {
        $scope.selectedImage = item;
    };

    $scope.save = function () {
        if ($scope.selectedImage !== '')
        {
            $modalInstance.close($scope.selectedImage);
        }

        $modalInstance.close();
    };
};

commonMarksModalController.$inject = ['$scope', '$rootScope', '$http', '$modalInstance'];