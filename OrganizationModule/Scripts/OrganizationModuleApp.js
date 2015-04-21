var OrganizationModuleApp = angular.module('OrganizationModuleApp', ['ngRoute']);

OrganizationModuleApp.controller('BaseController', BaseController);
//OrganizationModuleApp.service('SessionService', SessionService)

var configFunction = function ($routeProvider, $httpProvider, $locationProvider) {

    $locationProvider.hashPrefix('!').html5Mode(true);

    $routeProvider.
        when('/userCurrent', {
            templateUrl: 'User/LoggedUser'
        })
        .when('/userTranings', {
            templateUrl: 'User/TreningList'
        })
        .when('/userAvailableTranings', {
            templateUrl: 'User/AvailableTraningList'
        })
        .when('/userResult', {
            templateUrl: 'User/TraningResult'
        })
        .when('/userFaq', {
            templateUrl: 'User/TraningFaq'
        })
        .when('/creatorTraning', {
            templateUrl: 'Creator/CreateTemplate'
         })
        .when('/creatorTranings', {
            templateUrl: 'Creator/TraningList'
        })
        .when('/creatorHowTo', {
            templateUrl: 'Creator/About'
        })
        .when('/managerResult', {
            templateUrl: 'Manager/Results'
        })
        .when('/managerEdit', {
            templateUrl: 'Manager/EditTranings'
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

   // $httpProvider.interceptors.push('AuthHttpResponseInterceptor');

}
configFunction.$inject = ['$routeProvider', '$httpProvider', '$locationProvider'];

OrganizationModuleApp.config(configFunction);