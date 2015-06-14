var AuthHttpResponseInterceptor = function ($q, $location, $rootScope) {
    return {
        response: function (response) {
            if (response.status === 401) {
                console.log("Response 401");
            }

            return response || $q.when(response);
        },
        responseError: function (rejection) {
            if (rejection.status === 401) {
                console.log("Response Error 401", rejection);
                $rootScope.$broadcast('userChanged');
                var searchPath = $location.path();
                if (['/signin', '/resetPasswordConfirmation','/register', '/logoff', '/userTranings'].indexOf(searchPath) !== -1) {
                    $location.path('/signin');
                } else {
                    $location.path('/signin').search({ returnUrl: searchPath });
                }
            } else if (rejection.status === 404 && !isNaN(rejection.statusText)) {
                console.log("Response Error 404", rejection);
                $location.path('/Error/' + rejection.statusText + '/').search('');
            }

            return $q.reject(rejection);
        }
    }
}

AuthHttpResponseInterceptor.$inject = ['$q', '$location', '$rootScope'];