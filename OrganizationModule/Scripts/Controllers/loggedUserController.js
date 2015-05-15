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

    $scope.changeEmailModel = {};

    $scope.changeEmail = function () {
        UtilitiesFactory.showSpinner();
        $scope.changeEmailModel.Errors = undefined;

        $http.post('/User/ChangeName', $scope.changeEmailModel)
             .success(function (data) {
                 UtilitiesFactory.hideSpinner();

                 if (!data.Succeeded) {
                     $scope.changeEmailModel.Errors = data.Errors.join();
                 } else {
                     $scope.changeEmailModel.Errors = 'Email został wysłany';
                 }
             })
             .error(function () {
                 $scope.changeEmailModel.Errors = 'Wystąpił nieoczekiwany błąd';
                 UtilitiesFactory.hideSpinner();
             });
    };

    $scope.changePasswordModel = {};

    $scope.changePassword = function () {
        UtilitiesFactory.showSpinner();
        $scope.changePasswordModel.Errors = undefined;

        $http.post('/User/ChangePassword', $scope.changePasswordModel)
             .success(function (data) {
                 UtilitiesFactory.hideSpinner();

                 if (!data.Succeeded) {
                     $scope.changePasswordModel.Errors = data.Errors.join();
                 }
             })
             .error(function () {
                 $scope.changePasswordModel.Errors = 'Wystąpił nieoczekiwany błąd';
                 UtilitiesFactory.hideSpinner();
             });
    };

    $scope.changeNameModel = {};

    $scope.changeName = function () {
        UtilitiesFactory.showSpinner();
        $scope.changeNameModel.Errors = undefined;

        $http.post('/User/ChangeName', $scope.changeNameModel)
             .success(function (data) {
                 UtilitiesFactory.hideSpinner();

                 if (!data.Succeeded) {
                     $scope.changeNameModel.Errors = data.Errors.join();
                 } else {
                     $scope.changeNameModel.Errors = 'Email został wysłany';
                 }
             })
             .error(function () {
                 $scope.changeNameModel.Errors = 'Wystąpił nieoczekiwany błąd';
                 UtilitiesFactory.hideSpinner();
             });
    };
}

loggedUserController.$inject = ['$scope', '$http', '$modal', '$window', 'UtilitiesFactory', '$location'];