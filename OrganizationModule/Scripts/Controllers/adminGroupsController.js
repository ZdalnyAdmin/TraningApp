function adminGroupsController($scope, $http) {
    $scope.loading = true;
    $scope.addMode = false;
    $scope.currentGroup = {};

    //Used to display the data 
    $http.get('/api/Group').success(function (data) {
        $scope.Groups = data;
        $scope.loading = false;
    })
    .error(function () {
        $scope.error = "An Error has occured while loading posts!";
        $scope.loading = false;
    });
}