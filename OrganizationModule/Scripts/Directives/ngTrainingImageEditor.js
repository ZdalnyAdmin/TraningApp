﻿window.App
.directive('ngTrainingImageEditor', [function () {
    return {
        scope: { obj: '=' },
        restrict: 'A',
        replace: 'true',
        templateUrl: 'Templates/trainingImageEditor.html',
        controller: ['$scope', function ($scope) {
            $scope.currentDetail = {};
            //details methods
            $scope.upload = function (item) {
                //todo
            }

            $scope.cancel = function (item) {
                $scope.currentDetail.ExternalResource = '';
                $scope.currentDetail.InternalResource = '';
                $scope.currentDetail.ResourceType = undefined;
                $scope.currentDetail.isEdit = undefined;
            }

            $scope.add = function (item) {
                if (!$scope.currentDetail) {
                    return;
                }

                if ($scope.currentDetail.ExternalResource || $scope.currentDetail.InternalResource) {
                    $scope.currentDetail.isEdit = false;
                    $scope.currentDetail.ResourceType = $scope.currentDetail.ExternalResource ? 1 : 0;
                    $scope.obj.push(angular.copy($scope.currentDetail));
                    $scope.cancel();
                    return;
                }
            }
        }]
    };
}]);