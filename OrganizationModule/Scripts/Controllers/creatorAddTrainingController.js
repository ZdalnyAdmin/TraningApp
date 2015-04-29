function creatorAddTrainingController($scope, $http, $modal) {
    $scope.loading = true;
    $scope.trainingDetails = [];
    $scope.trainingQuestion = [];
    $scope.questionType = ['jednokrotnego wyboru', 'wielokrotnego wyboru', 'wpisanie odpowiedzi'];
    $scope.selectedQuestion = 0;
    $scope.currentQuestion = {};
    $scope.editableQuestion = {};
    $scope.currentDetail = {};
    $scope.editableDetail = {};
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

    //details methods
    $scope.upload = function (item, resourceType) {
        //todo
    }

    $scope.cancel = function (item, resourceType) {
        $scope.currentDetail = {};
    }

    $scope.add = function (item, resourceType) {
        if (!$scope.currentDetail)
        {
            return;
        }

        if($scope.currentDetail.Text && resourceType == 0)
        {
            $scope.currentDetail.isEdit = false;
            $scope.currentDetail.ResourceType = resourceType;
            $scope.trainingDetails.push($scope.currentDetail);
            $scope.currentDetail = {};
            return;
        }

        if ($scope.currentDetail.ExternalResource) {
            $scope.currentDetail.isEdit = false;
            $scope.currentDetail.ResourceType = resourceType;
            $scope.trainingDetails.push($scope.currentDetail);
            $scope.currentDetail = {};
            return;
        }
    }

    $scope.detailUp = function (item) {
        $scope.trainingDetails = changePosition($scope.trainingDetails, item, false);
    }

    $scope.detailDown = function (item) {
        $scope.trainingDetails = changePosition($scope.trainingDetails, item, true);
    }

    $scope.detailEdit = function (item) {
        $scope.editableDetail = angular.copy(item);
        item.isEdit = true;
    }

    $scope.detailDelete = function (item) {
        //todo remove source from server
        for (var i = 0; i < $scope.trainingDetails.length; i++) {
            if ($scope.trainingDetails[i] == item) {
                index = i;
                break;
            }
        }
        $scope.trainingDetails.splice(index, 1);
    }

    $scope.detailCancel = function (item) {
        item.Text = $scope.editableDetail.Text;
        item.ExternalResource = $scope.editableDetail.ExternalResource;
        $scope.editableDetail = {};
        item.isEdit = false;
    }

    $scope.detailSave = function (item) {
        $scope.editableDetail = {};
        item.isEdit = false;
    }

    //question methods
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
        $scope.editableQuestion = angular.copy(question);
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
        $scope.editableQuestion = {};
        question.isEdit = false;
    }

    $scope.questionCancel = function (question) {
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

    createAnswer = function () {
        var obj = {};
        obj.Text = '';
        obj.selected = false;
        obj.Score = '';
        return obj;
    }
}

creatorAddTrainingController.$inject = ['$scope', '$http', '$modal'];