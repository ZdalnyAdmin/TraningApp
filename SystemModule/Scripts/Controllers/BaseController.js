var BaseController = function ($scope, $location) {
    $scope.models = {
        Title: 'e-learning',
        Login: 'Nasz czlowiek'
    };

    var search = $location.search();
    if (!!search && !!search.page) {
        var page = search.page;
        delete search.page;
        $location.path('/' + page).search(search);
    }
}

BaseController.$inject = ['$scope', '$location'];