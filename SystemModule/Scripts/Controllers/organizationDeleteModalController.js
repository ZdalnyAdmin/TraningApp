var organizationDeleteModalController = function ($scope, $http, $modalInstance) {
    $scope.selectedReason = '';

    $scope.close = function () {
        $modalInstance.close();
    };

    $scope.save = function () {
        if ($scope.selectedReason !== '') {
            $modalInstance.close($scope.selectedReason);
        }
        $modalInstance.close();
    };
};

organizationDeleteModalController.$inject = ['$scope', '$http', '$modalInstance'];