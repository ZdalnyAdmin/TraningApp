function adminManagmentController($scope, $http) {
    $scope.loading = true;
    $scope.addMode = false;

    $http.get('/api/Training').success(function (data) {
        var internalTrainings = [];
        var kenproTrainings = [];
        angular.forEach(data, function (item) {
            if (item.TrainingTypeID == 1) {
                internalTrainings.push(item);
            }
            else {
                kenproTrainings.push(item);
            }
        });
        $scope.InternalTrainings = internalTrainings;
        $scope.KenproTrainings = kenproTrainings;
        $scope.loading = false;
    })
    .error(function () {
        $scope.error = "An Error has occured while loading posts!";
        $scope.loading = false;
    });
}