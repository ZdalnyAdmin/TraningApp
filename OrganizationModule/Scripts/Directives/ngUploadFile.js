window.App
.directive('ngUploadFile', [function () {
    return {
        scope: {
            options: '=',
            model: '='
        },
        restrict: 'A',
        replace: 'true',
        templateUrl: 'Templates/uploadFile.html',
        controller: ['$scope', '$element', '$http', '$sce', function ($scope, $element, $http, $sce) {
            var jqXHR = {};

            $scope.getUrl = function (url) {
                if (url) {
                    url = location.origin + url;
                    return $sce.trustAsResourceUrl('http://docs.google.com/gview?url=' + url + '&embedded=true');
                }
            };

            $scope.fileName = undefined;
            $scope.fileSrc = undefined;
            $scope.errorMessage = '';

            $scope.upload = function () {
                $scope.$apply(function (scope) {
                    var file = $element[0].getElementsByClassName('upload-file')[0].files[0];

                    $scope.errorMessage = checkFileSize(file);
                    if ($scope.errorMessage) {
                        if ($scope.$root.$$phase != '$apply' && $scope.$root.$$phase != '$digest') {
                            $scope.$apply();
                        }

                        return;
                    }

                    if (!checkExtension(file)) {

                        switch ($scope.options) {
                            case 'FILE':
                                $scope.errorMessage = 'Plik jest w niewłaściwym formacie.';
                                break;

                            case 'PRESENTATION':
                                $scope.errorMessage = 'Prezentacja jest w niewłaściwym formacie ­ wybierz plik PPT, PPTX lub PDF!';
                                break;
                        }

                        if ($scope.$root.$$phase != '$apply' && $scope.$root.$$phase != '$digest') {
                            $scope.$apply();
                        }

                        return;
                    }

                    if ($scope.model.InternalResource) {
                        deleteFile($scope.model.InternalResource);
                    }

                    $scope.fileName = file.name;
                    $scope.model.Name = file.name;
                    var fd = new FormData();
                    fd.append('file', file);
                    sendFileToServer(fd, new createStatusbar($element[0].getElementsByClassName('statusBar')));
                });
            };

            $scope.$watch('model.isEdit', function () {
                if (!$scope.model.isEdit) {
                    $scope.fileName = undefined;
                    $scope.fileSrc = undefined;
                    $scope.file = {};
                    $scope.errorMessage = '';

                    if (jqXHR && jqXHR.abort) {
                        jqXHR.abort();
                    }

                    $element.find('.progressBar').text('');
                    $element.find('.upload-file').parent().html('<input type="file" class="upload-file" onchange="angular.element(this).scope().upload(this)" name="uploadFiles">');
                }
            });

            function sendFileToServer(formData, status) {
                $scope.model.isEdit = true;
                var uploadURL = ""; //Upload URL

                switch ($scope.options) {
                    case 'FILE':
                        uploadURL = "./Upload/UploadFile";
                        break;

                    case 'PRESENTATION':
                        uploadURL = "./Upload/UploadPresentation";
                        break;
                }


                var extraData = {}; //Extra Data.
                jqXHR = $.ajax({
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
                            $scope.errorMessage = 'Plik został pomyślnie wczytany.';
                            $scope.model.InternalResource = $scope.fileSrc = data.Message;
                            if ($scope.$root.$$phase != '$apply' && $scope.$root.$$phase != '$digest') {
                                $scope.$apply();
                            }
                        }

                        status.hide();
                    },
                    error: function () {
                        $scope.errorMessage = 'Plik nie mógł zostać wczytany ­ spróbuj ponownie później.';

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

                switch ($scope.options) {
                    case 'FILE':
                        deleteURL = "./Upload/DeleteFile";
                        break;

                    case 'PRESENTATION':
                        deleteURL = "./Upload/DeletePresentation";
                        break;
                }

                $scope.fileName = undefined;
                $scope.fileSrc = undefined;

                $http.post(
                    deleteURL, {
                        FileName: fileName
                    }
                );
            }

            function checkExtension(file) {
                var availablePresentationExtension = ['PPT', 'PPTX', 'PDF'];
                var unavailableExtensions = ['INK', 'DLL', 'TMP', 'BAT']
                var fileNameParts = file.name.split('.');
                var extension = fileNameParts[fileNameParts.length - 1];

                switch ($scope.options) {
                    case 'FILE':
                        return unavailableExtensions.indexOf(extension.toUpperCase()) === -1;

                    case 'PRESENTATION':
                        return availablePresentationExtension.indexOf(extension.toUpperCase()) !== -1;
                }

                return false;
            }

            function checkFileSize(file) {
                var size = file.size / (1024 * 1024)

                switch ($scope.options) {
                    case 'FILE':
                    case 'PRESENTATION':
                        if (size > 30) {
                            return 'Plik jest zbyt duży ­ jego wielkość nie może przekraczać 30 MB!';
                        }

                        break;
                }

                return '';
            }
        }]
    }
}]);