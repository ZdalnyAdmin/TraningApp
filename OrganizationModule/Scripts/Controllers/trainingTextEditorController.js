/// <reference path="C:\asp\TraningApp\OrganizationModule\Templates/trainingTextEditor.html" />
var trainingTextEditorController = angular.module('OrganizationModuleApp', []);

trainingTextEditorController.directive('trainingTextEditor', function () {
    return {
        scope: true,  // use a child scope that inherits from parent
        restrict: 'AE',
        replace: 'true',
        template: 'Templates/trainingTextEditor.html'
    };
});