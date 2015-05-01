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

        }]
    };
}]);