window.App
.directive('ngTrainingDetailsTab', [function () {
    return {
        scope: { obj: '=' },
        restrict: 'A',
        replace: 'true',
        templateUrl: 'Templates/trainingDetailsTab.html',
        controller: ['$scope', function ($scope) {
            $scope.index = 0;

            $scope.showTab = function (index) {
                $scope.index = index;
            }

            $scope.$watch('obj', function (newVal, oldVal) {
                var index = 0;
                if (newVal) {
                    var index = 0;
                    for (var i = 0; i < newVal.length; i++) {
                        if (!newVal[i].ResourceType || newVal[i].ResourceType == -1) {
                            index = i;
                            $scope.index = -1;
                            break;
                        }
                    }
                    //delete element from list
                    $scope.obj.splice(index, 1);
                }
            }, true);

        }]
    };
}]);