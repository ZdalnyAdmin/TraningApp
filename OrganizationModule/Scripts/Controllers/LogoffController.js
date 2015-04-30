var LogoffController = function ($scope, $window, $timeout, $location, UserFactory) {
    (function () {
        var result = UserFactory.logoff();
        result.then(function (result) {
            if (result.success) {
                $timeout(function () {
                    $location.path('/');
                    $window.location.reload();
                }, 1000);
            }
        });
    })();
};

LogoffController.$inject = ['$scope', '$window', '$timeout', '$location', 'UserFactory'];
