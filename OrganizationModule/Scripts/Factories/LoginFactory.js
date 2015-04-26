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

    var register = function (registrationData) {

        var deferredObject = $q.defer();

        $http.post(
            '/Account/Register', registrationData
        ).
        success(function (data) {
            deferredObject.resolve(data);
        }).
        error(function () {
            deferredObject.resolve({ result: false });
        });

        return deferredObject.promise;
    };

    var reset = function (resetObject) {

        var deferredObject = $q.defer();

        $http.post(
            '/Account/ResetPassword', resetObject
        ).
        success(function (data) {
            deferredObject.resolve(data);
        }).
        error(function () {
            deferredObject.resolve(data);
        });

        return deferredObject.promise;
    };

    var resetPasswordConfirmation = function (resetObject) {

        var deferredObject = $q.defer();

        $http.post(
            '/Account/ResetPasswordConfirmation', resetObject
        ).
        success(function (data) {
            deferredObject.resolve(data);
        }).
        error(function () {
            deferredObject.resolve(data);
        });

        return deferredObject.promise;
    };

    return {
        login: login,
        logoff: logoff,
        register: register,
        reset: reset,
        resetPasswordConfirmation: resetPasswordConfirmation
    }
}

LoginFactory.$inject = ['$http', '$q'];