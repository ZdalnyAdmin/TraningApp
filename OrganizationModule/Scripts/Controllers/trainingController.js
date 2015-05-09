function trainingController($scope, $http, $modal, UtilitiesFactory) {
    $scope.answers = {};

    $scope.init = function (questionId, questionType) {
        switch (questionType) {
            case 'Multi':
                $scope.answers[questionId] = {};
                break;
            default:
                $scope.answers[questionId] = '';
                break;
        }
    };
}

trainingController.$inject = ['$scope', '$http', '$modal', 'UtilitiesFactory'];
