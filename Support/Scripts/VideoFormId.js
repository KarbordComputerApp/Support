var ViewModel = function () {
    var self = this;
    $("#chat-bell").hide();
    $("#box-chat").hide();
    var LogVideosUri = server + '/api/Data/LogVideos/';

    $(".LeftButtomMenu").hide();

    var LockInput = $("#LockInput").data("value");
    var Link = $("#Link").data("value");


    if (LockInput != '') {
        loginLink = true;
        getIP();
        localStorage.removeItem("lockNumber");
        lockNumber = LockInput;

        LockNumbersUri = server + '/api/Data/LockNumbers/';

        var LockNumbersObject = {
            LockNumber: lockNumber,
            IP: ipw,
            CallProg: 'Web'
        }
        ajaxFunction(LockNumbersUri, 'POST', LockNumbersObject, false).done(function (dataLock) {
            if (dataLock.length > 0) {
                companyName = dataLock[0].CompanyName.split("-")[0];
            }
        })
    }


    if (lockNumber == null) {
        window.location.href = localStorage.getItem("urlLogin");
    }

    $("#F_Video").attr("src", "");
    $("#Index_TextLogo").text('ویدیو آموزشی');

    $("#P_Aparat").hide();
    $("#videoclip").hide();


    var videoclip = document.getElementById('videoclip');
    var videosource = document.getElementById('mp4video');

    isAparat = false;

    if (Link.search("aparat.com") > 0) {
        isAparat = true;
        $("#P_Aparat").show();
    }
    else {
        $("#videoclip").show();
    }


    var LogVideosObject = {
        LockNumber: lockNumber,
        IP: ipw,
        CallProg: 'Web',
        Spec: Link,
        Act: 12
    }
    ajaxFunction(LogVideosUri, 'POST', LogVideosObject, true).done(function (data) {
        if (isAparat) {
            $("#F_Video").attr("src", Link);
        }
        else {
            videoclip.pause();
            videosource.setAttribute('src', Link);
            videoclip.load();
            videoclip.play();
        }

    });



};


ko.applyBindings(new ViewModel());




