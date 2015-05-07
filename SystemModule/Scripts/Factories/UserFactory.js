var UserFactory = function ($http, $q) {
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

    var registerOperator = function (registrationData) {

        var deferredObject = $q.defer();

        $http.post(
            '/Account/OperatorRegistry', registrationData
        ).
        success(function (data) {
            deferredObject.resolve(data);
        }).
        error(function () {
            deferredObject.resolve({ result: false });
        });

        return deferredObject.promise;
    };

    var operatorConfirmRegistration = function (registrationData) {

        var deferredObject = $q.defer();

        $http.post(
            '/Account/OperatorConfirmRegistration', registrationData
        ).
        success(function (data) {
            deferredObject.resolve(data);
        }).
        error(function () {
            deferredObject.resolve({ result: false });
        });

        return deferredObject.promise;
    };


    var organizationCreateMail = function (registrationData) {

        var deferredObject = $q.defer();

        $http.post(
            '/Account/OrganizationCreateMail', registrationData
        ).
        success(function (data) {
            deferredObject.resolve(data);
        }).
        error(function () {
            deferredObject.resolve({ result: false });
        });

        return deferredObject.promise;
    };

    var organizationDeleteMail = function (registrationData) {

        var deferredObject = $q.defer();

        $http.post(
            '/Account/OrganizationDeleteMail', registrationData
        ).
        success(function (data) {
            deferredObject.resolve(data);
        }).
        error(function () {
            deferredObject.resolve({ result: false });
        });

        return deferredObject.promise;
    };

    var organizationNameChangesMail = function (registrationData) {

        var deferredObject = $q.defer();

        $http.post(
            '/Account/OrganizationNameChangesMail', registrationData
        ).
        success(function (data) {
            deferredObject.resolve(data);
        }).
        error(function () {
            deferredObject.resolve({ result: false });
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

    var resetAdminPassword = function (resetObject) {

        var deferredObject = $q.defer();

        $http.post(
            '/Account/ResetAdminPassword', resetObject
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
        resetPasswordConfirmation: resetPasswordConfirmation,
        registerOperator: registerOperator,
        operatorConfirmRegistration : operatorConfirmRegistration,
        organizationCreateMail: organizationCreateMail,
        organizationDeleteMail: organizationDeleteMail,
        organizationNameChangesMail: organizationNameChangesMail,
        resetAdminPassword: resetAdminPassword
    }
}

UserFactory.$inject = ['$http', '$q'];
