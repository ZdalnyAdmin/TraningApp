function trainingListController($scope, $http, $rootScope, $modal, $location) {
    $scope.display = 'All';

    var trainings = angular.element.find('.course');

    angular.forEach(trainings, function (t) {
        var training = $(t);

        var desc = training.find('.description');
        var content = desc.html();

        if (content.length > 600) {
            var shortDesc = content.substring(0, 599) + '...';
            training.find('.less').hide();
            desc.html(shortDesc);

            training.find('.show-less-more').click(function () {
                if (training.find('.less').is(':visible')) {
                    training.find('.less').hide();
                    training.find('.more').show();

                    desc.html(shortDesc);
                } else {
                    training.find('.less').show();
                    training.find('.more').hide();

                    desc.html(content);
                }   
            });
        } else {
            training.find('.show-less-more').hide();
        }
    });

    $scope.changeDisplayedTrainings = function (mode) {
        $scope.display = mode;
    };

    $scope.startTraining = function (trainingID) {
        var modalInstance = {};
        var message = 'Wystąpił błąd podczas połączenia';

        $http.post(
            '/Training/TrainingList', {
                TrainingID: trainingID
            }
        ).
        success(function (data) {
            if (data.Succeeded) {
                $rootScope.$broadcast('reloadMenu');
                $location.path('/ActiveTraining/' + trainingID);
            } else {
                message = data.Errors.join();

                modalInstance = $modal.open({
                    templateUrl: '/Templates/Modals/messageModal.html',
                    controller: 'confirmModalController',
                    size: 'sm',
                    resolve: {
                        modalResult: function () { return message; }
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

trainingListController.$inject = ['$scope', '$http', '$rootScope', '$modal', '$location'];
