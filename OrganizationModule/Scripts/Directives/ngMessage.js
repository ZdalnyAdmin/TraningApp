window.App
.directive('ngMessages', function () {
    return {
        scope: true,
        restrict: 'A',
        replace: 'false',
        controller: ['$scope', '$element', '$compile', '$rootScope', '$timeout', function ($scope, $element, $compile, $rootScope, $timeout) {
            var messageTemplate = "<div id='%id%' ng-class='{success: %isSuccess%, error: %isError%}'>%message% <div class='close' ng-click=\"close('%id%')\" >X</div></div>";
            var timeouts = [];

            function guid() {
                function _p8(s) {
                    var p = (Math.random().toString(16) + "000000000").substr(2, 8);
                    return s ? p.substr(0, 4) + p.substr(4, 4) : p;
                }
                return _p8() + _p8(true) + _p8(true) + _p8();
            }

            $scope.close = function (object) {
                document.getElementById(object).outerHTML = '';

                if (!!timeouts[object]) {
                    $timeout.cancel(timeouts[object]);
                }
            };

            $rootScope.$on('showGlobalMessage', function (e, message) {
                var id = guid();
                var isSuccess = !!message.success;
                var isError = !message.success;
                var messageText = message.messageText;

                var template = messageTemplate;

                var re = new RegExp('%id%', 'g');
                template = template.replace(re, id)
                                   .replace('%isSuccess%', isSuccess)
                                   .replace('%isError%', isError)
                                   .replace('%message%', messageText);

                var compiledMessage = $compile(template)($scope);
                $element.append(compiledMessage);

                if (isSuccess) {
                    timeouts[id] = $timeout(function () { $scope.close(id); }, 5000, true);
                }
            });
        }]
    }
});