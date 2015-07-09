window.App
.directive('ngHrefReload', ['$location', '$route',
        function ($location, $route) {
            return function (scope, element, attrs) {
                scope.$watch('ngHrefReload', function () {
                    if (attrs.ngHrefReload) {
                        element.attr('href', attrs.ngHrefReload);
                        element.bind('click', function (event) {
                            scope.$apply(function () {
                                if ($location.path() == attrs.ngHrefReload) {
                                    $route.reload();
                                }
                            });
                        });
                    }
                });
            }
        }]);