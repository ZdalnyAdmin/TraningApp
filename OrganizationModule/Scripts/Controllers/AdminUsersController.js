function adminUsersController($scope, $http) {
    $scope.loading = true;
    $scope.addMode = false;

    $scope.status = ['aktywny','zablokowany'];

    //Used to display the data 
    $http.get('/api/Person').success(function (data) {
        $scope.Persons = data;
        $scope.loading = false;
    })
    .error(function () {
        $scope.error = "An Error has occured while loading posts!";
        $scope.loading = false;
    });
}