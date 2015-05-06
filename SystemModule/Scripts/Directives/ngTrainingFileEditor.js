window.App
.directive('ngTrainingFileEditor', [function () {
    return {
        scope: { obj: '=' },
        restrict: 'A',
        replace: 'true',
        templateUrl: 'Templates/trainingFileEditor.html',
        controller: ['$scope', function ($scope) {
            $scope.currentDetail = {};

            $scope.cancel = function (item) {
                $scope.currentDetail = {};
            }

            $scope.add = function (item) {
                if (!$scope.currentDetail) {
                    return;
                }

                if ($scope.currentDetail.ExternalResource) {
                    $scope.currentDetail.isEdit = false;
                    $scope.currentDetail.ResourceType = 4;
                    $scope.obj.push($scope.currentDetail);
                    $scope.currentDetail = {};
                    return;
                }
            }
        }]
    };
}]);