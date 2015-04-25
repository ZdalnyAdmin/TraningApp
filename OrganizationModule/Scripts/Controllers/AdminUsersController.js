function adminUsersController($scope, $http) {
    $scope.loading = true;
    $scope.addMode = false;
    $scope.showEdit = false;
   

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


    $http.get('/api/Status').success(function (data) {
        $scope.status = data;
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
            person = data;


            $scope.loading = false;
            $scope.showEdit = false;
        }).error(function (data) {
            $scope.error = "An Error has occured while saving person! " + data;
            $scope.loading = false;
            $scope.showEdit = false;
        });
    };

    //Used to save a record after edit 
    $scope.cancel = function (person) {
        if (!person) {
            return;
        }
        $scope.loading = true;
        //


        $http.get('/api/Person/', person.PersonID).success(function (data) {
            person = data;
            $scope.loading = false;
            $scope.showEdit = false;
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
        $scope.person = person;


        $scope.loading = true;
        $http.put('/api/Person/', person).success(function (data) {
            $scope.loading = false;
            $scope.showEdit = false;

            var index = 0;

            for (var i = 0; i < $scope.People.length; i++)
            {
                if($scope.People[i].IsDeleted)
                {
                    index = i;
                    break;
                }
            }
            $scope.People.splice(index, 1);
            //$scope.People. = people;
            $scope.DeletePeople.push( $scope.person);


        }).error(function (data) {
            $scope.error = "An Error has occured while deleting person! " + data;
            $scope.loading = false;
            $scope.showEdit = false;
        });
    };
}