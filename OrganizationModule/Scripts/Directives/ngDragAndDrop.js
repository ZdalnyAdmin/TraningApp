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
        controller: ['$scope', '$element', function ($scope, $element) {
            $scope.fileName = undefined;
            $scope.fileSrc = undefined;

            var el = $element[0].getElementsByTagName('img')[0];
            el.addEventListener(
                'dragover',
                function (e) {
                    e.dataTransfer.dropEffect = 'move';
                    // allows us to drop
                    if (e.preventDefault) e.preventDefault();
                    this.classList.add('over');
                    return false;
                },
                false
            );

            el.addEventListener(
                'dragenter',
                function (e) {
                    this.classList.add('over');
                    return false;
                },
                false
            );

            el.addEventListener(
                'dragleave',
                function (e) {
                    this.classList.remove('over');
                    return false;
                },
                false
            );

            $element[0].addEventListener('drop',
                function (e) {
                    if (e.stopPropagation) e.stopPropagation();
                    if (e.preventDefault) e.preventDefault();
                });

            el.addEventListener(
                'drop',
                function (e) {
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
 
                    $scope.fileName = file.name;
                    var fd = new FormData();
                    fd.append('file', file);
                    sendFileToServer(fd, new createStatusbar($element[0].getElementsByClassName('statusBar')));

                    $scope.$apply();
                    return false;
                },
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
                            $scope.model.InternalResource = $scope.fileSrc = data.Message;
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