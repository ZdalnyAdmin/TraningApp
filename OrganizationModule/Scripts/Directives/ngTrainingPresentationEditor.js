window.App
.directive('ngTrainingPresentationEditor', [function () {
    return {
        scope: { obj: '=' },
        restrict: 'A',
        replace: 'true',
        templateUrl: 'Templates/trainingPresentationEditor.html',
        controller: ['$scope', function ($scope) {
            $scope.currentDetail = {};
            //details methods
            $scope.upload = function (item) {
                //todo
            }

            $scope.cancel = function (item) {

                $scope.currentDetail.ResourceType = -1;
                $scope.obj.push($scope.currentDetail);

                $scope.currentDetail.InternalResource = '';
                $scope.currentDetail.ResourceType = undefined;
                $scope.currentDetail.isEdit = undefined;
            }

            $scope.add = function (item) {
                if (!$scope.currentDetail) {
                    return;
                }


                if ($scope.currentDetail.InternalResource) {
                    $scope.currentDetail.isEdit = false;
                    $scope.currentDetail.ResourceType = 3;
                    $scope.obj.push(angular.copy($scope.currentDetail));
                    $scope.cancel();
                    return;
                }
            }
        }]
    };
}]);