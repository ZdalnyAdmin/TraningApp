window.App
.directive('ngTrainingTextEditor', [function () {
    return {
        scope: { obj: '=' },
        restrict: 'A',
        replace: 'true',
        templateUrl: 'Templates/trainingTextEditor.html',
        controller: ['$scope', function ($scope) {
            $scope.currentDetail = {};
            $scope.textLength = 0;

            $scope.options = {
                maxCharacters: 2000
            };

            $scope.cancel = function (item) {

                $scope.currentDetail.ResourceType = -1;
                $scope.obj.push($scope.currentDetail);
                $scope.textValidation = false;
                $scope.currentDetail.Text = undefined;
            };

            $scope.blur = function (e, editor) {
                $scope.textValidation = true;
                $scope.textLength = editor.getText().length;
            };

            $scope.changed = function (e, editor) {
                $scope.textLength = editor.getText().length;
            };

            $scope.add = function (item) {
                if (!$scope.currentDetail) {
                    return;
                }

                if ($scope.currentDetail.Text) {
                    $scope.currentDetail.isEdit = false;
                    $scope.currentDetail.ResourceType = 0;
                    $scope.obj.push(angular.copy($scope.currentDetail));
                    $scope.cancel();
                    return;
                }
            };
        }]
    };
}]);