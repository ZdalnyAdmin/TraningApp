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
                if (newVal) {
                    var index = -1;
                    for (var i = 0; i < newVal.length; i++) {
                        if (newVal[i].ResourceType === undefined || newVal[i].ResourceType === -1) {
                            index = i;
                            $scope.index = -1;
                            break;
                        }
                    }
                    //delete element from list
                    if (index >= 0) {
                        $scope.obj.splice(index, 1);
                    }
                }
            }, true);

        }]
    };
}]);