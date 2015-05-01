window.App
.directive('ngTrainingEditDetails', [function () {
    return {
        scope: { collection: '=', item: '=' },
        restrict: 'A',
        replace: 'true',
        templateUrl: 'Templates/trainingEditDetails.html',
        controller: ['$scope', function ($scope) {
            $scope.editableDetail = {};

            $scope.up = function (item) {
                $scope.collection = changePosition($scope.collection, item, false);
            }

            $scope.down = function (item) {
                $scope.collection = changePosition($scope.collection, item, true);
            }

            $scope.edit = function (item) {
                $scope.editableDetail = angular.copy(item);
                item.isEdit = true;
            }

            $scope.delete = function (item) {
                //todo remove source from server
                for (var i = 0; i < $scope.collection.length; i++) {
                    if ($scope.collection[i] == item) {
                        index = i;
                        break;
                    }
                }
                $scope.collection.splice(index, 1);
            }

            $scope.cancel = function (item) {
                item.Text = $scope.editableDetail.Text;
                item.ExternalResource = $scope.editableDetail.ExternalResource;
                $scope.editableDetail = {};
                item.isEdit = false;
            }

            $scope.save = function (item) {
                $scope.editableDetail = {};
                item.isEdit = false;
            }

            changePosition = function (list, currentItem, up) {
                if (!list || !list.length) {
                    return;
                }
                var index = 0;
                for (var i = 0; i < list.length; i++) {
                    if (list[i] == currentItem) {
                        index = i;
                        break;
                    }
                }
                //delete element from list
                list.splice(index, 1);
                if (up) {
                    index++;
                }
                else {
                    index--;
                }

                var temp = [];
                //add on first position
                if (index <= 0) {
                    temp.push(currentItem);
                    angular.forEach(list, function (item) {
                        temp.push(item);
                    });
                    return temp;
                }
                //add for last position
                if (index >= list.length) {

                    angular.forEach(list, function (item) {
                        temp.push(item);
                    });
                    temp.push(currentItem);
                    return temp;
                }
                //change position
                for (var i = 0; i < list.length; i++) {
                    if (i == index) {
                        temp.push(currentItem);
                    }
                    temp.push(list[i]);
                }
                return temp;
            }
        }]
    };
}]);