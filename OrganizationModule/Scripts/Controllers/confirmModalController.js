var confirmModalController = function ($scope, $modalInstance) {

    $scope.confirm = function () {

        $modalInstance.close(true);
    };

    $scope.close = function () {

        $modalInstance.close(false);
    };
};

confirmModalController.$inject = ['$scope', '$modalInstance'];