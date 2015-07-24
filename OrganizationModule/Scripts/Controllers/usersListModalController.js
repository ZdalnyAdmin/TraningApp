var usersListModalController = function ($scope, $rootScope, $http, $modalInstance, UtilitiesFactory, selectedUsers) {
    $scope.viewModel = {};
    //temp solution
    $scope.selectedUsers = selectedUsers;

    //Used to display the data 
    $scope.loadData = function () {
        UtilitiesFactory.showSpinner();
        $scope.viewModel.ActionType = 4;
        $http.post('/api/Person/', $scope.viewModel).success(function (data) {
            $scope.viewModel = data;
            
            angular.forEach($scope.viewModel.People, function (x) {
                angular.forEach($scope.selectedUsers, function (y) {
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
        var selectedUsers = new Array();
        angular.forEach($scope.viewModel.People, function (user) {
            if (user.selected) {
                selectedUsers.push(user);
            }
        });

        $modalInstance.close(selectedUsers);
    };
};

usersListModalController.$inject = ['$scope', '$rootScope', '$http', '$modalInstance', 'UtilitiesFactory', 'selectedUsers'];