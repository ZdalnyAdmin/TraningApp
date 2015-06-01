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
        controller: ['$scope', '$element', '$http', '$sce', function ($scope, $element, $http, $sce) {
            function guid() {
                function _p8(s) {
                    var p = (Math.random().toString(16) + "000000000").substr(2, 8);
                    return s ? "-" + p.substr(0, 4) + "-" + p.substr(4, 4) : p;
                }
                return _p8() + _p8(true) + _p8(true) + _p8();
            }

            $scope.videoId = guid();
            $scope.errorMessage = '';

            $scope.fileName = undefined;
            $scope.fileSrc = undefined;
            var jqXHR = {};

            $scope.getUrl = function (url) {
                return $sce.trustAsResourceUrl(url);
            };

            $scope.upload = function () {
                $scope.$apply(function (scope) {
                    $scope.errorMessage = '';
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
                            case 'IMAGE':
                                $scope.errorMessage = 'Plik być w formacie JPG, JPEG, PNG, GIF lub BMP!';
                                break;

                            case 'MOVIE':
                                $scope.errorMessage = 'Plik może być w formacie MP4 lub WEBM!';
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

                if ($scope.options == 'MOVIE' && $scope.model.ExternalResource) {
                    var url = new URL($scope.model.ExternalResource);
                    var search = url.search;
                    var path = url.pathname;

                    if ($scope.model.ExternalResource.indexOf('youtube') !== -1) {
                        if (search) {
                            var searchSplit = search.replace('?', '').split('&');
                            for (var i = 0; i < searchSplit.length; i++) {
                                if (searchSplit[i].indexOf('v=') === 0) {
                                    $scope.model.ExternalResource = 'https://www.youtube.com/embed/' + searchSplit[i].replace('v=', '')
                                    break;
                                }
                            }
                        }
                    }

                    if ($scope.model.ExternalResource.indexOf('youtu.be') !== -1) {
                        if (path) {
                            var pathSplit = path.split('/');
                            if (pathSplit.length > 0) {
                                $scope.model.ExternalResource = 'https://www.youtube.com/embed/' + pathSplit[pathSplit.length - 1];
                            }
                        }
                    }

                    if ($scope.model.ExternalResource.indexOf('vimeo') !== -1) {
                        if (path) {
                            var pathSplit = path.split('/');
                            if (pathSplit.length > 0) {
                                $scope.model.ExternalResource = 'https://player.vimeo.com/video/' + pathSplit[pathSplit.length - 1];
                            }
                        }
                    }
                }
            };

            $scope.$watch('model.isEdit', function () {
                if (!$scope.model.isEdit) {
                    $scope.fileName = undefined;
                    $scope.fileSrc = undefined;
                    $scope.file = {};
                    $scope.errorMessage = '';
                    var video = $element.find('#' + $scope.videoId);

                    if ($scope.options == 'MOVIE' && video) {
                        video.html('');
                        video.attr('css', '');
                        video.attr('style', '');
                    }

                    if (jqXHR && jqXHR.abort) {
                        jqXHR.abort();
                    }

                    $element.find('.progressBar').text('');
                    $element.find('.upload-file').parent().html('<input type="file" class="upload-file" onchange="angular.element(this).scope().upload(this)" name="uploadFiles">');
                }
            });

            $element[0].addEventListener('drop',
                function (e) {
                    if (e.stopPropagation) e.stopPropagation();
                    if (e.preventDefault) e.preventDefault();
                });

            var emptyImage = $element[0].getElementsByClassName('empty-image')[0];
            var filledImage = $element[0].getElementsByClassName('filled-image')[0];
            //var filledMovie = $element[0].getElementsByClassName('filled-movie')[0];

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

            //filledMovie.addEventListener(
            //    'dragover',
            //    dragover,
            //    false
            //);

            function dragenter(e) {
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

            //filledMovie.addEventListener(
            //    'dragenter',
            //    dragenter,
            //    false
            //);

            function dragleave(e) {
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

            //filledMovie.addEventListener(
            //    'dragleave',
            //    dragleave,
            //    false
            //);

            function drop(e) {
                // Stops some browsers from redirecting.
                if (e.stopPropagation) e.stopPropagation();
                if (e.preventDefault) e.preventDefault();

                this.classList.remove('over');

                if (e.dataTransfer.types.indexOf("Files") === -1)
                    return false;

                var file = e.dataTransfer.files[0];

                $scope.errorMessage = checkFileSize(file);
                if ($scope.errorMessage) {
                    if ($scope.$root.$$phase != '$apply' && $scope.$root.$$phase != '$digest') {
                        $scope.$apply();
                    }

                    return;
                }

                if (!checkExtension(file)) {

                    switch ($scope.options) {
                        case 'IMAGE':
                            $scope.errorMessage = 'Plik być w formacie JPG, JPEG, PNG, GIF lub BMP!';
                            break;

                        case 'MOVIE':
                            $scope.errorMessage = 'Plik może być w formacie MP4 lub WEBM!';
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

            //filledMovie.addEventListener(
            //    'drop',
            //    drop,
            //    false
            //);

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
                            $scope.errorMessage = 'Plik został pomyślnie wczytany.';

                            if ($scope.options == 'MOVIE') {
                                jwplayer($scope.videoId).setup({
                                    file: $scope.fileSrc
                                });
                            }

                            if ($scope.$root.$$phase != '$apply' && $scope.$root.$$phase != '$digest') {
                                $scope.$apply();
                            }
                        }
                    },
                    error: function () {
                        $scope.errorMessage = 'Plik nie mógł zostać wczytany ­ spróbuj ponownie później.';

                        if ($scope.$root.$$phase != '$apply' && $scope.$root.$$phase != '$digest') {
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
                var availableMovieExtension = ['MP4', 'WEBM'];
                var availableImageExtension = ['JPG', 'JPEG', 'PNG', 'GIF', 'BMP'];
                var fileNameParts = file.name.split('.');
                var extension = fileNameParts[fileNameParts.length - 1];

                switch ($scope.options) {
                    case 'IMAGE':
                        return availableImageExtension.indexOf(extension.toUpperCase()) !== -1;

                    case 'MOVIE':
                        return availableMovieExtension.indexOf(extension.toUpperCase()) !== -1;
                }

                return false;
            }

            function checkFileSize(file) {
                var size = file.size / (1024 * 1024)

                switch ($scope.options) {
                    case 'IMAGE':
                        if (size > 5)
                        {
                            return 'Plik jest zbyt duży ­ wskaż plik o wielkości do 5 MB!';
                        }

                        break;

                    case 'MOVIE':
                        if (size > 500) {
                            return 'Plik jest zbyt duży ­ wskaż plik o wielkości do 500 MB!';
                        }

                        break;
                }

                return '';
            }
        }]
    }
}]);