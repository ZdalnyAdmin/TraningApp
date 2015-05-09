function trainingController($scope, $http, $modal, UtilitiesFactory, $templateCache, $route) {
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
        var message = "";
        var modalInstance = {};

        var missingAnswer = false;

        angular.forEach($scope.answers, function (answer) {
            if(answer instanceof Array)
            {
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
        
        if ($scope.currentRate < 1)
        {
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
            '/Training/CheckDate', {
                GenereateDate: $scope.generateDate,
                TrainingID: $scope.trainingID
            }
        ).
        success(function (data) {
            if (data.Succeeded) {
                deferredObject.resolve({ success: true });
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

    };
}

trainingController.$inject = ['$scope', '$http', '$modal', 'UtilitiesFactory', '$templateCache', '$route'];
