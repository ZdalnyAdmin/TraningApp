var confirmModalController = function ($scope, $modalInstance, modalResult) {
    $scope.modalResult = modalResult;

    $scope.confirm = function () {

        $modalInstance.close(true);
    };

    $scope.close = function () {

        $modalInstance.close(false);
    };
};

confirmModalController.$inject = ['$scope', '$modalInstance', 'modalResult'];