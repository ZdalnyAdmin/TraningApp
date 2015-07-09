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

    $scope.deleteUserModel = {};

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
            $scope.deleteUserModel.Errors = undefined;

            $http.post('/User/DeleteUser')
                .success(function (data) {
                    UtilitiesFactory.hideSpinner();
                    $rootScope.$broadcast('showGlobalMessage', {
                        success: true,
                        messageText: 'Wysłano maila z linkiem do potwierdzenia usunięcia konta!'
                                                                });
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
                     $rootScope.$broadcast('showGlobalMessage', {
                         success: false,
                         messageText: data.Errors.join()
                     });
                 } else {
                     $rootScope.$broadcast('showGlobalMessage', {
                         success: true,
                         messageText: 'Email został wysłany !'
                     });
                 }

                 $scope.changeEmailModel.Email = '';
                 angular.element('.email.cancel-button').click();
             })
             .error(function () {
                 $rootScope.$broadcast('showGlobalMessage', {
                     success: false,
                     messageText: 'Wystąpił nieoczekiwany błąd'
                 });

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
                     $rootScope.$broadcast('showGlobalMessage', {
                         success: false,
                         messageText: data.Errors.join()
                     });
                     
                 } else {
                    $rootScope.$broadcast('showGlobalMessage', {
                        success: true,
                        messageText: data.Message || 'Email z potwierdzeniam zmiany hasła został wysłany'
                    });
                 }

                 $scope.changePasswordModel.UserName = '';
                 $scope.changePasswordModel.Password = '';
                 $scope.changePasswordModel.ConfirmPassword = '';
                 angular.element('.password.cancel-button').click();
             })
             .error(function () {
                 $rootScope.$broadcast('showGlobalMessage', {
                     success: false,
                     messageText: 'Wystąpił nieoczekiwany błąd'
                 });

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
                     $rootScope.$broadcast('showGlobalMessage', {
                         success: false,
                         messageText: data.Errors.join()
                     });

                 } else {
                     $rootScope.$broadcast('showGlobalMessage', {
                         success: true,
                         messageText: 'Nazwa wyświetlania została zmieniona'
                     });

                     UserFactory.clearUser();
                     var result = UserFactory.getLoggedUser();

                     result.then(function (user) {
                         if (user && user.Id) {
                             $scope.user = user;
                         } else {
                             $scope.user = undefined;
                         }

                         $scope.changeNameModel.UserName = '';
                         $rootScope.$broadcast('userChanged', { preventReloadMenu: true });

                         angular.element('.user-name.cancel-button').click();
                     });
                 }
             })
             .error(function () {
                 $rootScope.$broadcast('showGlobalMessage', {
                     success: false,
                     messageText: 'Wystąpił nieoczekiwany błąd'
                 });

                 angular.element('.user-name.cancel-button').click();
                 UtilitiesFactory.hideSpinner();
             });
    };
}

loggedUserController.$inject = ['$rootScope','$scope', '$http', '$modal', '$window', 'UtilitiesFactory', '$location', 'UserFactory'];