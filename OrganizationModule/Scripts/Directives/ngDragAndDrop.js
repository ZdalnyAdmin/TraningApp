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

            el.addEventListener(
                'drop',
                function (e) {
                    // Stops some browsers from redirecting.
                    if (e.stopPropagation) e.stopPropagation();
                    if (e.preventDefault) e.preventDefault();

                    this.classList.remove('over');

                    if (e.dataTransfer.types.indexOf("Files") === -1)
                        return false;

                    var item = document.getElementById(e.dataTransfer.getData('URL'));
                    var file = e.dataTransfer.files[0];
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
                var uploadURL = "./Upload/UploadImage"; //Upload URL
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
                            $scope.fileSrc = data.Message;
                            $scope.$apply();
                        }
                    }
                });

                status.setAbort(jqXHR);
            }

            function createStatusbar(obj) {

                var row = "odd";
                this.statusbar = $("<div class='statusBar'></div>");
                this.filename = $("<div class='filename'></div>").appendTo(this.statusbar);
                this.size = $("<div class='filesize'></div>").appendTo(this.statusbar);
                this.progressBar = $("<div class='progressBar'><div></div></div>").appendTo(this.statusbar);
                this.abort = $("<div class='abort'>Abort</div>").appendTo(this.statusbar);
                $(obj).html('');
                $(obj).append(this.statusbar);

                this.setFileNameSize = function (name, size) {
                    var sizeStr = "";
                    var sizeKB = size / 1024;
                    if (parseInt(sizeKB) > 1024) {
                        var sizeMB = sizeKB / 1024;
                        sizeStr = sizeMB.toFixed(2) + " MB";
                    }
                    else {
                        sizeStr = sizeKB.toFixed(2) + " KB";
                    }

                    this.filename.html(name);
                    this.size.html(sizeStr);

                    $scope.$apply();
                }
                this.setProgress = function (progress) {
                    var progressBarWidth = progress * this.progressBar.width() / 100;
                    this.progressBar.find('div').animate({ width: progressBarWidth }, 10).html(progress + "% ");
                    if (parseInt(progress) >= 100) {
                        this.abort.hide();
                    }

                    $scope.$apply();
                }
                this.setAbort = function (jqxhr) {
                    var sb = this.statusbar;
                    this.abort.click(function () {
                        jqxhr.abort();
                        sb.hide();
                    });

                    $scope.$apply();
                }
            }
        }]
    }
}]);