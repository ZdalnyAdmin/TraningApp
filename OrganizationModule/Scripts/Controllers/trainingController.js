function trainingController($rootScope, $scope, $http, $modal, UtilitiesFactory, $templateCache, $route, $location, UserFactory) {
    $scope.answers = {};
    $scope.currentRate = 0;

    $scope.init = function (questionId, questionType) {
        var currentPageTemplate = $route.current.templateUrl;
        $templateCache.remove(currentPageTemplate({ trainingID: $scope.trainingID }));

        switch (questionType) {
            case 'Multi':
                $scope.answers[questionId] = [];
                break;
            default:
                $scope.answers[questionId] = '';
                break;
        }
    };

    $scope.rate = function (value) {
        $scope.currentRate = value;
    };

    $scope.summarize = function () {
        var path = $location.path();
        $http.post(
            '/Account/Check', {
                Id: (UserFactory.currentUser || {}).Id
            }
        ).
        success(function (data) {
            if (data === 'False') {
                $location.path('/signin').search({ returnUrl: path });
            } else {
                $scope.answer();
            }
        }).
        error(function () {
            $location.path('/signin').search({ returnUrl: path });
        });
    };

    $scope.answer = function () {
        var message = "";
        var modalInstance = {};

        var missingAnswer = false;

        angular.forEach($scope.answers, function (answer) {
            if (answer instanceof Array) {
                var selected = false;

                angular.forEach(answer, function (val) {
                    if (val) {
                        selected = true;
                    }
                });

                if (!selected) {
                    missingAnswer = true;
                }
            } else {
                if (!answer) {
                    missingAnswer = true;
                }
            }
        });

        if (missingAnswer) {
            message = "Niestety nie wypełniłeś wszystkich odpowiedzi. Wprowadź brakujace odpowiedzi i spróbuj jeszcze raz podsumować szkolenie.";
        }

        if ($scope.currentRate < 1) {
            message = "By podsumować kurs musisz go także ocenić. Twoja ocena pozwoli w przyszłości tworzyć jeszcze lepsze kursy.";
        }

        if (message) {
            modalInstance = $modal.open({
                templateUrl: '/Templates/Modals/messageModal.html',
                controller: 'confirmModalController',
                size: 'sm',
                resolve: {
                    modalResult: function () { return message; }
                }
            });

            return;
        }

        $http.post(
                '/Training/CheckIsTrainingActive', {
                    GenereateDate: $scope.generateDate,
                    TrainingID: $scope.trainingID
                }
            ).
            success(function (data) {
                if (data.Succeeded) {
                    checkDate();
                } else {
                    message = "To szkolenie nie jest już aktywne.";

                    modalInstance = $modal.open({
                        templateUrl: '/Templates/Modals/messageModal.html',
                        controller: 'confirmModalController',
                        size: 'sm',
                        resolve: {
                            modalResult: function () { return message; }
                        }
                    });

                    modalInstance.result.then(function (modalResult) {
                        if (modalResult !== undefined) {
                           $location.path('/');
                        }
                    });
                }
            }).
            error(function () {
                modalInstance = $modal.open({
                    templateUrl: '/Templates/Modals/messageModal.html',
                    controller: 'confirmModalController',
                    size: 'sm',
                    resolve: {
                        modalResult: function () { return message; }
                    }
                });
            });
    };

    function checkDate() {
        $http.post(
            '/Training/CheckDate', {
                GenereateDate: $scope.generateDate,
                TrainingID: $scope.trainingID
            }
        ).
        success(function (data) {
            if (data.Succeeded) {
                save();
            } else {
                message = "To szkolenie zostało w międzyczasie edytowane. Niezbędne jest jego odświeżenie do najnowszej wersji przed podsumowaniem.";

                modalInstance = $modal.open({
                    templateUrl: '/Templates/Modals/reloadModal.html',
                    controller: 'confirmModalController',
                    size: 'sm',
                    resolve: {
                        modalResult: function () { return message; }
                    }
                });

                modalInstance.result.then(function (modalResult) {
                    if (modalResult !== undefined) {
                        if (modalResult) {
                            var currentPageTemplate = $route.current.templateUrl;
                            $templateCache.remove(currentPageTemplate({ trainingID: $scope.trainingID }));
                            $route.reload();
                        }
                    }
                });
            }
        }).
        error(function () {
            modalInstance = $modal.open({
                templateUrl: '/Templates/Modals/messageModal.html',
                controller: 'confirmModalController',
                size: 'sm',
                resolve: {
                    modalResult: function () { return message; }
                }
            });
        });
    }

    function save() {
        var answers = {
            TrainingID: $scope.trainingID,
            GenereateDate: $scope.generateDate,
            TrainingAnswers: {},
            TrainingRate: $scope.currentRate
        };

        angular.forEach($scope.answers, function(answer, key) {
            var obj = {};
            answers.TrainingAnswers[key] = answer;

            if (answer instanceof Array) {
                var mulVal = '';

                angular.forEach(answer, function (val, keyVal) {
                    mulVal += keyVal + '-' + val + ';';
                });

                answers.TrainingAnswers[key] = mulVal;
            }
        });

        answers.TrainingAnswers = JSON.stringify(answers.TrainingAnswers);

        UtilitiesFactory.showSpinner();

        $http.post(
            '/Training/ActiveTraining', answers
        ).
        success(function (data) {
            UtilitiesFactory.hideSpinner();
            $rootScope.$broadcast('reloadMenu');
            $location.path('/userResult');
        }).
        error(function () {
            UtilitiesFactory.hideSpinner();
            var message = "Wystąpił błąd połączenia."
            modalInstance = $modal.open({
                templateUrl: '/Templates/Modals/messageModal.html',
                controller: 'confirmModalController',
                size: 'sm',
                resolve: {
                    modalResult: function () { return message; }
                }
            });
        });
    }
}

trainingController.$inject = ['$rootScope', '$scope', '$http', '$modal', 'UtilitiesFactory', '$templateCache', '$route', '$location', 'UserFactory'];
