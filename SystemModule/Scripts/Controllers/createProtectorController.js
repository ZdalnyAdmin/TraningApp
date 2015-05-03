function createProtectorController($scope, $http, $modal) {
    $scope.loading = true;
    $scope.organizations = [];
    $scope.current = {};

    $scope.loadOrganization = function()
    {
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

    $scope.save = function()
    {

    }

    $scope.changeOrganization= function(selected)
    {

    }
}

createProtectorController.$inject = ['$scope', '$http', '$modal'];