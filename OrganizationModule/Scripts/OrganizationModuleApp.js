var OrganizationModuleApp = angular.module('OrganizationModuleApp', ['ngRoute']);

OrganizationModuleApp.controller('BaseController', BaseController);
OrganizationModuleApp.factory('AuthHttpResponseInterceptor', AuthHttpResponseInterceptor);
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
        .when('/login?returnUrl', {
            templateUrl: 'Account/Login',
            controller: LoginController
        });

   $httpProvider.interceptors.push('AuthHttpResponseInterceptor');
}

configFunction.$inject = ['$routeProvider', '$httpProvider', '$locationProvider'];

OrganizationModuleApp.config(configFunction);