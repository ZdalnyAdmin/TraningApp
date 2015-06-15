var LogoffController = function ($scope, $window, $timeout, $location, UserFactory) {
    (function () {
        var result = UserFactory.logoff();
        result.then(function (result) {
            if (result.success) {
                $location.path('/');
                $window.location.reload();
            }
        });
    })();
};

LogoffController.$inject = ['$scope', '$window', '$timeout', '$location', 'UserFactory'];
