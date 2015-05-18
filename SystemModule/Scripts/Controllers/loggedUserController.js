function loggedUserController($rootScope, $scope, $http, $modal, $window, UtilitiesFactory, $location)
{
    $scope.resetPassword = function () {
        UtilitiesFactory.showSpinner();
        $scope.Message = undefined;

        $http.post('/Main/ResetUserPassword', $scope.changePasswordModel)
             .success(function (data) {
                 UtilitiesFactory.hideSpinner();

                 if (!data.Succeeded) {
                     $scope.Message = data.Errors.join();
                 } else {
                     $scope.Message = 'Email został wysłany';
                 }
             })
             .error(function () {
                 $scope.Message = 'Wystąpił nieoczekiwany błąd';
                 UtilitiesFactory.hideSpinner();
             });
    };
}

loggedUserController.$inject = ['$rootScope','$scope', '$http', '$modal', '$window', 'UtilitiesFactory', '$location'];