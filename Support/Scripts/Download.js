var ViewModel = function () {
    var self = this;
    var LearnUri = server + '/api/Data/Learn/';

    if (lockNumber == null) {
        window.location.href = localStorage.getItem("urlLogin");
    }

    $("#Index_TextLogo").text('دانلود ها');


    trs = localStorage.getItem("TrsDownload");

    $("#P_Learn").hide();
    $("#P_Api").hide();


    if (trs != "null") {

        if (trs.includes("VIDEO")) {
            $("#P_Learn").show();
        }
        if (trs.includes("API")) {
            $("#P_Api").show();
        }
    }

};


ko.applyBindings(new ViewModel());




