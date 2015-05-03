var UtilitiesFactory = function () {

    var showSpinner = function () {
        $('#spinner').show();
    };

    var hideSpinner = function () {
        $('#spinner').hide();
    };

    return {
        showSpinner: showSpinner,
        hideSpinner: hideSpinner
    }
};

