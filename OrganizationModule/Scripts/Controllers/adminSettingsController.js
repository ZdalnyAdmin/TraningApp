function adminSettingsController($scope, $http, $modal, UserFactory, UtilitiesFactory) {
    $scope.currentItem = {};
    $scope.viewModel = {};

    //Used to display the data 
    $scope.loadData = function () {
        UtilitiesFactory.showSpinner();
        $scope.currentItem = {};
        $scope.viewModel.ActionType = 0;

        $http.post('/api/Settings/', $scope.viewModel)
        .success(function (data) {
            $scope.viewModel = data;
            if ($scope.viewModel.CurrentOrganization) {
                $scope.currentItem.ChangeMail = $scope.viewModel.CurrentOrganization.CanUserChangeMail ? "1" : "0";
                $scope.currentItem.ChangeName = $scope.viewModel.CurrentOrganization.CanUserChangeName ? "1" : "0";
            }
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.viewModel.ErrorMessage = 'Wystąpił nieoczekiwany błąd podczas pobierania ustawień';
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

        $scope.viewModel.ActionType = 2;

        $scope.viewModel.CurrentOrganization.CanUserChangeName = $scope.currentItem.ChangeName == "1";
        $scope.viewModel.CurrentOrganization.CanUserChangeMail = $scope.currentItem.ChangeMail == "1";
       
        $http.post('/api/Settings/', $scope.viewModel)
        .success(function (data) {
            $scope.viewModel = data;
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.viewModel.ErrorMessage = 'Wystąpił nieoczekiwany błąd podczas zapisu ustawień';
            UtilitiesFactory.hideSpinner();
        });

    }
}

adminSettingsController.$inject = ['$scope', '$http', '$modal', 'UserFactory', 'UtilitiesFactory'];