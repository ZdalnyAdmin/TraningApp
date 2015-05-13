function loggedUserController($scope, $http, $modal, $window, UtilitiesFactory, $location)
{
    $scope.deleteUser = function () {
        var modalInstance = $modal.open({
            templateUrl: '/Templates/Modals/deleteUserModal.html',
            controller: 'confirmModalController',
            size: 'sm',
            resolve: {
                modalResult: {}
            }
        });

        modalInstance.result.then(function () {
            UtilitiesFactory.showSpinner();

            $http.post('/User/DeleteUser')
                .success(function (data) {
                    UtilitiesFactory.hideSpinner();
                    $location.path('/');
                    $window.location.reload();
                })
                .error(function () {
                    UtilitiesFactory.hideSpinner();
                });
        });
    };
}

loggedUserController.$inject = ['$scope', '$http', '$modal', '$window', 'UtilitiesFactory', '$location'];