function organizationCreateController($scope, $http, $modal, UserFactory, UtilitiesFactory) {
    $scope.viewModel = {};

    $scope.loadDate = function () {

        UtilitiesFactory.showSpinner();
        $scope.viewModel.ActionType = 5;

        $http.post('/api/Organizations/', $scope.viewModel).success(function (data) {
            $scope.viewModel = data;
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.viewModel.ErrorMessage = 'Wystąpił nieoczekiwany błąd podczas inicjalizacji danych';
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.loadDate();

    $scope.save = function () {
        if (!$scope.viewModel.Current.Name) {
            return;
        }

        UtilitiesFactory.showSpinner();
        $scope.viewModel.ActionType = 3;

        $http.post('/api/Organizations/', $scope.viewModel).success(function (data) {
            $scope.viewModel.Current.CreateUserID = $scope.viewModel.LoggedUser.Id;
            $scope.viewModel.Success = data.Success;
            var result = UserFactory.organizationCreateMail($scope.viewModel.Current);

            $scope.viewModel = data;
            UtilitiesFactory.hideSpinner();

            result.then(function (data) {
                if (data !== 'True') {
                    $scope.viewModel.ErrorMessage = 'Wystąpił nieoczekiwany błąd podczas wysylania wiadomosci email';
                }
            });           
        })
        .error(function () {
            $scope.viewModel.ErrorMessage = 'Wystąpił nieoczekiwany błąd podczas zapsiu danych';
            UtilitiesFactory.hideSpinner();
        });
    }
}

organizationCreateController.$inject = ['$scope', '$http', '$modal', 'UserFactory', 'UtilitiesFactory'];