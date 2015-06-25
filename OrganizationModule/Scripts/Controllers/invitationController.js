function invitationController($scope, $http, $modal, UserFactory, $route, $templateCache, UtilitiesFactory) {
    $scope.user = {
        Role: 5
    };

    $scope.errorMessage = '';

    $scope.invite = function () {
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
            reload();
        } else {
            UtilitiesFactory.hideSpinner();

            if (data.Errors) {
                $scope.errorMessage = '';
                angular.forEach(data.Errors, function (val) {
                    $scope.errorMessage += ' ' + val;
                });
            } else {
                $scope.errorMessage = 'Wystąpił nieoczekiwany błąd!';
            }
        }
    }

    function reload() {
        var currentPageTemplate = $route.current.templateUrl;
        $templateCache.remove(currentPageTemplate);
        $route.reload();
    }
}

invitationController.$inject = ['$scope', '$http', '$modal', 'UserFactory', '$route', '$templateCache', 'UtilitiesFactory'];