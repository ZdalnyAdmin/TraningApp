var confirmModalController = function ($scope, $modalInstance, modalResult) {
    $scope.modalResult = modalResult;

    $scope.confirm = function () {

        $modalInstance.close(true);
    };

    $scope.close = function () {

        $modalInstance.close(false);
    };

    $scope.cancel = function () {

        $modalInstance.dismiss('cancel');
    };
};

confirmModalController.$inject = ['$scope', '$modalInstance', 'modalResult'];