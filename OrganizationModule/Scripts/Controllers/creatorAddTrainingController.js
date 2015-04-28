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
            return;
        }
        if (type == 'wielokrotnego wyboru') {
            $scope.selectedQuestion = 2;
            $scope.currentQuestion.Type = 1;
            return;
        }
        if (type == 'wpisanie odpowiedzi') {
            $scope.selectedQuestion = 3;
            $scope.currentQuestion.Type = 2;
            return;
        }
        $scope.selectedQuestion = 0;
    }

    $scope.nextQuestion = function()
    {
        $scope.showQuestionType = true;
        if ($scope.currentQuestion && $scope.currentQuestion.Text) {

            $scope.currentQuestion.isEdit = false;
            $scope.trainingQuestion.push($scope.currentQuestion);
        }
        $scope.selectedQuestion = 0;
    }

    $scope.questionUP = function (question) {
        for (var i = 0; i < $scope.trainingQuestion.length; i++) {
            if ($scope.trainingQuestion[i] == question) {
                index = i;
                break;
            }
        }

        //todo
    }

    $scope.questionDown = function (question) {
        for (var i = 0; i < $scope.trainingQuestion.length; i++) {
            if ($scope.trainingQuestion[i] == question) {
                index = i;
                break;
            }
        }

        //todo
    }

    $scope.questionEdit = function (question) {
        question.isEdit = true;
    }

    $scope.questionDelete = function (question) {
        for (var i = 0; i < $scope.trainingQuestion.length; i++) {
            if ($scope.trainingQuestion[i] == question) {
                index = i;
                break;
            }
        }
        if (index) {
            $scope.trainingQuestion.splice(index, 1);
        }
    }

    $scope.questionSave = function (question) {
        question.isEdit = false;
    }

    $scope.questionCancel = function (question) {
        question.isEdit = false;
    }

}

creatorAddTrainingController.$inject = ['$scope', '$http', '$modal'];