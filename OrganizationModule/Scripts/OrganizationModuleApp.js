var OrganizationModuleApp = angular.module('OrganizationModuleApp', ['ngRoute']);

OrganizationModuleApp.controller('BaseController', BaseController);
OrganizationModuleApp.controller('LoginController', LoginController);
OrganizationModuleApp.controller('LogoffController', LogoffController);
OrganizationModuleApp.controller('RegisterController', RegisterController);
OrganizationModuleApp.controller('ResetPasswordController', ResetPasswordController);
OrganizationModuleApp.controller('ResetPasswordConfirmation', ResetPasswordConfirmation);
OrganizationModuleApp.factory('AuthHttpResponseInterceptor', AuthHttpResponseInterceptor);
OrganizationModuleApp.factory('LoginFactory', LoginFactory);
//OrganizationModuleApp.service('SessionService', SessionService)

var configFunction = function ($routeProvider, $httpProvider, $locationProvider) {

    $locationProvider.hashPrefix('!').html5Mode(true);

    $routeProvider.
        when('/userCurrent', {
            templateUrl: 'User/LoggedUser'
        })
        .when('/userTrainings', {
            templateUrl: 'User/TrainingList'
        })
        .when('/userAvailableTrainings', {
            templateUrl: 'User/AvailableTrainingList'
        })
        .when('/userResult', {
            templateUrl: 'User/TrainingResult'
        })
        .when('/userFaq', {
            templateUrl: 'User/TrainingFaq'
        })
        .when('/creatorTraining', {
            templateUrl: 'Creator/CreateTemplate'
         })
        .when('/creatorTrainings', {
            templateUrl: 'Creator/TrainingList'
        })
        .when('/creatorHowTo', {
            templateUrl: 'Creator/About'
        })
        .when('/managerResult', {
            templateUrl: 'Manager/Results'
        })
        .when('/managerEdit', {
            templateUrl: 'Manager/EditTrainings'
        })
        .when('/managerInvitation', {
            templateUrl: 'Manager/Invitation'
        })
        .when('/adminUsers', {
            templateUrl: 'Admin/Users'
        })
        .when('/adminGroups', {
            templateUrl: 'Admin/Groups'
        })
        .when('/adminManage', {
            templateUrl: 'Admin/Managment'
        })
        .when('/adminStats', {
            templateUrl: 'Admin/Statistics'
        })
        .when('/adminSettings', {
            templateUrl: 'Admin/Settings'
        })
        .when('/adminHowTo', {
            templateUrl: 'Admin/About'
        })
        .when('/protectorRoles', {
            templateUrl: 'Protector/Roles'
        })
        .when('/protectorLogs', {
            templateUrl: 'Protector/Logs'
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

OrganizationModuleApp.config(configFunction);