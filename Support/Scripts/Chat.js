var loginLink = true;
var ChatUri = server + '/api/Data/Chat/'; // آدرس لیست چت 
var DateUri = server + '/api/Data/GetDate/';
var UploadChatFileUri = server + '/api/Data/UploadChatFile/'; // آدرس ذخیره لیست پیوست 
var DocAttachChatUri = server + '/api/Data/DocAttachChat/'; // آدرس لیست پیوست 
var AddChatUri = server + '/api/Data/AddChat/'; // 
var DelChatUri = server + '/api/Data/DeleteChat/'; // 
var EndChatUri = server + '/api/KarbordData/EndChat/'; // پایان یافته
var ChatQueueUri = server + '/api/KarbordData/ChatQueue/'; // تعداد افراد در انتظار چت 
var ErjDocXKUri = server + '/api/KarbordData/Web_ErjDocXK/'; // آدرس تیکت ها 
var UpdateChatDownloadUri = server + '/api/KarbordData/UpdateChatDownload/';
var LockNumbersUri = server + '/api/Data/LockNumbers/';
var HasMainTenanceUri = server + '/api/Data/HasMainTenance/';

var activeChatQueue = null;
//getDataChat();

/*
var lockChat = [
    "10000", "10001", "10002", "10003",
    "11764",
    "14137",
    "12984",
    "13591",
    "11342",
    "13124",
    "14317",
    "14213",
    "14224"
];
*/

//localStorage.removeItem("MotaghaziChat");

var focusPage;

var machineIdKarbord_Support = localStorage.getItem("MachineIdKarbord_Support");
var motaghaziChat;
var motaghaziChatTiket = "";
var otherUserChat = null;
//localStorage.getItem("MotaghaziChat");



var orgTitle = document.title;
var animatedTitle = "پیام جدید " + orgTitle;

var idChat = localStorage.getItem("idChat");

var isLast = false;
var timerNotifation = null;
var timerNotifationPage = null;

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
$("#chat-bell").hide();
$('#ChatSizeIcon').attr('src', '/Content/img/Icon_Blue/min.png');

$("#chat-bell").show();

$("#box-chat").hide();



if (LockInput != "" && LockInput != null) {

    idChat = $("#IdChat").data("value");

    if (idChat != null) {  //panel admin
        idChat = idChat == "0" ? null : idChat;

        userCodeChat = $("#UserCode").data("value");
        lockNumber = LockInput;
        $(".continerHead").hide();
        $("#chat-bell").hide();
        $("#ActiveAttach").show();
        $("#P_AttachChat").show();

        isAdminChat = true;
        rightItem = "left";
        leftItem = "right";
        mode = 0;

        $("#box-Eshkalat").hide();

        $("#btn-end-chat").show();
        $("#box-chat").show();

        if (ipw == "" || ipw == "null" || ipw == null) {
            getIP();
        }

        GetRepFromUsers();
        getCompanyName();

        isLast = false;
        refresh(idChat, isLast);
        $("#L_MotaghaziChat").text(localStorage.getItem("CompanyNameChat") + " - " + motaghaziChat);



        var orgTitle = localStorage.getItem("CompanyNameChat");
        var animatedTitle = "پیام جدید از " + orgTitle;

        document.title = orgTitle;


        timer = setInterval(() => { refresh(idChat, false) }, 10000);
        CalcHeight();
    }
    else { //TiketLink

        if (lockNumber == "" || lockNumber == null) {
            lockNumber = LockInput;
        }

        if (lockNumber == "000091") {
            $("#ActiveAttach").hide();
            $("#chat-bell").hide();
            $("#btn-end-chat").hide();
            $("#AddNewErjDocXK").hide();
            $(".LeftButtomMenu").hide();
        }
        else {
            getHasContract();
            UserPanel();
        }
    }
}
else {
    //panel user
    UserPanel();

}

function UserPanel() {

    $("#ActiveAttach").hide();
    $("#chat-bell").show();
    $("#btn-end-chat").hide();

    timerLastIdChat = setInterval(() => {
        getDataChat();
    }, 100000);
    isLast = false;
    refresh(idChat, isLast);
}











if (lockNumber == "" || lockNumber == null) {
    //  window.location.href = localStorage.getItem("urlLogin");
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
    $("#chatbox").scrollTop(100000000);
}

function SetUpdateChatDownload(value) {

    var UpdateChatDownload_Object = {
        SerialNumber: idChat,
        ChatDownload: value
    }
    ajaxFunction(UpdateChatDownloadUri, 'POST', UpdateChatDownload_Object).done(function (data) {
        var a = data;
    });
}


function getDataTiket(id) {
    $("#P_AttachChat").show();
    if (isAdminChat == false)
        $("#P_AttachChat").hide();

    var ErjDocXKObject = {
        SerialNumber: id,
        LockNo: lockNumber,
        ModeCode: '204',
        FlagLog: false,
        IP: ipw,
        CallProg: 'Web',
        LoginLink: false,
        top: null,
        Status: null,
    }
    ajaxFunction(ErjDocXKUri, 'Post', ErjDocXKObject).done(function (data) {
        if (data.length > 0) {
            //localStorage.setItem("MotaghaziChat", data[0].Motaghazi);
            var chatDownload = data[0].ChatDownload;
            $("#ActiveAttachChat").prop('checked', chatDownload);
            if (chatDownload == true) {
                $("#P_AttachChat").show();
            }

            motaghaziChatTiket = data[0].Motaghazi;

            if (isAdminChat) {
                motaghaziChat = motaghaziChatTiket;
            }
            else {
                motaghaziChat = localStorage.getItem("MotaghaziChat");
            }
            otherUserChat = motaghaziChatTiket != motaghaziChat;


        }
    });
}



function CreateCaptcha() {
    let uniquechar = "";
    const randomchar = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    for (let i = 1; i < 6; i++) {
        uniquechar += randomchar.charAt(
            Math.random() * randomchar.length)
    }
    $("#CaptchaData").val(uniquechar);
}


$("#RefreshCaptcha").click(function () {
    $("#CaptchaVal").val("");
    CreateCaptcha();
});

$("#chat-bell").click(function () {

    if (idChat == null) {
        getDataChat();
    }

    idChat = localStorage.getItem("idChat");
    idChat = idChat == "0" ? null : idChat;
    $("#P_NewChatInfo").show();

    if (isAdminChat) {
        $("#box-chat").show();
        $("#chat-bell").hide();
    } else {
        ajaxFunction(HasMainTenanceUri + lockNumber, 'GET').done(function (data) {

            if (data == "") {
                if (idChat == null) {
                    CreateCaptcha();
                    $("#motaghaziChat").val("");
                    $("#CaptchaVal").val("");
                    $("#modal-NewChat").modal("show");
                    $("#chatbox").empty();
                } else if (otherUserChat) {
                    Swal.fire({
                        title: "",
                        text: "در حال حاضر چت با " + motaghaziChatTiket + " در حال انجام است " + "آیا به چت اضافه می شوید ؟",
                        type: 'warning',
                        showCancelButton: true,
                        cancelButtonColor: '#3085d6',
                        cancelButtonText: 'خیر',
                        allowOutsideClick: false,
                        confirmButtonColor: '#d33',
                        confirmButtonText: 'بله'
                    }).then((result) => {
                        if (result.value) {
                            CreateCaptcha();
                            $("#P_NewChatInfo").hide();
                            $("#motaghaziChat").val("");
                            $("#CaptchaVal").val("");
                            $("#modal-NewChat").modal("show");
                        }
                    })
                }
                else {
                    $("#box-chat").show();
                    $("#chat-bell").hide();
                }
            } else {
                return showNotification(data, 0);
            }
        });

    }


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




var show

function refresh(id, isLast) {
    idChat = id
    idChat = idChat == "0" ? null : idChat;
    if (idChat > 0) {
        getDataTiket(idChat);
    }

    if (isLast == true) {
        $("#box-send").hide();
    }

    if (isAdminChat == true && id == null) {
        $("#box-send").hide();
        $("#btn-end-chat").hide();
    }
    $("#box-notification").hide();

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

                if (data.length > 0 && activeChatQueue != false) {
                    dataAvtive = data.filter(key => key.Mode == 0);
                    if (dataAvtive.length > 0) {
                        activeChatQueue = false;
                    } else {
                        activeChatQueue = true;

                        if (data[0].Body == "!!@NewChat@!!" && data.length == 1) {
                            activeChatQueue = null;
                        }
                    }
                }

                data = data.filter(key => key.Body != "!!@NewChat@!!");

                if (endChat.length > 0) {
                    idChat = null;
                    activeChatQueue = null;
                    localStorage.removeItem("idChat");
                    $("#box-send").hide();
                    $("#btn-end-chat").hide();
                    clearInterval(timer);
                    otherUserChat = null;
                }
                if (focusPage == false && isAdminChat) {
                    NotifationChat(data[0].Body);
                }

                for (var i = 0; i < data.length; i++) {
                    item = data[i];
                    timeSend = item.TimeSend;

                    res +=
                        '<div class="dc-msg slideInleft ' + (item.Mode == 0 ? leftItem : rightItem) + '"> ';

                    if ((item.Mode == 1 && leftItem == 'left') || (item.Mode == 0 && leftItem == 'right')) {
                        res += '<img class="deleteChatImg" value="' + item.Id + '" src="/Content/img/Icon_blue/delete.png" width="25" style="padding-top: 10px;padding-right: 5px;display: none;" >';
                    }


                    res += '<div class="' + (item.Status == 1 ? 'dc-endchat' : 'dc-text') + '" rel="tooltip" data-container="body">';


                    if (item.Body.search("!!AttachFile!!") >= 0) {
                        fileName = item.Body.split(',')[1]
                        res += '<a class="ChatDownloadFile" idBand="' + item.Id + '"  onclick="ChatDownloadFile(this)">';
                        res +=
                            '<img src="/Content/img/Icon_Blue/Download.png" width="28" style="margin-left:10px">' +
                            '<span>' + fileName + '</span>';
                        res += '</a>'
                    }
                    else if (item.Status == 1) {
                        res += '<div class="dox-endchat">' +
                            '<p style="text-align: center">این چت به یایان رسید</p>' +
                            '<p style="text-align: center">روز خوبی را برای شما آرزومندیم</p>' +
                            '</div>';
                    }
                    else {

                        isLink = item.Body.search("www.") > 0 || item.Body.search("http://") > 0 || item.Body.search("https://") > 0 || item.Body.search("185.208.174.64") > 0
                        if (isLink) {
                            isVideo = item.Body.toLowerCase();
                            isVideo = isVideo.search("/content/video/") > 0;

                            if (isVideo) {

                                res += '<a href="#" name = "' + item.Body + '" onclick="ShowVideoChat(this)">';
                                res += '<span>ویدیو آموزشی</span>';
                                res += '</a>'
                            } else {
                                res += '<a href="' + item.Body + '" target="_blank" >';
                                res += '<span>' + item.Body + '</span>';
                                res += '</a>'
                            }
                        }

                        else {
                            res += '<span>' + item.Body + '</span>';
                        }



                    }

                    res +=
                        '</div>';

                    if (isAdminChat) {
                        dateText = item.Mode == 1 ? item.UserCode : SetNameUser(item.UserCode);

                        res += '<div class="timeago_' + (item.Mode == 0 ? leftItem : rightItem) + ' slideIn' + (item.Mode == 0 ? leftItem : rightItem) + '">' + dateText + '<span style="margin-right:5px; margin-left:5px">' + timeSend + '</span>' + '</div>';
                    }
                    else {
                        res += '<div class="timeago_' + (item.Mode == 0 ? leftItem : rightItem) + ' slideIn' + (item.Mode == 0 ? leftItem : rightItem) + '">' + '<span style="margin-left: 5px">' + timeSend + '</span>' + (item.Mode == 1 ? item.UserCode : "") + '</div>';

                    }
                    res += '</div>';
                }

                $("#chatbox").append(res);


                $("#chatbox").scrollTop(100000000);

            }
        });

        if (isAdminChat == false && activeChatQueue == true) {
            ajaxFunction(ChatQueueUri + '/' + idChat, 'GET', false).done(function (data) {
                if (data > 0) {
                    $("#box-notification").show();
                    $("#l-notification").text("شما نفر " + (data + 1) + " در صف انتظار هستید. لطفا منتظر بمانید...")
                }
            });
        }

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

$("#Eshkalat_End").hide();


$("#ChatMessage").keyup(function (e) {
    if (e.keyCode == 13) {
        ChatSend(false);
    }
})
$("#ChatSend").click(function () {
    ChatSend(false);
});

$("#Eshkalat_Other").click(function () {
    $("#box-Eshkalat").hide();
    $("#ChatMessage").focus();
    CalcHeight();
});

$("#Eshkalat_Tekrari").click(function () {
    caption = $(this.lastElementChild).text();
    link = "http://185.208.174.64:8001/Content/Video/Sending_double_invoices.mp4";
    ShowVideoEshkal(link, caption);
});

$("#Eshkalat_ClientId").click(function () {
    caption = $(this.lastElementChild).text();
    link = "http://185.208.174.64:8001/Content/Video/Upload_Public_key.mp4";
    ShowVideoEshkal(link, caption);
});

$("#Eshkalat_End").click(function () {
    caption = "اشکال برطرف شد";
    ChatSend(false, caption);
    $("#box-Eshkalat").hide();
    $("#Eshkalat_End").hide();
    CalcHeight();
});


function ShowVideoEshkal(link, caption) {
    caption = caption.substr(3, caption.length);
    var LogLinkTiketUri = server + '/api/Data/LogLinkTiket/';
    var LogLinkTiketObject = {
        LockNumber: lockNumber,
        IP: ipw,
        CallProg: 'Web',
        Link: link
    };
    ajaxFunction(LogLinkTiketUri, 'POST', LogLinkTiketObject, false).done(function (data) {
        videoclip.pause();
        videosource.setAttribute('src', link);
        videoclip.load();
        videoclip.play();
        $("#Title_VideoChat").text(caption);
        $("#modal-VideoChat").modal('show');
        $("#Eshkalat_End").show();
        ChatSend(false, "نمایش ویدیو: " + caption);
    });
}



//localStorage.removeItem("HasContract");


function ChatSend(firstSend, mess) {
    ajaxFunction(HasMainTenanceUri + lockNumber, 'GET').done(function (data) {
        if (data == "") {
            var message = firstSend == true ? "!!@NewChat@!!" : mess == null ? $("#ChatMessage").val() : mess;
            if (message.trim() == "") {
                $("#ChatMessage").val("");
                return showNotification('پیام را وارد کنید', 0);
            }

            if (idChat == null) {

            }
            else {

                if (motaghaziChat == "") {
                    motaghaziChat == "UserChat"
                }
                userCodeChat = isAdminChat == true ? userCodeChat : motaghaziChat;
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
        }
        else {
            return showNotification(data, 0);
        }
    });
}

$("#SendNewChat").click(function () {
    NewChat();
})

$("#SendNewChat").keyup(function (e) {
    if (e.keyCode == 13) {
        NewChat();
    }
})

machineIdKarbord_Support = localStorage.getItem("MachineIdKarbord_Support");

function NewChat() {
    var hasContract = localStorage.getItem("HasContract");

    if (hasContract != "1" && isAdminChat == false) {
        return showNotification('قرارداد شما پایان یافته است و امکان چت را ندارید', 0);
    }


    /* if (lockChat.includes(lockNumber.toString())) {
 
     }
     else {
         return showNotification('دسترسی ندارید', 0);
     }*/


    captchaData = $("#CaptchaData").val();
    captchaVal = $("#CaptchaVal").val();

    var motaghazi = $("#motaghaziChat").val();
    if (motaghazi == "") {
        return showNotification('نام درخواست کننده را وارد کنید', 0);
    }

    localStorage.setItem("MotaghaziChat", motaghazi);
    motaghaziChat = motaghazi;

    if (captchaData.toLowerCase() != captchaVal.toLowerCase()) {
        CreateCaptcha();
        return showNotification('لطفا کد امنیتی را با دقت وارد نمایید', 0);
    }

    var message = "";

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
        Motaghazi: motaghazi,
        IP: ipw,
        CallProg: 'Web',
        LoginLink: loginLink,
        ChatMode: 1
    }
    if (otherUserChat == null) {
        ajaxFunction(ErjSaveTicketUri, 'POST', ErjSaveTicket_HI).done(function (data) {
            idChat = data;
            idChat = idChat == "0" ? null : idChat;
            localStorage.setItem("idChat", idChat);
            // $("#motaghaziChat").hide();
            //$("#Captcha").hide()
            //CalcHeight();

            ChatSend(true);
        });
    }

    $("#modal-NewChat").modal("hide");
    $("#box-chat").show();
    CalcHeight();
    $("#chat-bell").hide();
}

$("#ChatAttach").change(function (e) {
    var file = e.target.files[0];
    SendAttach(file);
});

function SendAttach(file) {

    var name = file.name;
    var size = file.size;

    var hasContract = localStorage.getItem("HasContract");

    if (hasContract != "1" && isAdminChat == false) {
        return showNotification('قرارداد شما پایان یافته است و امکان چت را ندارید', 0);
    }

    /* if (lockChat.includes(lockNumber.toString())) {
 
     }
     else {
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

    if (motaghaziChat == "") {
        motaghaziChat == "UserChat"
    }
    userCodeChat = isAdminChat == true ? userCodeChat : motaghaziChat;


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
        if (isAdminChat == false) {
            SetUpdateChatDownload(false);
        }
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


var videoclip = document.getElementById('videoclipChat');
var videosource = document.getElementById('mp4videoChat');


function ShowVideoChat(e) {

    var href = $(e).attr("href");
    var name = $(e).attr("name");

    var LogLinkTiketUri = server + '/api/Data/LogLinkTiket/';
    var LogLinkTiketObject = {
        LockNumber: lockNumber,
        IP: ipw,
        CallProg: 'Web',
        Link: name
    }
    ajaxFunction(LogLinkTiketUri, 'POST', LogLinkTiketObject, false).done(function (data) {
        if (href == "#") {
            videoclip.pause();
            videosource.setAttribute('src', name);
            videoclip.load();
            videoclip.play();
            $("#Title_VideoChat").text("ویدیو آموزشی");
            $("#modal-VideoChat").modal('show');
        }
    });


}


$('#modal-VideoChat').on('hide.bs.modal', function () {
    videoclip.pause();
    videoclip.currentTime = 0
});


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

$('#ActiveAttachChat').change(function () {
    var check = $('#ActiveAttachChat').is(':checked');
    SetUpdateChatDownload(check);
});


var myWindow;

$(window).focus(function () {
    focusPage = true;
    clearInterval(timerNotifation);
    clearInterval(timerNotifationPage);
    document.title = orgTitle;
    if (myWindow != null) {
        myWindow = null;
    }

});

$(window).blur(function () {
    focusPage = false;
});




function NotifationChat(value) {
    startAnimation();
    CreatePageNotifation(value);

    timerNotifation = setInterval(startAnimation, 200);
    timerNotifationPage =
        setInterval(() => { CreatePageNotifation(value) }, 10000);

    var currentState = false;
    function startAnimation() {
        document.title = currentState ? orgTitle : animatedTitle;
        currentState = !currentState;
    }


    function CreatePageNotifation(value) {
        if (myWindow == null) {
            const windowFeatures = "left=0,top=0,width=300,height=100";
            myWindow = window.open("", 'Notifation', windowFeatures);
        }

        myWindow.document.body.innerHTML = null;
        myWindow.document.title = orgTitle;
        myWindow.document.write(value);
        myWindow.focus();
    }
}




function newExcitingAlerts() {
    /* 
       var myWindow = window.open('','zzz','width=600,height=700');
       myWindow.document.write('test');
     myWindow.focus();
 */


    /*  var child = window.open('Index', 'child');
      window.external.comeback = function () {
          var back = confirm('Are you sure you want to comback?');
          if (back) {
              child.close();
          } else {
              child.focus();
          }
      }زن
      */
    /*
        browser.runtime.onMessage.addListener((message, sender) => {
            console.log('Active Tab ID: ', sender.tab.id);
        });
    
        chrome.tabs.getSelected(null, function (tab) {
            console.log(tab);
        });
        chrome.tabs.query({ currentWindow: true, active: true }, function (tabs) {
            console.log(tabs[0]);
        });
    
        var myWindow0 = window;
        myWindow0.focus();
        */
};




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