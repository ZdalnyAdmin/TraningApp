var SystemModuleApp = angular.module('SystemModuleApp', ['ngRoute']);
SystemModuleApp.controller('BaseController', BaseController);
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
            templateUrl: 'Main/EditTraning'
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
        

}
configFunction.$inject = ['$routeProvider', '$httpProvider', '$locationProvider'];

SystemModuleApp.config(configFunction);