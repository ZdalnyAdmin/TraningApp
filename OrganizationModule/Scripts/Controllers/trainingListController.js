function trainingListController($scope, $http, $rootScope, $modal) {
    $scope.display = 'All';

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
