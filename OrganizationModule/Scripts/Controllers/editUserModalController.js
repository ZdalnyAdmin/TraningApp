var editUserModalController = function ($scope, $http, $modalInstance, UtilitiesFactory, user) {

    //temp solution
    $scope.Current = user;


    $scope.close = function () {
        $modalInstance.close();
    };

    $scope.save = function () {

        $modalInstance.close($scope.Current);
    };
};

editUserModalController.$inject = ['$scope', '$http', '$modalInstance', 'UtilitiesFactory', 'user'];