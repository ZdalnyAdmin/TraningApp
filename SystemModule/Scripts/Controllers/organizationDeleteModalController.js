var organizationDeleteModalController = function ($scope, $http, $modalInstance) {
    $scope.selectedReason = {};
    $scope.selectedReason.Text = '';

    $scope.close = function () {
        $modalInstance.close();
    };

    $scope.save = function () {
        if ($scope.selectedReason.Text) {
            $modalInstance.close($scope.selectedReason);
        }
        $modalInstance.close();
    };
};

organizationDeleteModalController.$inject = ['$scope', '$http', '$modalInstance'];