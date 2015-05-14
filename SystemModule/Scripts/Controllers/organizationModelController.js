var organizationModalController = function ($scope, $http, $modalInstance, UtilitiesFactory) {
    $scope.viewModel = {};
    //temp solution

    //Used to display the data 
    $scope.loadData = function () {
        UtilitiesFactory.showSpinner();
        $scope.viewModel.ActionType = 4;

        $http.post('/api/Organizations/', $scope.viewModel).success(function (data) {
            $scope.viewModel = data;
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.viewModel.ErrorMessage = "An Error has occured while loading posts!";
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.loadData();

    $scope.close = function () {
        $modalInstance.close();
    };

    $scope.save = function () {
        var organizations = new Array();
        angular.forEach($scope.viewModel.Organizations, function (user) {
            if (user.selected) {
                organizations.push(user);
            }
        });

        $modalInstance.close(organizations);
    };
};

organizationModalController.$inject = ['$scope', '$http', '$modalInstance', 'UtilitiesFactory'];