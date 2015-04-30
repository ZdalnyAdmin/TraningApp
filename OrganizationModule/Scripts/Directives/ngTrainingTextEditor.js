
window.App
.directive('ngTrainingTextEditor', [function () {
    return {
        scope: true,  // use a child scope that inherits from parent
        restrict: 'A',
        replace: 'true',
        templateUrl: 'Templates/trainingTextEditor.html',
        controller: ['$scope', function ($scope) {
            $scope.currentDetail = {
                Text: "It Works"
            };
        }]
    };
}]);