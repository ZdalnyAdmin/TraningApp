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

        $scope.viewModel.Current = item;

        if (item.selectedStatus == 'Aktywny') {
            $scope.viewModel.Current.Status = 1;
        } else if (item.selectedStatus == 'Usuniety') {
            $scope.viewModel.Current.Status = 3;
        } else if (item.selectedStatus == 'Ukryty') {
            $scope.viewModel.Current.Status = 2;
        }

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
            $scope.viewModel.ErrorMessage = "Wystąpił nieoczekiwany błąd podczas zmiany statusu organizacji";
            UtilitiesFactory.hideSpinner();
        });
    };

    $scope.showOrganizationDetails = function (item, show) {
        item.showDetails = show;
        $scope.NewName = item.NewName;

        if (!show) {
            return;
        }

        if (item.Status !== 1 && item.Status !== 2) {
            return;
        }

        UtilitiesFactory.showSpinner();
        $scope.viewModel.ActionType = 6;
        $scope.viewModel.OrganizationID = item.OrganizationID;

        $http.post('/api/Organizations/', $scope.viewModel).success(function (data) {
            $scope.viewModel.Detail = data.Detail;
            $scope.viewModel.Success = data.Success;
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
            if (!!selectedReason) {
                if (!selectedReason.Text) {
                    selectedReason.Text = '';
                }


                $scope.viewModel.ActionType = 1;

                UtilitiesFactory.showSpinner();

                $scope.viewModel.Current.DeletedReason = selectedReason.Text;
                $http.post('/api/Organizations/', $scope.viewModel).success(function (data) {
                    $scope.viewModel = data;

                    var result = UserFactory.organizationDeleteMail($scope.viewModel.Current);

                    result.then(function (data) {
                        if (data !== 'True') {
                            $scope.viewModel.ErrorMessage = 'Wystąpił nieoczekiwany błąd podczas usuniecia organizacji';
                        }
                    });

                    UtilitiesFactory.hideSpinner();

                })
                .error(function () {
                    $scope.viewModel.ErrorMessage = "Wystąpił nieoczekiwany błąd podczas usuniecia organizacji";
                    UtilitiesFactory.hideSpinner();
                });
            }
        });
    }

    $scope.changeName = function (item) {

        $scope.current = item;
        $scope.viewModel.Current = item;

        $scope.NewName = item.NewName;

        $scope.viewModel.ActionType = 1;

        var result = UserFactory.organizationNameChangesMail($scope.viewModel.Current);



        result.then(function (data) {
            if (data !== 'True') {
                $scope.viewModel.ErrorMessage = 'Wystąpił nieoczekiwany błąd podczas zmiany nazwy organizacji';
            }
            else {
                UtilitiesFactory.showSpinner();
                $scope.viewModel.ActionType = 2;
                $scope.viewModel.OrganizationID = item.OrganizationID;

                $scope.viewModel.Current.ChangeNameDate = new Date();

                $http.post('/api/Organizations/', $scope.viewModel).success(function (data) {
                    $scope.viewModel.Detail = data.Detail;
                    $scope.viewModel.Success = "Email z prosba o zmiane nazwy zostal wyslany";
                    UtilitiesFactory.hideSpinner();
                })
                .error(function () {
                    $scope.viewModel.ErrorMessage = 'Wystąpił nieoczekiwany błąd podczas edycji organizacji';
                    UtilitiesFactory.hideSpinner();
                });
            }
        });
    }

    $scope.edit = function (item) {

        item.NewName = $scope.NewName;

        $scope.current = item;
        $scope.viewModel.Current = item;

        UtilitiesFactory.showSpinner();
        $scope.viewModel.ActionType = 2;
        $scope.viewModel.OrganizationID = item.OrganizationID;

        $http.post('/api/Organizations/', $scope.viewModel).success(function (data) {
            $scope.viewModel.Detail = data.Detail;
            $scope.viewModel.Success = data.Success;
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.viewModel.ErrorMessage = 'Wystąpił nieoczekiwany błąd podczas edycji organizacji';
            UtilitiesFactory.hideSpinner();
        });
    }
}

organizationEditController.$inject = ['$scope', '$http', '$modal', 'UserFactory', 'UtilitiesFactory'];