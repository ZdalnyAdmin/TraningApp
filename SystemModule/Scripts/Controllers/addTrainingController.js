function addTrainingController($scope, $http, $element, $modal, UserFactory, UtilitiesFactory) {
    $scope.viewModel = {};

    //Used to display the data 
    $scope.loadData = function () {

        UtilitiesFactory.showSpinner();
        $scope.viewModel.ActionType = 5;
        $http.post('/api/Training/', $scope.viewModel)
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
            var file = $element[0].getElementsByClassName('upload-image')[0].files[0];

            if (!checkExtension(file)) {
                return;
            }

            if ($scope.viewModel.Current.TrainingResources) {
                deleteFile($scope.viewModel.Current.TrainingResources);
            }

            checkImageArtibutesAndUpload(file, 350, 400, 300, false);
        });
    }

    $scope.loadIcon = function (item) {
        $scope.$apply(function (scope) {
            var file = $element[0].getElementsByClassName('upload-mark')[0].files[0];

            if (!checkExtension(file)) {
                return;
            }

            if ($scope.viewModel.Current.PassResources) {
                deleteFile($scope.viewModel.Current.PassResources);
            }

            checkImageArtibutesAndUpload(file, 150, 100, 100, true);
        });
    }

    function checkImageArtibutesAndUpload(file, maxSize, maxWidth, maxHeight, marks)
    {
        $scope.fileName = file.name;
        var size = ~~(file.size / 1024);

        if (size > maxSize) {
            $scope.viewModel.ErrorMessage = 'Nieprawidłowa wielkość obrazka. Musi być mniejsze niz ' + maxSize + 'kb';
            return;
        }

        var img = new Image();

        img.src = window.URL.createObjectURL(file);

        img.onload = function () {
            var width = img.naturalWidth,
                height = img.naturalHeight;

            window.URL.revokeObjectURL(img.src);

            if (width == maxWidth && height == maxHeight) {
                var fd = new FormData();
                fd.append('file', file);
                sendFileToServer(fd, new createStatusbar($element[0].getElementsByClassName('statusBar')), marks);
            }
            else {
                $scope.viewModel.ErrorMessage = 'Nieprawidłowa rozdzielczość obrazka. Musi być ' + maxWidth + 'px na ' + maxHeight + 'px.';
                $scope.fileName = '';
                return;
            }
        };
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

    function checkExtension(file) {
        if (!file) {
            return;
        }
        var fileNameParts = file.name.split('.');
        var extension = fileNameParts[fileNameParts.length - 1];
        return file.type.indexOf('image') !== -1;
    }

    function sendFileToServer(formData, status, marks) {
        var uploadURL = ""; //Upload URL

        uploadURL = "./Upload/UploadImage";

        var extraData = {}; //Extra Data.
        var jqXHR = $.ajax({
            xhr: function () {
                var xhrobj = $.ajaxSettings.xhr();
                if (xhrobj.upload) {
                    xhrobj.upload.addEventListener('progress', function (event) {
                        var percent = 0;
                        var position = event.loaded || event.position;
                        var total = event.total;
                        if (event.lengthComputable) {
                            percent = Math.ceil(position / total * 100);
                        }
                        //Set progress
                        status.setProgress(percent);
                    }, false);
                }
                return xhrobj;
            },
            url: uploadURL,
            type: "POST",
            contentType: false,
            processData: false,
            cache: false,
            data: formData,
            success: function (data) {
                if (data.Succeeded) {

                    if (marks) {
                        $scope.viewModel.Current.PassResources = $scope.fileSrc = data.Message;
                    }
                    else {
                        $scope.viewModel.Current.TrainingResources = $scope.fileSrc = data.Message;
                    }

                    $scope.$apply();
                }
            }
        });
    }

    function createStatusbar(obj) {

        this.statusbar = $("<div class='statusBar'></div>");
        this.progressBar = $("<div class='progressBar'><div></div></div>").appendTo(this.statusbar);
        $(obj).html('');
        $(obj).append(this.statusbar);

        this.setProgress = function (progress) {
            var progressBarWidth = progress * this.progressBar.width() / 100;
            this.progressBar.find('div').animate({ width: progressBarWidth }, 10).html(progress + "% ");

            $scope.$apply();
        }
    }

    function deleteFile(fileName) {
        var deleteURL = ""; //Upload URL
        deleteURL = "./Upload/DeleteImage";

        $scope.fileName = undefined;
        $scope.fileSrc = undefined;

        $http.post(
            deleteURL, {
                FileName: fileName
            }
        );
    }

    $scope.save = function () {
        //check conditions
        if (!$scope.viewModel.Current.Name) {
            $scope.viewModel.Success = "Nalezy podac nazwe";
            return;
        }

        UtilitiesFactory.showSpinner();
        $scope.viewModel.ActionType = 3;
        angular.forEach($scope.viewModel.Groups, function (val) {
            if (val.selected) {
                $scope.viewModel.Current.Groups.push(val);
            }
        });


        $http.post('/api/Training/', $scope.viewModel)
        .success(function (data) {
            $scope.viewModel = data;
            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $scope.viewModel.ErrorMessage = 'Wystąpił nieoczekiwany błąd podczas zapisu szkolenia';
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.showOrganization = function()
    {
        var modalInstance = $modal.open({
            templateUrl: '/Templates/Modals/organizationModal.html',
            controller: 'organizationModalController',
            size: 'sm',
            resolve: {
                selectedOrganization: function () {
                    return $scope.selectedOrganization;
                }
            }
        });

        modalInstance.result.then(function (selectedOrganization) {
            if (!!selectedOrganization) {
                $scope.viewModel.Organizations = selectedOrganization;
                $scope.viewModel.AvailableForAll = selectedOrganization.length == 0;
            }
        });
    }
}

addTrainingController.$inject = ['$scope', '$http', '$element', '$modal', 'UserFactory', 'UtilitiesFactory'];