var editUserModalController = function ($scope, $http, $modalInstance, UtilitiesFactory, user) {
    $scope.availableStatus = ['Aktywny', 'Zablokowany'];
    $scope.availableProfiles = ['Administrator', 'Manager', 'Twórca', 'Opiekun', 'Uzytkownik'];
    //temp solution
    $scope.Current = user;


    $scope.close = function () {
        $modalInstance.close();
    };

    $scope.save = function () {

        $modalInstance.close($scope.Current);
    };


    $scope.changeStatus = function (person) {
        if (!person) {
            return;
        }
        person.Status = person.selectedStatus == 'Aktywny' ? 1 : 2;

    };

    $scope.changeProfile = function (person) {
        if (!person) {
            return;
        }

        if (person.ProfileName == 'Administrator') {
            person.Profile = 1;
        } else if (person.ProfileName == 'Manager') {
            person.Profile = 2;
        } else if (person.ProfileName == 'Twórca') {
            person.Profile = 3;
        } else if (person.ProfileName == 'Opiekun') {
            person.Profile = 4;
        } else if (person.ProfileName == 'Uzytkownik') {
            person.Profile = 5;
        }
    };
};

editUserModalController.$inject = ['$scope', '$http', '$modalInstance', 'UtilitiesFactory', 'user'];