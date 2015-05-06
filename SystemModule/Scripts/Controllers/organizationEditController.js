function organizationEditController($scope, $http, $modal) {
    $scope.loading = true;
    $scope.list = [];
    $scope.availableStatus = ['Aktywny', 'Ukryty'];

    $scope.loadDate = function () {
        $http.get('/api/Organizations').success(function (data) {
            $scope.list = data;

            angular.forEach($scope.list, function (item) {
                if (item.Status == 1) {
                    item.selectedStatus = 'Aktywny';
                } else if (item.Status == 3) {
                    item.selectedStatus = 'Usuniety';
                } else if (item.Status == 2) {
                    item.selectedStatus = 'Ukryty';
                }
            });

            $scope.loading = false;
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });
    }

    $scope.loadDate();

    $scope.changeStatus = function (item) {
        if (!item) {
            return;
        }
        if (item.selectedStatus == 'Aktywny') {
            item.Status = 1;
        } else if (item.selectedStatus == 'Usuniety') {
            item.Status = 3;
        } else if (item.selectedStatus == 'Ukryty') {
            item.Status = 2;
        }


        $http.put('/api/Organizations', item).success(function (data) {
            $scope.loading = false;

        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });
    };

    $scope.delete = function (item) {

        $scope.current = item;


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
                item.DeletedReason = selectedReason.Text;
                item.IsDeleted = true;
                item.Status = 3;
                $http.put('/api/Organizations', item).success(function (data) {
                    $scope.loading = false;
                    item.DeletedDate = data.DeletedDate;
                })
                .error(function () {
                    $scope.error = "An Error has occured while loading posts!";
                    $scope.loading = false;
                });
            }
        });

    }
}

organizationEditController.$inject = ['$scope', '$http', '$modal'];