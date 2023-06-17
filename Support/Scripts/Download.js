var ViewModel = function () {
    var self = this;
    var LearnUri = server + '/api/Data/Learn/';
    idUser = localStorage.getItem("IdUser");
    if (lockNumber == null) {
        window.location.href = localStorage.getItem("urlLogin");
    }

    $("#Index_TextLogo").text('دانلود ها');


    trs = localStorage.getItem("TrsDownload");

    ipw = localStorage.getItem("IPW");

    //$("#P_Learn").hide();
    $("#Karbord-AFI2").hide();
    $("#Karbord-AFI3").hide();
    $("#Karbord-CSH5").hide();
    $("#Karbord-ACC6").hide();
    $("#Karbord-FCT6").hide();
    $("#Karbord-INV6").hide();
    $("#Karbord-PAY6").hide();
    $("#Karbord-ERJ1").hide();

    $("#P_Api").hide();

    if (trs != "null") {
        trs = trs.split('-');
        if (jQuery.inArray("API", trs) !== -1) {
            $("#P_Api").show();
        }

        if (jQuery.inArray("VIDEO", trs) !== -1) { // all
            $("#Karbord-AFI2").show();
            //$("#Karbord-AFI3").show();
            //$("#Karbord-CSH5").show();
            $("#Karbord-ACC6").show();
            $("#Karbord-FCT6").show();
            $("#Karbord-INV6").show();
            $("#Karbord-PAY6").show();
            //$("#Karbord-ERJ1").show();
        }
        //VIDEO_AFI2-VIDEO_AFI3-VIDEO_ACC6-VIDEO_CSH5-VIDEO_FCT6-VIDEO_INV6-VIDEO_PAY6-VIDEO_ERJ1-API-

        if (jQuery.inArray("VIDEO_AFI2", trs) !== -1) $("#Karbord-AFI2").show();
        //if (jQuery.inArray("VIDEO_AFI3", trs) !== -1) $("#Karbord-AFI3").show();
        //if (jQuery.inArray("VIDEO_CSH5", trs) !== -1) $("#Karbord-CSH5").show();
        if (jQuery.inArray("VIDEO_ACC6", trs) !== -1) $("#Karbord-ACC6").show();
        if (jQuery.inArray("VIDEO_FCT6", trs) !== -1) $("#Karbord-FCT6").show();
        if (jQuery.inArray("VIDEO_INV6", trs) !== -1) $("#Karbord-INV6").show();
        if (jQuery.inArray("VIDEO_PAY6", trs) !== -1) $("#Karbord-PAY6").show();
        //if (jQuery.inArray("VIDEO_ERJ1", trs) !== -1) $("#Karbord-ERJ1").show();

    }



    downloadVideoUri = server + '/api/Data/DownloadVideo/' + lockNumber + "/Web/" + ipw + "/";
    tokenUri = server + '/api/Data/Token/';

    var a = document.createElement("a");

    $("#Karbord-AFI2").click(function () {
        a.href = downloadVideoUri + "Karbord-AFI2";
        a.click();
    });

    $("#Karbord-AFI3").click(function () {
        a.href = downloadVideoUri + "Karbord-AFI3";
        a.click();
    });

    $("#Karbord-ACC6").click(function () {
        a.href = downloadVideoUri + "Karbord-ACC6";
        a.click();
    });

    $("#Karbord-CSH5").click(function () {
        a.href = downloadVideoUri + "Karbord-CSH5";
        a.click();
    });

    $("#Karbord-FCT6").click(function () {
        a.href = downloadVideoUri + "Karbord-FCT6";
        a.click();
    });

    $("#Karbord-INV6").click(function () {
        a.href = downloadVideoUri + "Karbord-INV6";
        a.click();
    });

    $("#Karbord-PAY6").click(function () {
        a.href = downloadVideoUri + "Karbord-PAY6";
        a.click();
    });

    $("#Karbord-ERJ1").click(function () {
        a.href = downloadVideoUri + "Karbord-ERJ1";
        a.click();
    });
};


ko.applyBindings(new ViewModel());




