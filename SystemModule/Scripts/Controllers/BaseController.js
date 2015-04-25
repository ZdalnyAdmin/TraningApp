var BaseController = function ($scope, $location) {
    $scope.models = {
        Title: 'e-learning',
        Login: 'Nasz czlowiek'
    };

    var search = $location.search();
    if (!!search && !!search.page) {
        $location.path('/' + search.page).search('');
    }
}

BaseController.$inject = ['$scope', '$location'];