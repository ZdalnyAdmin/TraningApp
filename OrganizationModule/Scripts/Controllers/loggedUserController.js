function loggedUserController($rootScope, $scope, $http, $modal, $window, UtilitiesFactory, $location, UserFactory)
{
    var result = UserFactory.getLoggedUser();

    result.then(function (user) {
        if (user && user.Id) {
            $scope.user = user;
        } else {
            $scope.user = undefined;
        }
    });

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

        $http.post('/User/ChangeEmail', $scope.changeEmailModel)
             .success(function (data) {
                 UtilitiesFactory.hideSpinner();

                 if (!data.Succeeded) {
                     $scope.changeEmailModel.Errors = data.Errors.join();
                 } else {
                     $scope.changeEmailModel.Errors = 'Email został wysłany';
                 }

                 $scope.changeEmailModel.Email = '';
                 angular.element('.email.cancel-button').click();
             })
             .error(function () {
                 $scope.changeEmailModel.Errors = 'Wystąpił nieoczekiwany błąd';
                 angular.element('.email.cancel-button').click();
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
                 } else {
                     $scope.changeEmailModel.Errors = 'Email z potwierdzeniam zmiany hasła został wysłany';
                 }

                 $scope.changePasswordModel.UserName = '';
                 $scope.changePasswordModel.Password = '';
                 $scope.changePasswordModel.ConfirmPassword = '';
                 angular.element('.password.cancel-button').click();
             })
             .error(function () {
                 $scope.changePasswordModel.Errors = 'Wystąpił nieoczekiwany błąd';
                 angular.element('.password.cancel-button').click();
                 UtilitiesFactory.hideSpinner();
             });
    };

    $scope.changeNameModel = {};

    $scope.changeName = function () {
        UtilitiesFactory.showSpinner();
        $scope.changeNameModel.Errors = undefined;

        $http.post('/User/ChangeUserName', $scope.changeNameModel)
             .success(function (data) {
                 UtilitiesFactory.hideSpinner();

                 if (!data.Succeeded) {
                     $scope.changeNameModel.Errors = data.Errors.join();
                 } else {
                     $scope.changeNameModel.Errors = 'Nazwa wyświetlania została zmieniona';
                     UserFactory.clearUser();
                     var result = UserFactory.getLoggedUser();

                     result.then(function (user) {
                         if (user && user.Id) {
                             $scope.user = user;
                         } else {
                             $scope.user = undefined;
                         }

                         $scope.changeEmailModel.Email = '';
                         $rootScope.$broadcast('userChanged', { preventReloadMenu: true });

                         angular.element('.user-name.cancel-button').click();
                     });
                 }
             })
             .error(function () {
                 $scope.changeNameModel.Errors = 'Wystąpił nieoczekiwany błąd';
                 angular.element('.user-name.cancel-button').click();
                 UtilitiesFactory.hideSpinner();
             });
    };
}

loggedUserController.$inject = ['$rootScope','$scope', '$http', '$modal', '$window', 'UtilitiesFactory', '$location', 'UserFactory'];