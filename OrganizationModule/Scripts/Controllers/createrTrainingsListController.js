function createrTrainingsListController($scope, $rootScope, $http, $modal, $element, $location, UserFactory, UtilitiesFactory) {
    $scope.viewModel = {};
    $scope.imageMessage = '';

    $scope.loadTrainings = function () {
        UtilitiesFactory.showSpinner();
        $scope.viewModel.ActionType = 9;
        $http.post('/api/Training/', $scope.viewModel)
        .success(function (data) {
            $scope.viewModel = data;

            if ($scope.viewModel && $scope.viewModel.Success) {
                $rootScope.$broadcast('showGlobalMessage', {
                    success: true,
                    messageText: $scope.viewModel.Success
                });
            }

            UtilitiesFactory.hideSpinner();
        })
        .error(function () {
            $rootScope.$broadcast('showGlobalMessage', {
                success: false,
                messageText: 'Wystąpił nieoczekiwany błąd podczas inicjalizacji danych'
            });
            UtilitiesFactory.hideSpinner();
        });
    }

    $scope.loadTrainings();

    $scope.edit = function (item) {
        if (!item) {
            return;
        }

        //call view 
        $location.path('/' + 'creatorTrainings/').search({ trainingID: item.TrainingID });
    }

    $scope.loadImage = function (item, current) {
        if (!current) {
            return;
        }

        $scope.viewModel.Current = current;

        $scope.$apply(function (scope) {
            var file = $element[0].getElementsByClassName('upload-image')[0].files[0];

            if (!checkExtension(file)) {
                $scope.imageMessage = 'Plik może być w formacie JPG, JPEG, PNG, GIF lub BMP.';
                return;
            }

            if ($scope.viewModel.Current.TrainingResources) {
                deleteFile($scope.viewModel.Current.TrainingResources);
            }

            var statusBar = $('.statusBar').eq(0);

            checkImageArtibutesAndUpload(file, 350, 250, 250, false, statusBar);
        });
    }

    function checkImageArtibutesAndUpload(file, maxSize, maxWidth, maxHeight, marks, statusBar) {
        $scope.fileName = file.name;
        var size = ~~(file.size / 1024);

        if (size > maxSize) {
            if (marks) {
                $scope.markMessage = 'Wielkość pliku nie może przekraczać ' + maxSize + 'KB';
            }
            else {
                $scope.imageMessage = 'Wielkość pliku nie może przekraczać ' + maxSize + 'KB';
            }
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
                if (marks) {
                    $scope.markMessage = 'Wczytana grafika musi mieć wymiary ' + maxWidth + 'x ' + maxHeight + 'px';
                }
                else {
                    $scope.imageMessage = 'Wczytana grafika musi mieć wymiary ' + maxWidth + 'x ' + maxHeight + 'px';
                }
                $scope.fileName = '';
                return;
            }
        };
    }

    function checkExtension(file) {
        if (!file) {
            return;
        }

        var availableExtension = ['jpg', 'jpeg', 'png', 'gif', 'bmp'];

        var fileNameParts = file.name.split('.');
        var extension = fileNameParts[fileNameParts.length - 1];
        return availableExtension.indexOf(extension) !== -1;
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
                        $scope.markMessage = 'Plik został pomyślnie wczytany.';
                        $scope.viewModel.Current.PassResources = $scope.fileSrc = data.Message;
                    }
                    else {
                        $scope.imageMessage = 'Plik został pomyślnie wczytany.';
                        $scope.viewModel.Current.TrainingResources = $scope.fileSrc = data.Message;
                    }

                    $scope.$apply();

                    UtilitiesFactory.showSpinner();
                    $scope.viewModel.ActionType = 10;
                    $http.post('/api/Training/', $scope.viewModel)
                    .success(function (data) {
                        $scope.viewModel = data;

                        if ($scope.viewModel && $scope.viewModel.Success) {
                            $rootScope.$broadcast('showGlobalMessage', {
                                success: true,
                                messageText: $scope.viewModel.Success
                            });
                        }

                        UtilitiesFactory.hideSpinner();
                    })
                    .error(function () {
                        $rootScope.$broadcast('showGlobalMessage', {
                            success: false,
                            messageText: 'Wystąpił nieoczekiwany błąd podczas inicjalizacji danych'
                        });
                        UtilitiesFactory.hideSpinner();
                    });

                }
                else {

                    if (marks) {
                        $scope.markMessage = 'Plik nie mógł zostać wczytany - spróbuj ponownie później.';
                    }
                    else {
                        $scope.imageMessage = 'Plik nie mógł zostać wczytany - spróbuj ponownie później.';
                    }
                }

                status.hide();
            },
            error: function () {
                $scope.imageMessage = 'Plik nie mógł zostać wczytany ­ spróbuj ponownie później.';

                if ($scope.$root.$$phase != '$apply' && $scope.$root.$$phase != '$digest') {
                    $scope.$apply();
                }

                status.hide();
            }
        });
    }

    function createStatusbar(obj) {

        $(obj).html('');
        $(obj).show();

        this.progressBar = $("<div class='progressBar'><div></div></div>");

        $(obj).append(this.progressBar);

        this.setProgress = function (progress) {
            var progressBarWidth = progress * this.progressBar.width() / 100;
            this.progressBar.find('div').animate({ width: progressBarWidth }, 10).html(progress + "% ");

            if ($scope.$root.$$phase != '$apply' && $scope.$root.$$phase != '$digest') {
                $scope.$apply();
            }
        }

        this.hide = function () {
            $(obj).hide();
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
}

createrTrainingsListController.$inject = ['$scope', '$rootScope', '$http', '$modal', '$element', '$location', 'UserFactory', 'UtilitiesFactory'];