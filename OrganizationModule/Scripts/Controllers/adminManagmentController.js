function adminManagmentController($scope, $http) {
    $scope.loading = true;
    $scope.addMode = false;

    $http.get('/api/Traning').success(function (data) {
        var internalTranings = [];
        var kenproTranings = [];
        angular.forEach(data, function (item) {
            if (item.TraningTypeID == 1) {
                internalTranings.push(item);
            }
            else {
                kenproTranings.push(item);
            }
        });
        $scope.InternalTranings = internalTranings;
        $scope.KenproTranings = kenproTranings;
        $scope.loading = false;
    })
    .error(function () {
        $scope.error = "An Error has occured while loading posts!";
        $scope.loading = false;
    });
}