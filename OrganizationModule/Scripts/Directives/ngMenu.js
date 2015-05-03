window.App
.directive('ngMenu', [function () {
    return {
        scope: true,
        restrict: 'A',
        replace: 'true',
        templateUrl: 'Templates/menu.html',
        controller: ['$scope', 'UserFactory', '$location', '$rootScope', function ($scope, UserFactory, $location, $rootScope) {
            $scope.menuUrl = '';
            $scope.currentUser = {};
            $scope.visible = false;
            var MENU_URL = './Menu/Index';

            function reload() {
                $scope.menuUrl = '';
                $scope.currentUser = {};
                $scope.visible = false;

                var result = UserFactory.getLoggedUser();

                result.then(function (user) {
                    if (user && user.Id) {
                        $scope.menuUrl = MENU_URL;
                        $scope.visible = true;
                        $scope.currentUser = user;
                    } else {
                        if ($location.path().indexOf('/resetPassword') == -1 &&
                            $location.path().indexOf('/signin') == -1 &&
                            $location.path().indexOf('/registerUser') == -1) {
                                $location.path('/signin').search('');
                        }
                    }
                });
            }

            $rootScope.$on('userChanged', reload);

            reload();
        }]
    }
}]);