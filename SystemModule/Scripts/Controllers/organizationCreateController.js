function organizationCreateController($scope, $rootScope, $http, $modal, UserFactory, UtilitiesFactory) {
    $scope.viewModel = {};

    $scope.loadDate = function () {

        UtilitiesFactory.showSpinner();
        $scope.viewModel.ActionType = 5;

        $http.post('/api/Organizations/', $scope.viewModel).success(function (data) {
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

            if ($scope.viewModel && $scope.viewModel.Success) {
                $rootScope.$broadcast('showGlobalMessage', {
                    success: true,
                    messageText: $scope.viewModel.Success
                });
            }

            result.then(function (data) {
                if (data !== 'True') {
                    $rootScope.$broadcast('showGlobalMessage', {
                        success: false,
                        messageText: 'Wystąpił nieoczekiwany błąd podczas wysylania wiadomosci email'
                    });

                }
            });           
        })
        .error(function () {
            $rootScope.$broadcast('showGlobalMessage', {
                success: false,
                messageText: 'Wystąpił nieoczekiwany błąd podczas zapsiu danych'
            });

            UtilitiesFactory.hideSpinner();
        });
    }
}

organizationCreateController.$inject = ['$scope', '$rootScope', '$http', '$modal', 'UserFactory', 'UtilitiesFactory'];