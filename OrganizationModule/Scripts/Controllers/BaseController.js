var BaseController = function ($scope, $location) {
    $scope.models = {
        Title: 'e-learning',
        Login: 'nasz czlowiek'
    };

    var search = $location.search();
    if (!!search && !!search.page) {
        $location.path('/' + search.page).search('');
    }
}

BaseController.$inject = ['$scope', '$location'];