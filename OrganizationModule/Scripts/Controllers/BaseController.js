var BaseController = function ($scope, $location) {
    $scope.models = {
        Title: 'e-learning',
        Login: 'Nasz czlowiek'
    };

    var search = $location.search();
    if (!!search && !!search.page) {
        var page = search.page;
        var controller = search.controller;
        delete search.page;
        delete search.controller;

        if (controller) {
            $location.path('/' + controller + '/' + page).search(search);
        } else {
            $location.path('/' + page).search(search);
        }
    }
}

BaseController.$inject = ['$scope', '$location'];