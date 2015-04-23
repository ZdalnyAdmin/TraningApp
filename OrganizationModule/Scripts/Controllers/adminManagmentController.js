function adminManagmentController($scope, $http) {
    $scope.loading = true;
    $scope.addMode = false;

    $http.get('/api/Traning').success(function (data) {
        $scope.Traning = data;
        $scope.loading = false;
    })
    .error(function () {
        $scope.error = "An Error has occured while loading posts!";
        $scope.loading = false;
    });
}