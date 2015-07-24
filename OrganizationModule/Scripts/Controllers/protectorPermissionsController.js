function protectorPermissionsController($scope, $rootScope, $http, $modal, UserFactory, UtilitiesFactory) {
    $scope.viewModel = {};
    //Used to display the data 
    $scope.loadData = function () {

        UtilitiesFactory.showSpinner();
        $scope.viewModel.ActionType = 0;

        $http.post('/api/Settings/', $scope.viewModel)
        .success(function (data) {
            $scope.viewModel = data;
            if (!$scope.viewModel.Setting) {
                $rootScope.$broadcast('showGlobalMessage', {
                    success: false,
                    messageText: 'Wystąpił nieoczekiwany błąd podczas pobierania ustawień, Brak definicji ustawien globalnych dla organizacji.'
                });
            }
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $rootScope.$broadcast('showGlobalMessage', {
                success: false,
                messageText: 'Wystąpił nieoczekiwany błąd podczas pobierania ustawień'
            });
            UtilitiesFactory.hideSpinner();
        });
        //Used to display the data 

    }

    $scope.loadData();

    $scope.edit = function () {

        if (!$scope.viewModel.CurrentOrganization) {
            return;
        }
        UtilitiesFactory.showSpinner();

        $scope.viewModel.ActionType = 3;

        $http.post('/api/Settings/', $scope.viewModel)
        .success(function (data) {
            $scope.viewModel = data;
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $rootScope.$broadcast('showGlobalMessage', {
                success: false,
                messageText: 'Wystąpił nieoczekiwany błąd podczas zapisu ustawień'
            });
            UtilitiesFactory.hideSpinner();
        });

    }
}

protectorPermissionsController.$inject = ['$scope', '$rootScope', '$http', '$modal', 'UserFactory', 'UtilitiesFactory'];