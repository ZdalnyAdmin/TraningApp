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
        controller: ['$scope', '$element', '$http', function ($scope, $element, $http) {
            var jqXHR = {};

            $scope.fileName = undefined;
            $scope.fileSrc = undefined;

            $scope.upload = function () {
                $scope.$apply(function (scope) {
                    var file = $element[0].getElementsByClassName('upload-file')[0].files[0];

                    if (!checkExtension(file)) {
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

                    if (jqXHR && jqXHR.abort) {
                        jqXHR.abort();
                    }
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
                            $scope.model.InternalResource = $scope.fileSrc = data.Message;
                            if ($scope.$root.$$phase != '$apply' && $scope.$root.$$phase != '$digest') {
                                $scope.$apply();
                            }
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

                    if ($scope.$root.$$phase != '$apply' && $scope.$root.$$phase != '$digest') {
                        $scope.$apply();
                    }
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
                var availablePresentationExtension = ['ppt', 'pptx', 'pdf'];
                var fileNameParts = file.name.split('.');
                var extension = fileNameParts[fileNameParts.length - 1];

                switch ($scope.options) {
                    case 'FILE':
                        return true

                    case 'PRESENTATION':
                        return availablePresentationExtension.indexOf(extension) !== -1;
                }

                return false;
            }
        }]
    }
}]);