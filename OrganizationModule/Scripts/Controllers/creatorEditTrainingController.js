function creatorEditTrainingController($scope, $http, $element, $modal, $location, UserFactory, UtilitiesFactory) {
    $scope.viewModel = {};

    var searchObj = $location.search();


    //Used to display the data 
    $scope.loadGroups = function () {
        UtilitiesFactory.showSpinner();
        $scope.viewModel.ActionType = 5;
        $http.post('/api/Training/', $scope.viewModel)
        .success(function (data) {
            $scope.viewModel = data;
            UtilitiesFactory.hideSpinner();
            $scope.loadTraining();
        })
        .error(function () {
            $scope.viewModel.ErrorMessage = 'Wystąpił nieoczekiwany błąd podczas inicjalizacji danych';
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.loadTraining = function () {

        if (!!searchObj && !!searchObj.trainingID) {
            $scope.viewModel.TrainingID = searchObj.trainingID;
        }

        UtilitiesFactory.showSpinner();
        $scope.viewModel.ActionType = 6;
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

    $scope.loadGroups();

    $scope.loadImage = function (item) {
        $scope.$apply(function (scope) {
            var file = $element[0].getElementsByClassName('upload-image')[0].files[0];

            if (!checkExtension(file)) {
                return;
            }

            if ($scope.viewModel.Current.TrainingResources) {
                deleteFile($scope.viewModel.Current.TrainingResources);
            }

            var statusBar = $('.statusBar').eq(0);

            checkImageArtibutesAndUpload(file, 350, 250, 250, false, statusBar);
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

            var statusBar = $('.statusBarMark').eq(0);

            checkImageArtibutesAndUpload(file, 150, 150, 150, true, statusBar);
        });
    }

    function checkImageArtibutesAndUpload(file, maxSize, maxWidth, maxHeight, marks, statusBar) {
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
                if (marks) {
                    sendFileToServer(fd, new createMarkStatusbar(statusBar || $element[0].getElementsByClassName('statusBarMark')), marks);
                } else {
                    sendFileToServer(fd, new createStatusbar(statusBar || $element[0].getElementsByClassName('statusBar')), marks);
                }
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

    function createMarkStatusbar(obj) {

        this.statusbar = $("<div class='statusBarMark'></div>");
        this.progressBar = $("<div class='progressBarMark'><div></div></div>").appendTo(this.statusbar);
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
            return;
        }
        UtilitiesFactory.showSpinner();
        $scope.viewModel.ActionType = 2;
        angular.forEach($scope.viewModel.Groups, function (val) {
            if (val.selected) {
                $scope.viewModel.Current.Groups.push(val);
            }
        });

        $http.post('/api/Training', $scope.viewModel)
        .success(function (data) {
            $scope.viewModel = data;
            UtilitiesFactory.hideSpinner();
        })
        .error(function (data) {
            if (!data) {
                $scope.viewModel.ErrorMessage = 'Wystąpił nieoczekiwany błąd podczas zapisu szkolenia';
            }
            else {
                $scope.viewModel.ErrorMessage = data.Message;
            }
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.remove = function (resources, isMainResources) {
        if (!resources) {
            return;
        }
        if (isMainResources) {
            if (resources.indexOf("Assets\\Image") == -1) {
                deleteFile(resources);
                $scope.viewModel.Current.TrainingResources = "Assets\\Image\\main_image.png";
                resources = "Assets\\Image\\main_image.png";
            }
        }
        else {
            if (resources.indexOf("Assets\\Marks") == -1) {
                deleteFile(resources);
            }
            $scope.viewModel.Current.PassResources = "";
            resources = "";
        }
    }
}

creatorEditTrainingController.$inject = ['$scope', '$http', '$element', '$modal', '$location', 'UserFactory', 'UtilitiesFactory'];