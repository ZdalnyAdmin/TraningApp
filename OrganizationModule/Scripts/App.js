window.App = angular.module('OrganizationModuleApp', ['ngRoute',
                                                      'ui.bootstrap',
                                                      'ui.bootstrap.tpls',
                                                      'ngSanitize',
                                                      'froala'])
		            .value('froalaConfig', {
		                inlineMode: false,
		                events: {
		                    align: function (e, editor, alignment) {
		                        console.log(alignment + ' aligned');
		                    }
		                }
		            });