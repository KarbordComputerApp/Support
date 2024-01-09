var loginLink = true;
var ChatUri = server + '/api/Data/Chat/'; // آدرس لیست چت 
var DateUri = server + '/api/Data/GetDate/';
var UploadChatFileUri = server + '/api/Data/UploadChatFile/'; // آدرس ذخیره لیست پیوست 
var DocAttachChatUri = server + '/api/Data/DocAttachChat/'; // آدرس لیست پیوست 
var AddChatUri = server + '/api/Data/AddChat/'; // 
var DelChatUri = server + '/api/Data/DeleteChat/'; // 
var EndChatUri = server + '/api/KarbordData/EndChat/'; // پایان یافته
var ErjDocXKUri = server + '/api/KarbordData/Web_ErjDocXK/'; // آدرس تیکت ها 

//getDataChat();

var idChat = localStorage.getItem("idChat");

var isLast = false;

idChat = idChat == "0" ? null : idChat;

var isAdminChat = false;
var LockInput = $("#LockInput").data("value");
var userCodeChat = 'User'
var leftItem = "left";
var rightItem = "right";
var mode = 1;
var maxIdMessage = 0;
var timer;
var timerLastIdChat;

$("#chatbox").empty();
$("#motaghaziChat").hide();
$("#chat-bell").hide();
$('#ChatSizeIcon').attr('src', '/Content/img/Icon_Blue/min.png');

$("#chat-bell").show();

$("#box-chat").hide();

if (LockInput != "" && LockInput != null) {
    //panel admin
    idChat = $("#IdChat").data("value");
    idChat = idChat == "0" ? null : idChat;

    userCodeChat = $("#UserCode").data("value");
    lockNumber = LockInput;
    $(".continerHead").hide();
    $("#chat-bell").hide();

    isAdminChat = true;
    rightItem = "left";
    leftItem = "right";
    mode = 0;
    $("#btn-end-chat").show();
    $("#box-chat").show();

    var ErjDocXKObject = {
        SerialNumber: idChat,
        LockNo: lockNumber,
        ModeCode: '204',
        FlagLog: false,
        IP: ipw,
        CallProg: 'Web',
        LoginLink: false
    }
    ajaxFunction(ErjDocXKUri, 'Post', ErjDocXKObject).done(function (data) {
        if (data.length > 0) {
            var motaghazi = data[0].Motaghazi;
            var docDate = data[0].DocDate;
            $("#L_MotaghaziChat").text(motaghazi);
        }
       
    });

    isLast = false;
    refresh(idChat, isLast);
    timer = setInterval(() => { refresh(idChat, false) }, 10000);
    CalcHeight();
}
else {
    //panel user
    $("#chat-bell").show();
    $("#btn-end-chat").hide();
    timerLastIdChat = setInterval(() => {
        getDataChat();
        if (idChat == null) {
            $("#box-send").show();
            $("#chatbox").empty();
        }
    }, 100000);
    isLast = false;
    refresh(idChat, isLast);
}

$("#btn-close-chat").click(function () {
    $("#chat-bell").show();
    $("#box-chat").hide();
    clearInterval(timer);
});



function CalcHeight() {
    all = $("#box-chat").css('height').replace('px', '');
    head = $(".box-header").css('height').replace('px', '');
    footer = $("#box-send").css('height').replace('px', '');

    footer = $("#box-send").css('display') == 'none' ? 0 : footer

    body = parseInt(all) - (parseInt(head) + parseInt(footer));
    $("#chatbox").css('height', body + 2)
    $("#chatbox").scrollTop(1000000);
}


$("#chat-bell").click(function () {
    $("#motaghaziChat").hide();
    $("#motaghaziChat").val("");

    if (idChat == null) {
        getDataChat();
    }

    if (idChat == null) {
        $("#motaghaziChat").show();
        $("#motaghaziChat").removeAttr('disabled');
    }

    idChat = localStorage.getItem("idChat");
    idChat = idChat == "0" ? null : idChat;
    $("#chat-bell").hide();
    $("#box-chat").show();

    if (isLast == true) {
        $("#chatbox").empty();
        isLast = false;
        maxIdMessage = 0;
    }

    refresh(idChat, isLast);
    timer = setInterval(() => { refresh(idChat, false) }, 5000);
    CalcHeight();
});

$("#btn-max-chat").click(function () {
    if ($("#box-chat").css("width") == "380px") {
        $("#box-chat").css("width", "-webkit-fill-available");
        $("#box-chat").css("height", "95vh");
        $('#ChatSizeIcon').attr('src', '/Content/img/Icon_Blue/max.png');
    } else {
        $("#box-chat").css("width", "380px");
        $("#box-chat").css("height", "70vh");
        $('#ChatSizeIcon').attr('src', '/Content/img/Icon_Blue/min.png');
    }
    CalcHeight();
});


//Get DocAttach List

//localStorage.setItem("idChat",22);


function refresh(id, isLast) {
    idChat = id
    idChat = idChat == "0" ? null : idChat;

    if (isLast == true) {
        $("#box-send").hide();
    }

    if (isAdminChat == true && id == null) {
        $("#box-send").hide();
        $("#btn-end-chat").hide();
    }
    //$("#box-send").hide();
    //$("#chatbox").empty();
    //maxIdMessage = localStorage.getItem("MaxIdMessage");
    //maxIdMessage = maxIdMessage == "null" || maxIdMessage == null ? 0 : maxIdMessage;

    if (idChat != null) {
        var ChatObject = {
            LockNumber: lockNumber,
            SerialNumber: idChat,
            IdMessage: maxIdMessage,
        }
        res = '';
        ajaxFunction(ChatUri, 'POST', ChatObject).done(function (data) {
            $("#box-send").show();
            if (data.length > 0) {

                maxIdMessage = data[data.length - 1].Id;
                //localStorage.setItem("MaxIdMessage", maxIdMessage);

                //data.filter(key => key.id == 1);
                endChat = data.filter(key => key.Status == 1);

                if (endChat.length > 0) {
                    idChat = null;
                    localStorage.removeItem("idChat");
                    $("#box-send").hide();
                    $("#btn-end-chat").hide();
                    clearInterval(timer);
                }


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
                        '<div class="dc-msg slideInleft ' + (item.Mode == 0 ? leftItem : rightItem) + '"> ';

                    if ((item.Mode == 1 && leftItem == 'left') || (item.Mode == 0 && leftItem == 'right')) {
                        res += '<img class="deleteChatImg" value="' + item.Id + '" src="/Content/img/Icon_blue/delete.png" width="25" style="padding-top: 10px;padding-right: 5px;display: none;" >';
                    }

                    res += '<div class="dc-text" rel="tooltip" data-container="body">';

                    if (item.Body.search("!!AttachFile!!") >= 0) {
                        fileName = item.Body.split(',')[1]
                        res += '<a class="ChatDownloadFile" idBand="' + item.Id + '"  onclick="ChatDownloadFile(this)">';
                        res +=
                            '<img src="/Content/img/Icon_Blue/Download.png" width="28" style="margin-left:10px">' +
                            '<span>' + fileName + '</span>';
                        res += '</a>'
                    }
                    else {
                        res += item.Body
                    }

                    res +=
                        '</div>';

                    if (item.Mode == 0) {
                        dateText = item.UserCode
                        res += '<div class="timeago_' + (item.Mode == 0 ? leftItem : rightItem) + ' slideIn' + (item.Mode == 0 ? leftItem : rightItem) + '">' + dateText + '</div>';
                    }
                    res += '</div>';
                }

                $("#chatbox").append(res);


                $("#chatbox").scrollTop(1000000);

            }
        });
    }


}

/*
$('.slideInleft.right').hover(
    function () {
        a = $(this).find('.deleteChatImg');
        if (a.length > 0)
            a.show();
    },

    function () {
        a = $(this).find('.deleteChatImg');
        if (a.length > 0)
            a.hide();
    }
);*/
/*

$("#chatbox").on("mouseenter", ".slideInleft.right", function () {
    $(this).find('.deleteChatImg').show();
});

$("#chatbox").on("mouseleave", ".slideInleft.right", function () {
    $(this).find('.deleteChatImg').hide();
});



$("chatbox").on("click", ".slideInleft.right.deleteChatImg", function (event) {
    if (!confirm('Are you sure?')) return false;
    $(this).parent().remove();
    return true;
});



$(".deleteChatImg").click(function (e) {
    var idDel = $(this).attr("value");
    element = this.offsetParent;
    Swal.fire({
        title: 'پیام حذف شود ؟',
        type: 'info',
        showCancelButton: true,
        cancelButtonColor: '#3085d6',
        cancelButtonText: 'خیر',
        confirmButtonColor: '#d33',
        confirmButtonText: 'بله'
    }).then((result) => {
        if (result.value) {
            var DeleteChatObject = {
                Id: idDel,
                IsAttach: 0
            }
            ajaxFunction(DelChatUri, 'POST', DeleteChatObject).done(function (data) {
                element.remove();
            });
        }
    })
})
*/

$("#ChatMessage").keyup(function (e) {
    if (e.keyCode == 13) {
        ChatSend();
    }
})
$("#ChatSend").click(function () {
    ChatSend();
});


function ChatSend() {

    var hasContract = localStorage.getItem("HasContract");

    if (hasContract != "1" && isAdminChat == false) {
        return showNotification('قرارداد شما پایان یافته است و امکان چت را ندارید', 0);
    }

    if (lockNumber == '10000' || lockNumber == '10001' || lockNumber == '10003' || lockNumber == '12035') {

    }
    else {
        return showNotification('دسترسی ندارید', 0);
    }

    var message = $("#ChatMessage").val();
    if (message == "") {
        return showNotification('پیام را وارد کنید', 0);
    }
    if (idChat == null) {

        var motaghazi = $("#motaghaziChat").val();
        if (motaghazi == "") {
            return showNotification('نام درخواست کننده را وارد کنید', 0);
        }

        ajaxFunction(DateUri, 'GET', false).done(function (data) {
            DateNow = data[0];
        });

        var ErjSaveTicketUri = server + '/api/KarbordData/ErjSaveTicket_HI/'; // آدرس  دانلود پیوست 
        var ErjSaveTicket_HI = {
            SerialNumber: 0,
            DocDate: DateNow,
            UserCode: 'ZAND',
            Status: "فعال",
            Spec: "",
            LockNo: lockNumber,
            Text: 'چت : ' + message,
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
            Motaghazi: motaghazi,
            IP: ipw,
            CallProg: 'Web',
            LoginLink: loginLink
        }
        ajaxFunction(ErjSaveTicketUri, 'POST', ErjSaveTicket_HI).done(function (data) {
            idChat = data;
            idChat = idChat == "0" ? null : idChat;
            localStorage.setItem("idChat", idChat);
            $("#motaghaziChat").attr('disabled', 'disabled');
        });
    }


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
        isLast = false;
        refresh(idChat, isLast);
        $("#ChatMessage").val("");
    });
}

$("#ChatAttach").change(function (e) {

    SendAttach();
});

function SendAttach() {
    var file = e.target.files[0];
    var name = file.name;
    var size = file.size;

    var hasContract = localStorage.getItem("HasContract");

    if (hasContract != "1" && isAdminChat == false) {
        return showNotification('قرارداد شما پایان یافته است و امکان چت را ندارید', 0);
    }

    if (lockNumber == '10000' || lockNumber == '10001' || lockNumber == '10003' || lockNumber == '12035') {

    }
    else {
        return showNotification('دسترسی ندارید', 0);
    }

    /*if (lockNumber != '10000' && lockNumber != '10003' && lockNumber != '12035') {
        return showNotification('دسترسی ندارید', 0);
    }*/

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
        isLast = false;
        refresh(idChat, isLast);
    })
}





function ChatDownloadFile(e) {
    idBand = $(e).attr("idBand");

    var DocAttachChatObject = {
        SerialNumber: idChat,
        BandNo: idBand
    }
    ajaxFunction(DocAttachChatUri, 'POST', DocAttachChatObject).done(function (data) {
        var sampleArr = base64ToArrayBuffer(data[0].Atch);
        saveByteArray(data[0].FName, sampleArr);
    });
}



$("#btn-end-chat").click(function () {
    Swal.fire({
        title: 'چت پایان یافته شود ؟',
        type: 'info',
        showCancelButton: true,
        cancelButtonColor: '#3085d6',
        cancelButtonText: 'خیر',
        confirmButtonColor: '#d33',
        confirmButtonText: 'بله'
    }).then((result) => {
        if (result.value) {
            if (idChat != null) {
                var AddChatObject = {
                    LockNumber: lockNumber,
                    SerialNumber: idChat,
                    Mode: mode,
                    Status: 1,
                    ReadSt: 0,
                    UserCode: userCodeChat,
                    Body: "پایان یافته",
                }
                ajaxFunction(AddChatUri, 'POST', AddChatObject).done(function (data) {
                    serialNumber = data;

                    var EndChatObject = {
                        LockNumber: lockNumber,
                        SerialNumber: idChat,
                    }
                    ajaxFunction(EndChatUri, 'POST', EndChatObject).done(function (data) {

                    });

                    isLast = false;
                    refresh(idChat, isLast);
                });
            }

        }
    })

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