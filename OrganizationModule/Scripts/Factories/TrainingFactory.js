var TrainingFactory = function ($http, $q) {


    var trainingLogs = function (trainingID) {

        var deferredObject = $q.defer();

        $http.post(
            '/Admin/Managment', trainingID
        ).
        success(function (data) {
            if (data == "True") {
                deferredObject.resolve({ success: true });
            } else {
                deferredObject.resolve({ success: false });
            }
        }).
        error(function () {
            deferredObject.resolve({ success: false });
        });

       
        return deferredObject.promise;
    };
    return {
        trainingLogs: trainingLogs
    }
};

TrainingFactory.$inject = ['$http', '$q'];