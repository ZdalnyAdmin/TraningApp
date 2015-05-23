window.App
.directive('ngDragAndDrop', [function () {
    return {
        scope: {
            options: '=',
            model: '='
        },
        restrict: 'A',
        replace: 'true',
        templateUrl: 'Templates/dragAndDrop.html',
        controller: ['$scope', '$element', '$http', function ($scope, $element,$http) {
            $scope.fileName = undefined;
            $scope.fileSrc = undefined;
            var jqXHR = {};

            $scope.upload = function () {
                $scope.$apply(function(scope) {
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
            };

            $scope.change = function () {
                $scope.model.isEdit = true;

                if ($scope.model.InternalResource) {
                    deleteFile($scope.model.InternalResource);
                    $scope.model.InternalResource = undefined;
                }
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

            $element[0].addEventListener('drop',
                function (e) {
                    if (e.stopPropagation) e.stopPropagation();
                    if (e.preventDefault) e.preventDefault();
                });

            var emptyImage = $element[0].getElementsByClassName('empty-image')[0];
            var filledImage = $element[0].getElementsByClassName('filled-image')[0];
            var filledMovie = $element[0].getElementsByClassName('filled-movie')[0];

            function dragover(e) {
                e.dataTransfer.dropEffect = 'move';
                // allows us to drop
                if (e.preventDefault) e.preventDefault();
                this.classList.add('over');
                return false;
            }

            emptyImage.addEventListener(
                'dragover',
                dragover,
                false
            );

            filledImage.addEventListener(
                'dragover',
                dragover,
                false
            );

            filledMovie.addEventListener(
                'dragover',
                dragover,
                false
            );

            function dragenter (e) {
                this.classList.add('over');
                return false;
            }

            emptyImage.addEventListener(
                'dragenter',
                dragenter,
                false
            );

            filledImage.addEventListener(
                'dragenter',
                dragenter,
                false
            );

            filledMovie.addEventListener(
                'dragenter',
                dragenter,
                false
            );

            function dragleave (e) {
                this.classList.remove('over');
                return false;
            }

            emptyImage.addEventListener(
                'dragleave',
                dragleave,
                false
            );

            filledImage.addEventListener(
                'dragleave',
                dragleave,
                false
            );

            filledMovie.addEventListener(
                'dragleave',
                dragleave,
                false
            );

            function drop (e) {
                // Stops some browsers from redirecting.
                if (e.stopPropagation) e.stopPropagation();
                if (e.preventDefault) e.preventDefault();

                this.classList.remove('over');

                if (e.dataTransfer.types.indexOf("Files") === -1)
                    return false;

                var file = e.dataTransfer.files[0];

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
                $scope.model.isEdit = true;

                if ($scope.$root.$$phase != '$apply' && $scope.$root.$$phase != '$digest') {
                    $scope.$apply();
                }

                return false;
            }

            emptyImage.addEventListener(
                'drop',
                drop,
                false
            );

            filledImage.addEventListener(
                'drop',
                drop,
                false
            );

            filledMovie.addEventListener(
                'drop',
                drop,
                false
            );

            function sendFileToServer(formData, status) {
                var uploadURL = ""; //Upload URL

                switch ($scope.options) {
                    case 'IMAGE':
                        uploadURL = "./Upload/UploadImage";
                        break;

                    case 'MOVIE':
                        uploadURL = "./Upload/UploadMovie";
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
                    case 'IMAGE':
                        deleteURL = "./Upload/DeleteImage";
                        break;

                    case 'MOVIE':
                        deleteURL = "./Upload/DeleteMovie";
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
                var availableMovieExtension = ['avi', 'mkv', 'mpeg', 'mp4', ];
                var fileNameParts = file.name.split('.');
                var extension = fileNameParts[fileNameParts.length - 1];

                switch ($scope.options) {
                    case 'IMAGE':
                        return file.type.indexOf('image') !== -1;

                    case 'MOVIE':
                        return availableMovieExtension.indexOf(extension) !== -1;
                }

                return false;
            }
        }]
    }
}]);