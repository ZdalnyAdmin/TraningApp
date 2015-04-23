function adminUsersController($scope, $http) {
    $scope.loading = true;
    $scope.addMode = false;

    $scope.status = ['aktywny', 'zablokowany'];

    //Used to display the data 
    $http.get('/api/Person').success(function (data) {
        var persons = [];
        var deletePersons = [];
        angular.forEach(data, function (item) {
            if (item.IsDeleted) {
                deletePersons.push(item);
            }
            else {
                persons.push(item);
            }
        });
        $scope.Persons = persons;
        $scope.DeletePersons = deletePersons;
        $scope.loading = false;
    })
    .error(function () {
        $scope.error = "An Error has occured while loading posts!";
        $scope.loading = false;
    });
}