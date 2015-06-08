var BaseController = function ($scope, $location, UserFactory, $rootScope) {
    $scope.models = {
        Title: 'e-learning'
    };

    $scope.currentUser = undefined;
    $rootScope.$on('userChanged', reload);

    function reload() {
        UserFactory.clearUser();
        $scope.menuUrl = '';
        $scope.currentUser = undefined;
        $scope.visible = false;

        var result = UserFactory.getLoggedUser();

        result.then(function (user) {
            if (user && user.Id) {
                $scope.currentUser = user;
            } else {
                $scope.currentUser = undefined;
            }
        });
    }

    var search = $location.search();
    if (!!search && !!search.page) {
        var page = search.page;
        var controller = search.controller;
        var trainingID = undefined;
        delete search.page;
        delete search.controller;

        if (page === 'ActiveTraining') {
            trainingID = search.trainingID;
            delete search.trainingID;
        }

        if (controller) {
            $location.path('/' + controller + '/' + page).search(search);
        } else if (trainingID) {
            $location.path('/' + page + '/' + trainingID).search(search);
        } else {
            $location.path('/' + page).search(search);
        }
    }

    reload();
}

BaseController.$inject = ['$scope', '$location', 'UserFactory', '$rootScope'];