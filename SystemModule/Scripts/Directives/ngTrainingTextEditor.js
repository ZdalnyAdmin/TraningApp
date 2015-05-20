window.App
.directive('ngTrainingTextEditor', [function () {
    return {
        scope: { obj: '=' },
        restrict: 'A',
        replace: 'true',
        templateUrl: 'Templates/trainingTextEditor.html',
        controller: ['$scope', function ($scope) {
            $scope.currentDetail = {};

            $scope.cancel = function (item) {
                $scope.currentDetail.ResourceType = -1;
                $scope.obj.push($scope.currentDetail);
                $scope.currentDetail = {};
            }

            $scope.add = function (item) {
                if (!$scope.currentDetail) {
                    return;
                }

                if ($scope.currentDetail.Text) {
                    $scope.currentDetail.isEdit = false;
                    $scope.currentDetail.ResourceType = 0;
                    $scope.obj.push($scope.currentDetail);
                    $scope.currentDetail = {};
                    return;
                }
            }
        }]
    };
}]);