var LoginFactory = function ($http, $q) {
    var login = function (emailAddress, password) {

        var deferredObject = $q.defer();

        $http.post(
            '/Account/Login', {
                Email: emailAddress,
                Password: password
            }
        ).
        success(function (data) {
            if (data == "True") {
                deferredObject.resolve({ success: true });
            } else {
                deferredObject.resolve({ success: false });
            }
        }).
        error(function () {
            deferredObject.resolve({ success: false });
        });

        return deferredObject.promise;
    };

    var logoff = function () {

        var deferredObject = $q.defer();

        $http.post(
            '/Account/Logoff'
        ).
        success(function (data) {
            if (data == "True") {
                deferredObject.resolve({ success: true });
            } else {
                deferredObject.resolve({ success: false });
            }
        }).
        error(function () {
            deferredObject.resolve({ success: false });
        });

        return deferredObject.promise;
    };

    return {
        login: login,
        logoff: logoff
    }
}

LoginFactory.$inject = ['$http', '$q'];