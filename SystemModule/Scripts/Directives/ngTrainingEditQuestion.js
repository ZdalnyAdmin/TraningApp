window.App
.directive('ngTrainingEditQuestion', [function () {
    return {
        scope: { questions: '='},
        restrict: 'A',
        replace: 'true',
        templateUrl: 'Templates/trainingEditQuestion.html',
        controller: ['$scope', function ($scope) {
            $scope.editableQuestion = {};

            $scope.up = function (question) {
                $scope.questions = changePosition($scope.questions, question, false);
            }

            $scope.down = function (question) {
                $scope.questions = changePosition($scope.questions, question, true);
            }

            $scope.edit = function (question) {
                $scope.editableQuestion = angular.copy(question);
                question.isEdit = true;
            }

            $scope.delete = function (question) {
                for (var i = 0; i < $scope.questions.length; i++) {
                    if ($scope.questions[i] == question) {
                        index = i;
                        break;
                    }
                }
                $scope.questions.splice(index, 1);
            }

            $scope.save = function (question) {
                $scope.editableQuestion = {};
                question.isEdit = false;
            }

            $scope.cancel = function (question) {
                question.Text = $scope.editableQuestion.Text;
                question.Answers = $scope.editableQuestion.Answers;
                $scope.editableQuestion = {};
                question.isEdit = false;
            }

            changePosition = function (list, currentItem, up) {
                if (!list || !list.length) {
                    return;
                }
                var index = 0;
                for (var i = 0; i < list.length; i++) {
                    if (list[i] == currentItem) {
                        index = i;
                        break;
                    }
                }
                //delete element from list
                list.splice(index, 1);
                if (up) {
                    index++;
                }
                else {
                    index--;
                }

                var temp = [];
                //add on first position
                if (index <= 0) {
                    temp.push(currentItem);
                    angular.forEach(list, function (item) {
                        temp.push(item);
                    });
                    return temp;
                }
                //add for last position
                if (index >= list.length) {

                    angular.forEach(list, function (item) {
                        temp.push(item);
                    });
                    temp.push(currentItem);
                    return temp;
                }
                //change position
                for (var i = 0; i < list.length; i++) {
                    if (i == index) {
                        temp.push(currentItem);
                    }
                    temp.push(list[i]);
                }
                return temp;
            }

        }]
    };
}]);