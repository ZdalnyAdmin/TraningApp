function organizationEditController($scope, $http, $modal, UserFactory, UtilitiesFactory) {
    $scope.viewModel = {};
    $scope.availableStatus = ['Aktywny', 'Ukryty'];

    $scope.loadDate = function () {

        UtilitiesFactory.showSpinner();
        $scope.viewModel.ActionType = 4;

        $http.post('/api/Organizations/', $scope.viewModel).success(function (data) {
            $scope.viewModel = data;

            angular.forEach($scope.viewModel.Organizations, function (item) {
                if (item.Status == 1) {
                    item.selectedStatus = 'Aktywny';
                } else if (item.Status == 3) {
                    item.selectedStatus = 'Usuniety';
                } else if (item.Status == 2) {
                    item.selectedStatus = 'Ukryty';
                }
            });

            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.viewModel.ErrorMessage = "An Error has occured while loading posts!";
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.loadDate();

    $scope.changeStatus = function (item) {
        if (!item) {
            return;
        }

        UtilitiesFactory.showSpinner();
        $scope.viewModel.ActionType = 2;

        if (item.selectedStatus == 'Aktywny') {
            $scope.viewModel.Current.Status = 1;
        } else if (item.selectedStatus == 'Usuniety') {
            $scope.viewModel.Current.Status = 3;
        } else if (item.selectedStatus == 'Ukryty') {
            $scope.viewModel.Current.Status = 2;
        }

        $http.post('/api/Organizations/', $scope.viewModel).success(function (data) {
            $scope.viewModel = data;
            UtilitiesFactory.hideSpinner();

        })
        .error(function () {
            $scope.viewModel.ErrorMessage = "Wystąpił nieoczekiwany błąd podczas zmiany statusu organizacji";
            UtilitiesFactory.hideSpinner();
        });
    };

    $scope.showOrganizationDetails = function (item, show) {
        item.showDetails = show;
        item.NewName = item.Name;

        if (!show) {
            return;
        }

        UtilitiesFactory.showSpinner();
        $scope.viewModel.ActionType = 6;
        $scope.viewModel.OrganizationID = item.OrganizationID;

        $http.post('/api/Organizations/', $scope.viewModel).success(function (data) {
            $scope.viewModel.Detail = data.Detail;

            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.viewModel.ErrorMessage = "An Error has occured while loading posts!";
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.delete = function (item) {
        $scope.current = item;
        $scope.viewModel.Current = item;

        var modalInstance = $modal.open({
            templateUrl: '/Templates/Modals/organizationDeleteModal.html',
            controller: 'organizationDeleteModalController',
            size: 'sm',
            resolve: {
                selectedReason: function () {
                    return $scope.selectedReason;
                }
            }
        });

        modalInstance.result.then(function (selectedReason) {
            if (!!selectedReason.Text) {

                $scope.viewModel.ActionType = 1;

                $scope.viewModel.Current.DeletedReason = selectedReason.Text;
                $http.post('/api/Organizations/', $scope.viewModel).success(function (data) {
                    $scope.viewModel = data;

                    var result = UserFactory.organizationDeleteMail($scope.viewModel.Current);

                    result.then(function (data) {
                        if (data !== 'True') {
                            $scope.viewModel.ErrorMessage = 'Wystąpił nieoczekiwany błąd podczas usuniecia organizacji';
                        }
                    });

                })
                .error(function () {
                    $scope.viewModel.ErrorMessage = "Wystąpił nieoczekiwany błąd podczas usuniecia organizacji";
                });
            }
        });
    }

    $scope.changeName = function (item) {
        if (item.NewName === item.Name) {
            return;
        }

        $scope.current = item;
        $scope.viewModel.Current = item;

        $scope.viewModel.ActionType = 1;

        var result = UserFactory.organizationNameChangesMail($scope.viewModel.Current);

        result.then(function (data) {
            if (data !== 'True') {
                $scope.viewModel.ErrorMessage = 'Wystąpił nieoczekiwany błąd podczas zmiany nazwy organizacji';
            }
        });
    }
}

organizationEditController.$inject = ['$scope', '$http', '$modal', 'UserFactory', 'UtilitiesFactory'];