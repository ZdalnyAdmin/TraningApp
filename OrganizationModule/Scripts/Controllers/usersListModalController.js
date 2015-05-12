var usersListModalController = function ($scope, $http, $modalInstance, UtilitiesFactory) {
    $scope.viewModel = {};
    //temp solution

    //Used to display the data 
    $scope.loadData = function () {
        UtilitiesFactory.showSpinner();
        $scope.viewModel.ActionType = 0;
        $http.post('/api/Person/', $scope.viewModel).success(function (data) {
            $scope.viewModel = data;
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.viewModel.ErrorMessage = 'Wystąpił nieoczekiwany błąd podczas pobierania uzytkownikow';
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

usersListModalController.$inject = ['$scope', '$http', '$modalInstance', 'UtilitiesFactory'];