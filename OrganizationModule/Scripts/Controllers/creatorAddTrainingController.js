function creatorAddTrainingController($scope, $http, $modal, UserFactory, UtilitiesFactory) {
    $scope.viewModel = {};

    //Used to display the data 
    $scope.loadData = function () {

        UtilitiesFactory.showSpinner();
        $scope.viewModel.ActionType = 5;
        $http.post('/api/Training/')
        .success(function (data) {
            $scope.viewModel = data;
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.viewModel.ErrorMessage = 'Wystąpił nieoczekiwany błąd podczas inicjalizacji danych';
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.loadData();

    $scope.loadImage = function (item) {
        $scope.$apply(function (scope) {
            var file = $element[0].getElementsByClassName('upload-file')[0].files[0];

            if (!checkExtension(file)) {
                return;
            }

            if ($scope.model.InternalResource) {
                deleteFile($scope.model.InternalResource);
            }

            $scope.fileName = file.name;
            var fd = new FormData();
            fd.append('file', file);
            sendFileToServer(fd, new createStatusbar($element[0].getElementsByClassName('statusBar')));
        });
    }

    $scope.loadIcon = function () {
        $scope.$apply(function (scope) {
            var file = $element[0].getElementsByClassName('upload-file')[0].files[0];

            if (!checkExtension(file)) {
                return;
            }

            if ($scope.model.InternalResource) {
                deleteFile($scope.model.InternalResource);
            }

            $scope.fileName = file.name;
            var fd = new FormData();
            fd.append('file', file);
            sendFileToServer(fd, new createStatusbar($element[0].getElementsByClassName('statusBar')));
        });
    }

    $scope.showIcon = function () {
        var modalInstance = $modal.open({
            templateUrl: '/Templates/Modals/commonMarksModal.html',
            controller: 'commonMarksModalController',
            size: 'sm',
            resolve: {
                selectedMark: function () {
                    return $scope.selectedMark;
                }
            }
        });

        modalInstance.result.then(function (selectedMark) {
            if (!!selectedMark) {
                $scope.viewModel.Current.PassResources = selectedMark;
            }
        });
    }

    $scope.save = function () {
        //check conditions
        if (!$scope.viewModel.Current.Name) {
            return;
        }

        UtilitiesFactory.showSpinner();
        $scope.viewModel.ActionType = 3;
        angular.forEach($scope.viewModel.Groups, function (val) {
            if (val.selected) {
                $scope.viewModel.Current.Groups.push(val);
            }
        });


        $http.post('/api/Training/', $scope.viewModel.Current)
        .success(function (data) {
            $scope.viewModel = data;
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.viewModel.ErrorMessage = 'Wystąpił nieoczekiwany błąd podczas zapisu szkolenia';
            UtilitiesFactory.hideSpinner();
        });
    }
}

creatorAddTrainingController.$inject = ['$scope', '$http', '$modal', 'UserFactory', 'UtilitiesFactory'];