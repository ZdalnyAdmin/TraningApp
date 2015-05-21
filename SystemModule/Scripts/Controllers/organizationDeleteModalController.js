var organizationDeleteModalController = function ($scope, $http, $modalInstance) {
    $scope.selectedReason = {};
    $scope.selectedReason.Text = '';

    $scope.close = function () {
        $modalInstance.close();
    };

    $scope.save = function () {
        $modalInstance.close($scope.selectedReason);
    };
};

organizationDeleteModalController.$inject = ['$scope', '$http', '$modalInstance'];