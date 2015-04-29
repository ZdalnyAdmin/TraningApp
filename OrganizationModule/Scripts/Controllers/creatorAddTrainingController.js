function creatorAddTrainingController($scope, $http, $modal) {
    $scope.loading = true;
    $scope.trainingDetails = [];
    $scope.trainingQuestion = [];
    $scope.questionType = ['jednokrotnego wyboru', 'wielokrotnego wyboru', 'wpisanie odpowiedzi'];
    $scope.selectedQuestion = 0;
    $scope.currentQuestion = {};
    $scope.showQuestionType = false;
    //Used to display the data 

    $scope.loadData = function () {

        $http.get('/api/Logs').success(function (data) {
            $scope.Logs = data;
            $scope.DbLogs = data;
            $scope.loading = false;
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });

        //Used to display the data 

    }


    $scope.loadGroups = function () {
        $http.get('/api/Group').success(function (data) {
            if (!data) {
                return;
            }
            $scope.Groups = data;
            $scope.loading = false;
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });
    }

    $scope.loadData();
    $scope.loadGroups();

    $scope.loadImage = function () {

    }

    $scope.loadIcon = function () {

    }

    $scope.showIcon = function () {

    }

    $scope.upload = function () {

    }

    $scope.cancel = function () {

    }

    $scope.save = function () {

    }

    $scope.add = function () {

    }

    $scope.changeQuestion = function (type) {
        $scope.showQuestionType = false;
        $scope.currentQuestion = {};
        if (type == 'jednokrotnego wyboru') {
            $scope.selectedQuestion = 1;
            $scope.currentQuestion.Type = 0;
            $scope.currentQuestion.Answers = [];
            for (var i = 0; i < 4; i++) {
                $scope.currentQuestion.Answers.push(createAnswer());
            }
            return;
        }
        if (type == 'wielokrotnego wyboru') {
            $scope.selectedQuestion = 2;
            $scope.currentQuestion.Type = 1;
            $scope.currentQuestion.Answers = [];
            for (var i = 0; i < 4; i++) {
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
        $scope.showQuestionType = true;
        if ($scope.currentQuestion && $scope.currentQuestion.Text) {

            $scope.currentQuestion.isEdit = false;
            $scope.trainingQuestion.push($scope.currentQuestion);
        }
        $scope.selectedQuestion = 0;
    }

    $scope.questionUP = function (question) {
        $scope.trainingQuestion = changePosition($scope.trainingQuestion, question, false);
    }

    $scope.questionDown = function (question) {
        $scope.trainingQuestion = changePosition($scope.trainingQuestion, question, true);
    }

    $scope.questionEdit = function (question) {
        $scope.currentQuestion = angular.copy(question);
        question.isEdit = true;
    }

    $scope.questionDelete = function (question) {
        for (var i = 0; i < $scope.trainingQuestion.length; i++) {
            if ($scope.trainingQuestion[i] == question) {
                index = i;
                break;
            }
        }
        $scope.trainingQuestion.splice(index, 1);
    }

    $scope.questionSave = function (question) {
        $scope.currentQuestion = {};
        question.isEdit = false;
    }

    $scope.questionCancel = function (question) {
        question.Text = $scope.currentQuestion.Text;
        question.Answers = $scope.currentQuestion.Answers;
        question.Score = $scope.currentQuestion.Score;
        question.selected = $scope.currentQuestion.selected;
        $scope.currentQuestion = {};
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

    createAnswer = function () {
        var obj = {};
        obj.Text = '';
        obj.selected = false;
        obj.Score = '';
        return obj;
    }

}

creatorAddTrainingController.$inject = ['$scope', '$http', '$modal'];