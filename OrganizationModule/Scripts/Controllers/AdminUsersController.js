function adminUsersController($scope, $http) {
    $scope.loading = true;
    $scope.addMode = false;
    $scope.showEdit = false;
    $scope.status = ['aktywny', 'zablokowany'];

    //Used to display the data 
    $http.get('/api/Person').success(function (data) {
        var people = [];
        var deletePeople = [];
        angular.forEach(data, function (item) {
            if (item.IsDeleted) {
                deletePeople.push(item);
            }
            else {
                people.push(item);
            }
        });
        $scope.People = people;
        $scope.DeletePeople = deletePeople;
        $scope.loading = false;
    })
    .error(function () {
        $scope.error = "An Error has occured while loading posts!";
        $scope.loading = false;
    });


    //Used to save a record after edit 
    $scope.save = function (person) {
        if (!person) {
            return;
        }
        $scope.loading = true;


        $http.put('/api/Person/', person).success(function (data) {
            emp.editMode = false;
            person = data;


            $scope.loading = false;
            $scope.showEdit = true;
        }).error(function (data) {
            $scope.error = "An Error has occured while saving person! " + data;
            $scope.loading = false;
            $scope.showEdit = false;
        });
    };

    //Used to save a record after edit 
    $scope.cancel = function person() {
        if (!person) {
            return;
        }
        $scope.loading = true;
        //


        $http.get('/api/Person/', person.PersonID).success(function (data) {
            emp.editMode = false;
            person = data;
            $scope.loading = false;
            $scope.showEdit = true;
        }).error(function (data) {
            $scope.error = "An Error has occured while restore person data! " + data;
            $scope.loading = false;
            $scope.showEdit = false;
        });
    };

    $scope.delete = function (person) {
        if (!person) {
            return;
        }
        person.IsDeleted = true;
        //person.DeletedDate = Date.UTC;
        //get from score - logged user id
        //person.DeleteUserID

        $scope.loading = true;
        $http.put('/api/Person/', person).success(function (data) {
            emp.editMode = false;
            $scope.loading = false;
            $scope.showEdit = true;
        }).error(function (data) {
            $scope.error = "An Error has occured while deleting person! " + data;
            $scope.loading = false;
            $scope.showEdit = false;
        });
    };
}