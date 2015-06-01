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

                if (!$scope.currentQuestion) {
                    return;
                }
                $scope.ErrorMessage = "";
                var isValid = true;

                if (!$scope.currentQuestion.Text || $scope.currentQuestion.Text.length < 10) {
                    $scope.ErrorMessage += "Wpisz pytanie na przynajmniej 10 znaków!";
                    isValid = false;
                }

                //if (!$scope.currentQuestion.Type === 0) {


                //    $scope.ErrorMessage += "Wpisz pytanie na przynajmniej 10 znaków!";
                //    isValid = false;
                //}

                //if (!$scope.currentQuestion.Type === 1) {
                //    $scope.ErrorMessage += "Wpisz pytanie na przynajmniej 10 znaków!";
                //    isValid = false;
                //}

                //if (!$scope.currentQuestion.Type === 2) {
                //    $scope.ErrorMessage += "Wpisz pytanie na przynajmniej 10 znaków!";
                //    isValid = false;
                //}


                $scope.showQuestionType = true;
                if ($scope.currentQuestion && $scope.currentQuestion.Text) {

                    $scope.currentQuestion.isEdit = false;
                    $scope.questions.push($scope.currentQuestion);
                    $scope.changeQuestion("");
                    $scope.selected = "Wybierz";
                }
                
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