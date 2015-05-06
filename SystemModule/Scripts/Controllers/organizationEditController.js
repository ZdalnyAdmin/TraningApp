function organizationEditController($scope, $http, $modal) {
    $scope.loading = true;
    $scope.list = [];
    $scope.availableStatus = ['Aktywny', 'Ukryty'];

    $scope.loadDate = function () {
        $http.get('/api/Organizations').success(function (data) {
            $scope.list = data;
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
    };

    $scope.delete = function(item)
    {
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
                item.DeletedReason = selectedReason;
            }

            item.DeletedDate = true;

            $http.put('/api/Organizations', item).success(function (data) {

                var index = 0;

                for (var i = 0; i < $scope.list.length; i++) {
                    if ($scope.list[i].IsDeleted) {
                        index = i;
                        break;
                    }
                }
                $scope.list.splice(index, 1);

                $scope.loading = false;
            })
            .error(function () {
                $scope.error = "An Error has occured while loading posts!";
                $scope.loading = false;
            });
        });

    }
}

organizationEditController.$inject = ['$scope', '$http', '$modal'];