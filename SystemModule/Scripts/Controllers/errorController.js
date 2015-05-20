function errorController($scope, $routeParams) {
    $scope.errorMessage = 'Nie ma takiej strony';

    if($routeParams.errorId)
    {
        switch ($routeParams.errorId) {
            case '1':
                $scope.errorMessage = 'Nie posiadasz uprawnień do oglądania tej strony';
                break;
            default:
                $scope.errorMessage = 'Nie ma takiej strony';
                break;
        }
    }
}

errorController.$inject = ['$scope', '$routeParams'];