function invitationController($scope, $http, $modal, UserFactory, $route, $templateCache, UtilitiesFactory, $location) {
    $scope.user = {
        Role: 5
    };

    var search = $location.search();
    $scope.errorMessage = search ? search.message : '';

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
            var search = {};
            if ($scope.invitation) {
                search = { message: 'Zaproszenie zostało wysłane!' };
            }

            reload(search);
        } else {
            $scope.invitation = false;
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

    function reload(search) {
        var currentPageTemplate = $route.current.templateUrl;
        var locSearch = $location.search();
        $templateCache.remove(currentPageTemplate);

        if (locSearch.message) {
            $route.reload();
        } else {
            $location.path('/managerInvitation').search(search);
        }
    }
}

invitationController.$inject = ['$scope', '$http', '$modal', 'UserFactory', '$route', '$templateCache', 'UtilitiesFactory', '$location'];