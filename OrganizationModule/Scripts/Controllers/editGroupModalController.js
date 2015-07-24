var editGroupModalController = function ($scope, $rootScope, $http, $modalInstance, UtilitiesFactory, group) {
    $scope.viewModel = {};
    //temp solution
    $scope.Current = group;

    $scope.loadData = function () {
        UtilitiesFactory.showSpinner();
        $scope.viewModel.ActionType = 4;
        $http.post('/api/Person/', $scope.viewModel).success(function (data) {
            $scope.viewModel = data;

            angular.forEach($scope.viewModel.People, function (x) {
                angular.forEach($scope.Current.AssignedPeople, function (y) {
                    if (x.Id == y.Id) {
                        x.selected = true;
                    }
                })
            });

            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $rootScope.$broadcast('showGlobalMessage', {
                success: false,
                messageText: 'Wystąpił nieoczekiwany błąd podczas pobierania uzytkownikow'
            });

            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.loadData();

    $scope.close = function () {
        $modalInstance.close();
    };

    $scope.save = function () {

        $scope.Current.AssignedPeople = [];

        angular.forEach($scope.viewModel.People, function (user) {
            if (user.selected) {
                $scope.Current.AssignedPeople.push(user);
            }
        });

        $modalInstance.close($scope.Current);
    };
};

editGroupModalController.$inject = ['$scope', '$rootScope', '$http', '$modalInstance', 'UtilitiesFactory', 'group'];