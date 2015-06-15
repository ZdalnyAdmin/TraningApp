window.App
.directive('ngMenuMaintenance', [function () {
    return {
        scope: true,
        restrict: 'A',
        replace: 'false',
        controller: ['$scope', '$location', '$rootScope', function ($scope, $location, $rootScope) {
            function maintenance() {
                var path = $location.path();

                var menuElements = $('.navbar li');

                for (var i = 0; i < menuElements.length; i++) {
                    var el = menuElements[i];
                    var href = $(el).find('a').attr('href');

                    if (path == href) {
                        $(el).addClass('active');
                    } else {
                        $(el).removeClass('active');
                    }
                }
            }

            $rootScope.$on('$routeChangeStart', maintenance);

            maintenance();
        }]
    }
}]);