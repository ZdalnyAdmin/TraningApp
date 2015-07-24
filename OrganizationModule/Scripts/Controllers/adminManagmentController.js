function adminManagmentController($scope, $rootScope, $http, $modal, UtilitiesFactory) {
    $scope.addMode = false;
    $scope.viewModel = {};
    $scope.index = 1;

    $scope.loadData = function () {

        UtilitiesFactory.showSpinner();
        $scope.viewModel.ActionType = 0;

        $http.post('/api/TrainingManagment/', $scope.viewModel).success(function (data) {
            $scope.viewModel = data;
            $scope.index = 1;
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $rootScope.$broadcast('showGlobalMessage', {
                success: false,
                messageText: "Wystapil problem z pobraniem danych!"
            });
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.loadData();

    $scope.changeStatus = function (training) {
        if (!training) {
            return;
        }

        training.IsActive = !training.IsActive;

        UtilitiesFactory.showSpinner();

        $scope.viewModel.Current = training;

        $http.put('/api/TrainingManagment/', $scope.viewModel).success(function (data) {
            $scope.loadData();
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $rootScope.$broadcast('showGlobalMessage', {
                success: false,
                messageText: "Wystapil problem z pobraniem danych!"
            });
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.edit = function (training) {
        if (!training) {
            return;
        }


        $scope.viewModel.Current = training;

        var modalInstance = $modal.open({
            templateUrl: '/Templates/Modals/editExternalTrainingsModal.html',
            controller: 'editExternalTrainingsModalController',
            size: 'sm',
            resolve: {
                selectedGroups: function () {
                    return $scope.viewModel.Current.AssignedGroups;
                },
                groups : function () {
                    return $scope.viewModel.Groups;
                },
            }
        });

        modalInstance.result.then(function (selectedGroups) {
            if (!!selectedGroups) {
                UtilitiesFactory.showSpinner();

                $scope.viewModel.Current.Groups =selectedGroups;

                $scope.viewModel.ActionType = 3;

                $http.post('/api/TrainingManagment/', $scope.viewModel).success(function (data) {
                    $scope.viewModel = data;
                    UtilitiesFactory.hideSpinner();
                })
                .error(function () {
                    $rootScope.$broadcast('showGlobalMessage', {
                        success: false,
                        messageText: "Wystapil problem z zapisem danych!"
                    });
                    UtilitiesFactory.hideSpinner();
                });
            }
        });
    }

    $scope.showLogs = function (training) {
        training.showLog = true;
    }

    $scope.hideLogs = function (training) {
        training.showLog = false;
    }

    $scope.showTab = function (index) {
        $scope.index = index;

        if ($scope.index == 2) {
            UtilitiesFactory.showSpinner();
            $scope.viewModel.ActionType = 1;

            $http.post('/api/TrainingManagment', $scope.viewModel).success(function (data) {
                $scope.viewModel = data;
                UtilitiesFactory.hideSpinner();
            })
            .error(function () {
                $rootScope.$broadcast('showGlobalMessage', {
                    success: false,
                    messageText: "Wystapil problem z pobraniem danych!"
                });
                UtilitiesFactory.hideSpinner();
            });

        }
    }
    
}

adminManagmentController.$inject = ['$scope', '$rootScope', '$http', '$modal', 'UtilitiesFactory'];