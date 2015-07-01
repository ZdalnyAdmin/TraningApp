var editExternalTrainingsModalController = function ($scope, $http, $modalInstance, UtilitiesFactory, selectedGroups, groups) {

    //temp solution
    $scope.Groups = groups;
    $scope.SelectedGroups = selectedGroups;


    angular.forEach($scope.Groups, function (x) {
        x.selected = false;
    });

    angular.forEach($scope.Groups, function (x) {
        angular.forEach($scope.SelectedGroups, function (y) {
            if (x.ProfileGroupID == y.Id) {
                x.selected = true;
            }
        })
    });

    $scope.close = function () {
        $modalInstance.close();
    };

    $scope.selectAll = function (boo) {

        var isForAll = false;
        angular.forEach($scope.Groups, function (x) {
            if (x.Name == 'Wszyscy') {
                x.selected = false;
            }
        });


        if (isForAll) {
            angular.forEach($scope.Groups, function (x) {
                if (x.Name !== 'Wszyscy') {
                    isForAll = true;
                }
            });
        }
    }

    $scope.save = function () {

        var temp = [];

        angular.forEach($scope.Groups, function (val) {
            if (val.selected) {
                temp.push(val);
            }
        });

        $modalInstance.close(temp);
    };
};

editExternalTrainingsModalController.$inject = ['$scope', '$http', '$modalInstance', 'UtilitiesFactory', 'selectedGroups', 'groups'];