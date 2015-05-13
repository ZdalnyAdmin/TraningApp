function trainingListController($scope, $http, $rootScope, $modal) {
    $scope.display = 'All';

    var trainings = angular.element.find('.training');

    angular.forEach(trainings, function (t) {
        var training = $(t);

        var desc = training.find('.description');
        var content = desc.html();

        if (content.length > 600) {
            var shortDesc = content.substring(0, 599) + '...';
            training.find('.hide-details').hide();
            desc.html(shortDesc);

            training.find('.details').click(function () {
                training.find('.hide-details').show();
                training.find('.details').hide();

                desc.html(content);
            });

            training.find('.hide-details').click(function () {
                training.find('.hide-details').hide();
                training.find('.details').show();

                desc.html(shortDesc);
            });
        } else {
            training.find('.details').hide();
            training.find('.hide-details').hide();
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

trainingListController.$inject = ['$scope', '$http', '$rootScope', '$modal'];
