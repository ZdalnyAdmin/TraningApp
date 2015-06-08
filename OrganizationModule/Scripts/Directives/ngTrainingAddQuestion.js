window.App
.directive('ngTrainingAddQuestion', [function () {
    return {
        scope: { questions: '=' },
        restrict: 'A',
        replace: 'true',
        templateUrl: 'Templates/trainingAddQuestion.html',
        controller: ['$scope', function ($scope) {
            $scope.questionType = ['jednokrotnego wyboru', 'wielokrotnego wyboru', 'wpisanie odpowiedzi'];
            $scope.selectedQuestion = 0;
            $scope.currentQuestion = {};
            $scope.showQuestionType = false;

            //question methods
            $scope.changeQuestion = function (type) {
                $scope.showQuestionType = false;
                $scope.currentQuestion = {};
                if (type == 'jednokrotnego wyboru') {
                    $scope.selectedQuestion = 1;
                    $scope.currentQuestion.Type = 0;
                    $scope.currentQuestion.Answers = [];
                    for (var i = 0; i < 6; i++) {
                        $scope.currentQuestion.Answers.push(createAnswer());
                    }
                    return;
                }
                if (type == 'wielokrotnego wyboru') {
                    $scope.selectedQuestion = 2;
                    $scope.currentQuestion.Type = 1;
                    $scope.currentQuestion.Answers = [];
                    for (var i = 0; i < 6; i++) {
                        $scope.currentQuestion.Answers.push(createAnswer());
                    }
                    return;
                }
                if (type == 'wpisanie odpowiedzi') {
                    $scope.selectedQuestion = 3;
                    $scope.currentQuestion.Type = 2;
                    $scope.currentQuestion.Answers = [];
                    for (var i = 0; i < 1; i++) {
                        $scope.currentQuestion.Answers.push(createAnswer());
                    }
                    return;
                }
                $scope.selectedQuestion = 0;
            }

            $scope.nextQuestion = function () {

                $scope.ErrorMessage = "";

                if (!$scope.currentQuestion) {
                    return;
                }

                if (!$scope.showQuestionType && !$scope.currentQuestion.Text) {
                    $scope.showQuestionType = true;
                    return;
                }

                var isValid = true;

                if (!$scope.currentQuestion.Text || $scope.currentQuestion.Text.length < 10) {
                    isValid = false;
                }

                var checkScore = false;
                var checkAnswerCount = 0;

                if ($scope.currentQuestion.Type === 0 || $scope.currentQuestion.Type === 1) {
                    angular.forEach($scope.currentQuestion.Answers, function (val) {
                        if (!!val.Text && val.Text.length > 0) {
                            checkAnswerCount++;
                            if (!!val.Score && val.Score > 0 && val.Score < 100) {
                                checkScore = true;
                            }
                        }
                        
                        if (!!val.Score && val.Score < 0 || val.Score > 100) {
                            isValid = false;
                        }

                    });

                    if (!checkScore) {
                        $scope.ErrorMessage += "Przy najmniej jedna odpowiedź musi być prawidłowa - wyznacz za nią punkty! <br> ";
                        isValid = false;
                    }

                    if (checkAnswerCount < 2) {
                        $scope.ErrorMessage += "Wpisz przy najmniej 2 odpowiedzi na pytanie! <br>";
                        isValid = false;
                    }
                }

                if ($scope.currentQuestion.Type === 2) {
                    if (!$scope.currentQuestion.Answers[0].Text || $scope.currentQuestion.Answers[0].Text.length < 1) {
                        $scope.ErrorMessage += "Wpisz chodź jedną odpowiedź na pytanie <br>";
                        isValid = false;
                    }

                    if (!!$scope.currentQuestion.Answers[0].Score && $scope.currentQuestion.Answers[0].Score > 0 && $scope.currentQuestion.Answers[0].Score < 100) {
                        $scope.ErrorMessage += "Przy najmniej jedna odpowiedź musi być prawidłowa - wyznacz za nią punkty! <br>";
                        isValid = false;
                    }
                }

                if (!isValid) {
                    return;
                }

                $scope.currentQuestion.isEdit = false;
                $scope.questions.push($scope.currentQuestion);
                $scope.changeQuestion("");
                $scope.selected = "Wybierz";
            }

            createAnswer = function () {
                var obj = {};
                obj.Text = '';
                obj.IsSelected = false;
                obj.Score = 0;
                return obj;
            }

        }]
    };
}]);