var UserFactory = function ($http, $q, $rootScope) {
    var currentUser = null;

    var login = function (emailAddress, password) {

        var deferredObject = $q.defer();

        $http.post(
            '/Account/Login', {
                Email: emailAddress,
                Password: password
            }
        ).
        success(function (data) {
            deferredObject.resolve(data);
        }).
        error(function () {
            deferredObject.resolve({ Succeeded: false });
        });

        return deferredObject.promise;
    };

    var logoff = function () {
        currentUser = null;
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

    var inviteUser = function (inviteObject) {

        var deferredObject = $q.defer();

        $http.post(
            '/Manager/Invitation', inviteObject
        ).
        success(function (data) {
            deferredObject.resolve(data);
        }).
        error(function () {
            deferredObject.resolve(data);
        });

        return deferredObject.promise;
    };

    var removeInvitation = function (user) {

        var deferredObject = $q.defer();

        $http.post(
            '/Manager/RemoveInvitation', user
        ).
        success(function (data) {
            deferredObject.resolve(data);
        }).
        error(function () {
            deferredObject.resolve(data);
        });

        return deferredObject.promise;
    };

    var deleteUser = function (user) {

        var deferredObject = $q.defer();

        $http.post(
            '/Account/DeleteUserMail', user
        ).
        success(function (data) {
            deferredObject.resolve(data);
        }).
        error(function () {
            deferredObject.resolve(data);
        });

        return deferredObject.promise;
    };

    var getLoggedUser = function () {
        var deferredObject = $q.defer();

        if (currentUser) {
            deferredObject.resolve(currentUser);
            return deferredObject.promise;
        }

        $http.post(
            '/Account/GetLoggedUser'
        ).
        success(function (data) {
            currentUser = data;
            deferredObject.resolve(currentUser);
        }).
        error(function () {
            currentUser = null;
            deferredObject.resolve(null);
        });

        return deferredObject.promise;
    };

    var clearUser = function () {
        currentUser = null;
    };

    var checkUser = function () {
        if (!currentUser) {
            return;
        }

        $http.post(
            '/Account/Check', {
                Id: currentUser.Id
            }
        ).
        success(function (data) {
            if (data === 'False') {
                clearUser();
                $rootScope.$broadcast('userChanged');
            }
        }).
        error(function () {
            clearUser();
            $rootScope.$broadcast('userChanged');
        });
    };

    return {
        login: login,
        logoff: logoff,
        register: register,
        reset: reset,
        inviteUser: inviteUser,
        removeInvitation: removeInvitation,
        resetPasswordConfirmation: resetPasswordConfirmation,
        getLoggedUser: getLoggedUser,
        deleteUser: deleteUser,
        clearUser: clearUser,
        checkUser: checkUser,
        currentUser: currentUser
    }
};

UserFactory.$inject = ['$http', '$q', '$rootScope'];