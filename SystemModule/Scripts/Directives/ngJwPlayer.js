﻿window.App
.directive('ngJwPlayer', ['$compile', function ($compile) {
    return {
        restrict: 'EC',
        scope: {
            playerId: '@',
            setupVars: '@setup'
        },
        link: function (scope, element, attrs) {
            var id = scope.playerId || 'random_player_' + Math.floor((Math.random() * 999999999) + 1),
                getTemplate = function (playerId) {
                    return '<div id="' + playerId + '"></div>';
                };

            setup = JSON.parse(scope.setupVars)

            element.html(getTemplate(id));
            $compile(element.contents())(scope);
            jwplayer(id).setup(setup);
        }
    };
}]);