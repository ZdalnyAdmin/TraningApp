var SystemModuleApp = angular.module('SystemModuleApp', ['ngRoute']);
SystemModuleApp.controller('BaseController', BaseController);
SystemModuleApp.controller('LoginController', LoginController);
SystemModuleApp.controller('LogoffController', LogoffController);
SystemModuleApp.controller('RegisterController', RegisterController); 
SystemModuleApp.controller('ResetPasswordController', ResetPasswordController);
SystemModuleApp.controller('ResetPasswordConfirmation', ResetPasswordConfirmation);
SystemModuleApp.factory('AuthHttpResponseInterceptor', AuthHttpResponseInterceptor);
SystemModuleApp.factory('UserFactory', UserFactory);
//SystemModuleApp.service('SessionService', SessionService);

var configFunction = function ($routeProvider, $httpProvider, $locationProvider) {

   $locationProvider.hashPrefix('!').html5Mode(true);

    $routeProvider.
        when('/currentUser', {
            templateUrl: 'Main/LoggedUser'
        })
        .when('/createOrganization', {
            templateUrl: 'Main/CreateOrganization'
        })
        .when('/organizationsList', {
            templateUrl: 'Main/OrganizationsList'
        })
        .when('/globalSetting', {
            templateUrl: 'Main/GlobalSetting'
        })
        .when('/statistics', {
            templateUrl: 'Main/Statistics'
        })
        .when('/createTraning', {
            templateUrl: 'Main/CreateTraning'
        })
        .when('/editTraning', {
            templateUrl: 'Main/Tranings'
        })
        .when('/createProtector', {
            templateUrl: 'Main/CreateProtectorRole'
        })
        .when('/editProtector', {
            templateUrl: 'Main/EditProtectorRole'
        })
        .when('/traningsList', {
            templateUrl: 'Main/TraningsList'
        })
        .when('/globalAdmins', {
            templateUrl: 'Main/GlobalAdmins'
        })
        .when('/history', {
            templateUrl: 'Main/History'
        })
        .when('/login', {
            templateUrl: 'Account/Login',
            controller: LoginController
        })
        .when('/resetPassword', {
            templateUrl: 'Account/ResetPassword',
            controller: ResetPasswordController
        })
        .when('/resetPasswordConfirmation', {
            templateUrl: 'Account/ResetPasswordConfirmation',
            controller: ResetPasswordConfirmation
        })
        .when('/register', {
            templateUrl: 'Account/Register',
            controller: RegisterController
        })
        .when('/logoff', {
            templateUrl: 'Account/Logoff',
            controller: LogoffController
        });
        
    $httpProvider.interceptors.push('AuthHttpResponseInterceptor');
}

configFunction.$inject = ['$routeProvider', '$httpProvider', '$locationProvider'];

SystemModuleApp.config(configFunction);