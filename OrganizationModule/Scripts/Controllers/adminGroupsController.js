function adminGroupsController($scope, $http) {
    $scope.loading = true;
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

    $scope.add = function () {
        $scope.loading = true;

        //if ($scope.currentGroup)

        $http.post('/api/Group/', $scope.currentGroup).success(function (data) {
            person = data;
            $scope.loading = false;
            //move to other methods
            $http.get('/api/Group').success(function (data) {
                $scope.Groups = data;
                $scope.loading = false;
            })
           .error(function () {
               $scope.error = "An Error has occured while loading posts!";
               $scope.loading = false;
           });



        }).error(function (data) {
            $scope.error = "An Error has occured while creating group! " + data;
            $scope.loading = false;
        });
    };

    $scope.edit = function (group) {
        if (!group) {
            return;
        }
        $scope.loading = true;

        $http.put('/api/Group/', group).success(function (data) {
            group = data;


            $scope.loading = false;
        }).error(function (data) {
            $scope.error = "An Error has occured while saving group! " + data;
            $scope.loading = false;
        });
    };

    //Used to save a record after edit 
    $scope.cancel = function (group) {
        if (!group) {
            return;
        }
        $scope.loading = true;
        //

        $http.get('/api/Group/', group).success(function (data) {
            person = data;
            $scope.loading = false;
        }).error(function (data) {
            $scope.error = "An Error has occured while restore group data! " + data;
            $scope.loading = false;
        });
    };

    $scope.delete = function (group) {
        if (!group) {
            return;
        }
        $scope.loading = true;

        $http.delete('/api/Group/', group).success(function (data) {
            group = data;
            $scope.loading = false;
        }).error(function (data) {
            $scope.error = "An Error has occured while deleting group! " + data;
            $scope.loading = false;
        });
    };
}