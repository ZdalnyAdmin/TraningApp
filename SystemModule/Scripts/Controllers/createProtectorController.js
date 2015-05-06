function createProtectorController($scope, $http, $modal) {
    $scope.loading = true;
    $scope.organizations = [];
    $scope.current = {};

    $scope.loadOrganization = function () {
        $http.get('/api/NotProtectedOrganization')
        .success(function (data) {
            if (!data) {
                return;
            }
            $scope.organizations = data;
            $scope.loading = false;
        })
        .error(function () {
            $scope.error = "An Error has occured while loading posts!";
            $scope.loading = false;
        });
    }

    $scope.loadOrganization();

    $scope.save = function () {
        if (!$scope.current.UserName || !$scope.current.Email || !$scope.current.Organization) {
            return;
        }

        $scope.current.OrganizationID = $scope.current.Organization.OrganizationID;

        $http.post('/api/Protector', $scope.current)
            .success(function (data) {
                $scope.current = {};
                $scope.loading = false;
            })
            .error(function () {
                $scope.error = "An Error has occured while loading posts!";
                $scope.loading = false;
            });

    }

    $scope.changeOrganization = function (selected) {
        if (!selected) {
            $scope.current.Organization = undefined;
            return;
        }
        $scope.current.Organization = selected;
    }
}

createProtectorController.$inject = ['$scope', '$http', '$modal'];