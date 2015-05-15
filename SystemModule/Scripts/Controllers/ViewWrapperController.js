var ViewWrapperController = function ($scope, $location) {
    var search = $location.search();
    var url = $location.url();
    var absoluteUri = $location.absUrl();
    var path = $location.path();
    var host = absoluteUri.replace(url, '');

    path = path.split('/');
    $scope.viewUrl = host + '/' + path[path.length - 1] + '?';

    angular.forEach(search, function(value, key) {
        $scope.viewUrl += key + '=' + encodeURIComponent(value) + '&';
    });
}

ViewWrapperController.$inject = ['$scope', '$location'];