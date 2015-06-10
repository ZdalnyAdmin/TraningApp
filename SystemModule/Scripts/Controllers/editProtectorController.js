function editProtectorController($scope, $http, $modal, UserFactory, UtilitiesFactory) {
    $scope.viewModel = {};


    $scope.loadData = function () {
        UtilitiesFactory.showSpinner();
        $scope.viewModel.ActionType = 0;

        $http.post('/api/User/', $scope.viewModel).success(function (data) {
            angular.forEach(data.People, function (val) {
                val.showDetails = true;
            });
            $scope.viewModel = data;
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.viewModel.ErrorMessage = 'Wystąpił nieoczekiwany błąd podczas inicjalizacji danych';
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.loadData();

    $scope.showDetails = function (item, showDetails) {
        item.showDetails = showDetails;
    }


    $scope.save = function (item) {

        $scope.viewModel.ErrorMessage = "";

        if (!item.UserName || item.UserName.length < 3 || item.UserName.length > 30) {
            $scope.viewModel.ErrorMessage = "Nazwa opiekuna musi zawierac miedzy 3 a 30 znakow";
            return;
        }

        var re = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;
        if (!item.Email || !re.test(item.Email)) {
            $scope.viewModel.ErrorMessage = "Bledny format adresu email";
            return;
        }

        UtilitiesFactory.showSpinner();
        $scope.viewModel.ActionType = 2;
        $scope.viewModel.Current = item;

        $http.post('/api/User/', $scope.viewModel).success(function (data) {
            angular.forEach(data.People, function (val) {
                val.showDetails = true;
            });
            $scope.viewModel = data;
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.viewModel.ErrorMessage = 'Wystąpił nieoczekiwany błąd podczas edycji opiekuna';
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.delete = function (item) {
        UtilitiesFactory.showSpinner();
        $scope.viewModel.ActionType = 1;
        $scope.viewModel.Current = item;

        $http.post('/api/User/', $scope.viewModel).success(function (data) {
            angular.forEach(data.People, function (val) {
                val.showDetails = true;
            });
            $scope.viewModel = data;
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.viewModel.ErrorMessage = 'Wystąpił nieoczekiwany błąd podczas usuwania opiekuna';
            UtilitiesFactory.hideSpinner();
        });
    }
}

editProtectorController.$inject = ['$scope', '$http', '$modal', 'UserFactory', 'UtilitiesFactory'];