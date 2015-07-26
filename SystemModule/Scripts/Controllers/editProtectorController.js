function editProtectorController($scope, $rootScope, $http, $modal, UserFactory, UtilitiesFactory) {
    $scope.viewModel = {};


    $scope.loadData = function () {
        UtilitiesFactory.showSpinner();
        $scope.viewModel.ActionType = 0;

        $http.post('/api/User/', $scope.viewModel).success(function (data) {
            angular.forEach(data.People, function (val) {
                val.showDetails = true;
            });
            $scope.viewModel = data;
            if ($scope.viewModel && $scope.viewModel.Success) {
                $rootScope.$broadcast('showGlobalMessage', {
                    success: true,
                    messageText: $scope.viewModel.Success
                });
            }

            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $rootScope.$broadcast('showGlobalMessage', {
                success: false,
                messageText: 'Wystąpił nieoczekiwany błąd podczas inicjalizacji danych'
            });

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
            $rootScope.$broadcast('showGlobalMessage', {
                success: false,
                messageText: "Nazwa opiekuna musi zawierac miedzy 3 a 30 znakow"
            });

            return;
        }

        var re = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;
        if (!item.Email || !re.test(item.Email)) {
            $rootScope.$broadcast('showGlobalMessage', {
                success: false,
                messageText: "Bledny format adresu email"
            });

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

            if ($scope.viewModel && $scope.viewModel.Success) {
                $rootScope.$broadcast('showGlobalMessage', {
                    success: true,
                    messageText: $scope.viewModel.Success
                });
            }

            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $rootScope.$broadcast('showGlobalMessage', {
                success: false,
                messageText: 'Wystąpił nieoczekiwany błąd podczas edycji opiekuna'
            });

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
            if ($scope.viewModel && $scope.viewModel.Success) {
                $rootScope.$broadcast('showGlobalMessage', {
                    success: true,
                    messageText: $scope.viewModel.Success
                });
            }

            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $rootScope.$broadcast('showGlobalMessage', {
                success: false,
                messageText: 'Wystąpił nieoczekiwany błąd podczas usuwania opiekuna'
            });

            UtilitiesFactory.hideSpinner();
        });
    }
}

editProtectorController.$inject = ['$scope', '$rootScope', '$http', '$modal', 'UserFactory', 'UtilitiesFactory'];