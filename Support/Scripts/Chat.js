var loginLink = true;
var ChatUri = server + '/api/Data/Chat/'; // آدرس لیست چت 
var DateUri = server + '/api/Data/GetDate/';
var UploadChatFileUri = server + '/api/Data/UploadChatFile/'; // آدرس ذخیره لیست پیوست 

//localStorage.removeItem("idChat")
var idChat = localStorage.getItem("idChat");


var isUserChat = false;
var LockInput = $("#LockInput").data("value");
var userCodeChat = 'User'
var leftItem = "left";
var rightItem = "right";
var mode = 1;

if (LockInput != "" && LockInput != null) {
    idChat = $("#IdChat").data("value");
    userCodeChat = $("#UserCode").data("value");
    lockNumber = LockInput;
    $(".continerHead").hide();
    $("#chat-bell").hide();
    isUserChat = true;
    rightItem = "left";
    leftItem = "right";
    mode = 0;
}
else {
    $("#box-chat").hide();
    $("#chat-bell").show();
}

$("#btn-close-chat").click(function () {
    $("#chat-bell").show();
    $("#box-chat").hide();
});

$("#chat-bell-image-wrapper").click(function () {
    $("#chat-bell").hide();
    $("#box-chat").show();
});

$("#btn-max-chat").click(function () {
    if ($("#box-chat").css("width") == "380px") {
        $("#box-chat").css("width", "-webkit-fill-available");
        $("#box-chat").css("height", "auto");
        var h = $("#box-chat").css("height");
        $(".dragandrophandler").css("height", h);
    } else {
        $("#box-chat").css("width", "380px");
        $("#box-chat").css("height", "620px");
        $(".dragandrophandler").css("height", "490px");
    }

});

$("#chatbox").empty();




//Get DocAttach List


function refresh() {
    $("#chatbox").empty();
    var ChatObject = {
        LockNumber: lockNumber,
        SerialNumber: idChat
    }
    res = '';
    ajaxFunction(ChatUri, 'POST', ChatObject).done(function (data) {
        for (var i = 0; i < data.length; i++) {
            item = data[i];
            dateMin = item.DateMin;
            var dateText = "";
            if (dateMin == 0) {
                dateText = "به تازگی";
            } else if (dateMin < 60) {
                dateText = dateMin + " دقیقه پیش";
            }
            else if (dateMin >= 60 && dateMin <= 1440) {
                dateText = Math.round(dateMin / 60) + " ساعت پیش";
            }
            else {
                dateText = Math.round(dateMin / 1440) + " روز پیش";
            }

            res +=
                '<div class="dc-msg slideInleft ' + (item.Mode == 0 ? leftItem : rightItem) + '" data-tmp="48476959" data-pmtype="t"> ' +
                '<div class="dc-text" rel="tooltip" data-container="body">';

            if (item.Body.search("!!AttachFile!!") >= 0) {
                fileName = item.Body.split(',')[1]
                res += '<a class="ChatDownloadFile" idBand="' + item.Id + '">'
                res +=
                    '<img src="/Content/img/Icon_Blue/Download.png" width="28" style="margin-left:10px">' +
                    '<span>' + fileName + '</span>';
                res += '</a>'
            }
            else {
                res += item.Body
            }
            res +=
                '</div>' +
                '<div class="timeago_' + (item.Mode == 0 ? leftItem : rightItem) + ' slideIn' + (item.Mode == 0 ? leftItem : rightItem) + '">' + dateText + '</div>' +
                '</div>';


        }
        $("#chatbox").append(res);
    });
}

if (idChat != null) {
    refresh();
    setInterval(refresh, 6000);
}

$("#ChatSend").click(function () {
    var message = $("#ChatMessage").val();
    if (message == "") {
        return showNotification('پیام را وارد کنید', 0);
    }
    if (idChat == null) {
        ajaxFunction(DateUri, 'GET', false).done(function (data) {
            DateNow = data[0];
        });

        var ErjSaveTicketUri = server + '/api/KarbordData/ErjSaveTicket_HI/'; // آدرس  دانلود پیوست 
        var ErjSaveTicket_HI = {
            SerialNumber: 0,
            DocDate: DateNow,
            UserCode: 'ESTIRI',
            Status: "فعال",
            Spec: "",
            LockNo: lockNumber,
            Text: message,
            F01: '',
            F02: '',
            F03: '',
            F04: '',
            F05: '',
            F06: '',
            F07: '',
            F08: '',
            F09: '',
            F10: '',
            F11: '',
            F12: '',
            F13: '',
            F14: '',
            F15: '',
            F16: '',
            F17: '',
            F18: '',
            F19: '',
            F20: '',
            Motaghazi: "چت",
            IP: ipw,
            CallProg: 'Web',
            LoginLink: loginLink
        }
        ajaxFunction(ErjSaveTicketUri, 'POST', ErjSaveTicket_HI).done(function (data) {
            idChat = data;
            localStorage.setItem("idChat", idChat);
        });
    }



    var AddChatUri = server + '/api/Data/AddChat/'; // 
    var AddChatObject = {
        LockNumber: lockNumber,
        SerialNumber: idChat,
        Mode: mode,
        Status: 0,
        ReadSt: 0,
        UserCode: userCodeChat,
        Body: message,
    }
    ajaxFunction(AddChatUri, 'POST', AddChatObject).done(function (data) {
        serialNumber = data;
        refresh();
        $("#ChatMessage").val("");
    });

});

$("#ChatAttach").change(function (e) {
    var dataFile;
    var file = e.target.files[0];
    var name = file.name;
    var size = file.size;

    if (idChat == null) {
        return showNotification('چت را با پیام متنی شروع کنید', 0);
    }

    if (size > 5000000) { // بیشتر از 5 مگابایت
        return showNotification('فایل پیوست باید کمتر از 5 مگابایت باشد', 0);
    }


    ajaxFunction(DateUri, 'GET', false).done(function (data) {
        DateNow = data[0];
    });

    fileData = name.split(".");
    fileName = fileData[0];


    var AddChatUri = server + '/api/Data/AddChat/'; // 
    var AddChatObject = {
        LockNumber: lockNumber,
        SerialNumber: idChat,
        Mode: mode,
        Status: 0,
        ReadSt: 0,
        UserCode: userCodeChat,
        Body: "!!AttachFile!!," + name,
    }
    ajaxFunction(AddChatUri, 'POST', AddChatObject).done(function (data) {
        serialNumber = data;
    });

    var formData = new FormData();
    formData.append('SerialNumber', idChat);
    formData.append("BandNo", serialNumber);
    formData.append('FName', name);
    formData.append('Atch', file);

    ajaxFunctionUploadTiket(UploadChatFileUri, formData, false).done(function (response) {

    })
});






$(".ChatDownloadFile").click(function () {
    a = $(this).attr("idBand");

});


/*
var DocAttachBoxListObject = {
    Id: item.IId,
    ByData: 1,
    IP: ipw,
    CallProg: 'Web'
}

ajaxFunction(DocAttachBoxListUri, 'POST', DocAttachBoxListObject).done(function (data) {
    var sampleArr = base64ToArrayBuffer(data[0].Atch);
    saveByteArray(data[0].FName, sampleArr);
});
 */