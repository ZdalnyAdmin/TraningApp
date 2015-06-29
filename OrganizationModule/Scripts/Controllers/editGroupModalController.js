var editGroupModalController = function ($scope, $http, $modalInstance, UtilitiesFactory, group) {

    //temp solution
    $scope.Current = group;


    $scope.close = function () {
        $modalInstance.close();
    };

    $scope.save = function () {


        $modalInstance.close($scope.Current);
    };
};

editGroupModalController.$inject = ['$scope', '$http', '$modalInstance', 'UtilitiesFactory', 'group'];