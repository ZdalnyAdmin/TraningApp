window.App
.directive('ngTrainingAddQuestion', [function () {
    return {
        scope: { questions: '=' },
        restrict: 'A',
        replace: 'true',
        templateUrl: 'Templates/trainingAddQuestion.html',
        controller: ['$scope', function ($scope) {
            $scope.questionType = ['Wybierz','jednokrotnego wyboru', 'wielokrotnego wyboru', 'wpisanie odpowiedzi'];
            $scope.selectedQuestion = 0;
            $scope.currentQuestion = {};
            $scope.showQuestionType = true;
            $scope.currentQuestion.selected = 'Wybierz';

            //question methods
            $scope.changeQuestion = function (type) {
                $scope.showQuestionType = false;
                $scope.currentQuestion = {};
                $scope.currentQuestion.selected = 'Wybierz';
                if (type == 'jednokrotnego wyboru') {
                    $scope.selectedQuestion = 1;
                    $scope.currentQuestion.Type = 0;
                    $scope.currentQuestion.Answers = [];
                    for (var i = 0; i < 2; i++) {
                        $scope.currentQuestion.Answers.push(createAnswer());
                    }
                    return;
                }
                if (type == 'wielokrotnego wyboru') {
                    $scope.selectedQuestion = 2;
                    $scope.currentQuestion.Type = 1;
                    $scope.currentQuestion.Answers = [];
                    for (var i = 0; i < 2; i++) {
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

            }

            $scope.addAnswer = function () {
                if (!$scope.currentQuestion.Answers) {
                    $scope.currentQuestion.Answers = [];
                }


                if ($scope.currentQuestion.Answers.length > 6) {
                    $scope.ErrorMessage = "Maksymalnie moze byc 6 odpowiedzi";
                    return;
                }

                $scope.currentQuestion.Answers.push(createAnswer());
            }

            $scope.removeAnswer = function (answer) {

                if (!$scope.currentQuestion.Answers) {
                    $scope.currentQuestion.Answers = [];
                }

                var index = 0;
                for (var i = 0; i < $scope.currentQuestion.Answers.length; i++) {
                    if ($scope.currentQuestion.Answers[i] == answer) {
                        index = i;
                        break;
                    }
                }
                //delete element from list
                $scope.currentQuestion.Answers.splice(index, 1);
            }

            $scope.addQuestion = function () {

                $scope.ErrorMessage = "";

                if (!$scope.currentQuestion) {
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


                    if (!$scope.currentQuestion.Answers[0].Score || $scope.currentQuestion.Answers[0].Score < 0 || $scope.currentQuestion.Answers[0].Score > 100) {
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
                $scope.currentQuestion.selected = 'Wybierz';
                $scope.showQuestionType = true;
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