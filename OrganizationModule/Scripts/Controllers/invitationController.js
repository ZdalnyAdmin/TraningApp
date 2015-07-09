function invitationController($scope, $rootScope, $http, $modal, UserFactory, $route, $templateCache, UtilitiesFactory, $location) {
    $scope.user = {
        Role: 5
    };

    var search = $location.search();

    if (search && search.message) {
        $rootScope.$broadcast('showGlobalMessage', {
            success: true,
            messageText: search.message
        });
    }

    $scope.errorMessage = '';

    $scope.invite = function () {
        $scope.invitation = true;
        UtilitiesFactory.showSpinner();
        var result = UserFactory.inviteUser($scope.user);
        result.then(processResponse);
    };

    function insert(str, index, value) {
        return str.substr(0, index) + value + str.substr(index);
    }

    $scope.deleteUser = function (id) {
        id = insert(id, 8, '-');
        id = insert(id, 13, '-');
        id = insert(id, 18, '-');
        id = insert(id, 23, '-');

        UtilitiesFactory.showSpinner();
        var result = UserFactory.removeInvitation({ Id: id });
        result.then(processResponse);
    };
    
    $scope.toggle = function (rowId) {
        $scope[rowId] = !$scope[rowId];
    };

    function processResponse (data){
        if (data.Succeeded) {
            if ($scope.invitation) {
                $rootScope.$broadcast('showGlobalMessage', {
                    success: true,
                    messageText: 'Zaproszenie zostało wysłane!'
                });
            }

            reload();
        } else {
            $scope.invitation = false;
            UtilitiesFactory.hideSpinner();

            if (data.Errors) {
                var errorMessage = '';
                angular.forEach(data.Errors, function (val) {
                    errorMessage += ' ' + val;
                });

                $rootScope.$broadcast('showGlobalMessage', {
                    success: false,
                    messageText: errorMessage
                });
            } else {
                $rootScope.$broadcast('showGlobalMessage', {
                    success: false,
                    messageText: 'Wystąpił nieoczekiwany błąd!'
                });
            }
        }
    }

    function reload(search) {
        var currentPageTemplate = $route.current.templateUrl;
        var locSearch = $location.search();
        $templateCache.remove(currentPageTemplate);

        $route.reload();
    }
}

invitationController.$inject = ['$scope', '$rootScope', '$http', '$modal', 'UserFactory', '$route', '$templateCache', 'UtilitiesFactory', '$location'];