var SystemModuleApp = window.App;

SystemModuleApp.controller('BaseController', BaseController);
SystemModuleApp.controller('LoginController', LoginController);
SystemModuleApp.controller('LogoffController', LogoffController);
SystemModuleApp.controller('RegisterController', RegisterController); 
SystemModuleApp.controller('ResetPasswordController', ResetPasswordController);
SystemModuleApp.controller('ResetPasswordConfirmation', ResetPasswordConfirmation);
SystemModuleApp.controller('loggedUserController', loggedUserController);
SystemModuleApp.controller('ViewWrapperController', ViewWrapperController);
SystemModuleApp.factory('AuthHttpResponseInterceptor', AuthHttpResponseInterceptor);
SystemModuleApp.factory('UserFactory', UserFactory);
SystemModuleApp.factory('UtilitiesFactory', UtilitiesFactory);
//SystemModuleApp.service('SessionService', SessionService);

var configFunction = function ($routeProvider, $httpProvider, $locationProvider) {

   $locationProvider.hashPrefix('!').html5Mode(true);

    $routeProvider.
        when('/currentUser', {
            templateUrl: 'Main/LoggedUser',
            controller: loggedUserController
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
            templateUrl: 'Main/InternalTrainings'
        })
        .when('/globalAdmins', {
            templateUrl: 'Main/GlobalAdmins'
        })
        .when('/history', {
            templateUrl: 'Main/History'
        })
        .when('/signin', {
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
        .when('/Templates/delOrganization', {
            templateUrl: 'viewWrapper.html',
            controller: ViewWrapperController
        })
        .when('/Templates/registerUser', {
            templateUrl: 'viewWrapper.html',
            controller: ViewWrapperController
        })
        .when('/logoff', {
            templateUrl: 'Account/Logoff',
            controller: LogoffController
        });
        
    $httpProvider.interceptors.push('AuthHttpResponseInterceptor');
}

configFunction.$inject = ['$routeProvider', '$httpProvider', '$locationProvider'];

SystemModuleApp.config(configFunction);