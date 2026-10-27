var ViewModel = function () {
    var self = this;
    //localStorage.removeItem("SeeInformation01Karbord_Support");

    var SeeInformation = localStorage.getItem("SeeInformationKarbord_Support");
    if (SeeInformation == null || SeeInformation == '') {
        $("#modal-Information").modal("show");
        $("#InformationNote").text("امکان چت به سایت اضافه شد.لطفا برای چت بر روی آیکن چت در پایین صفحه سمت راست کلیک کنید");
    }

    $("#SeeInformation").click(function () {
        localStorage.setItem("SeeInformationKarbord_Support", "Ok");
        $("#modal-Information").modal("hide");
    });


    var SeeInformation01 = localStorage.getItem("SeeInformation01Karbord_Support");
    if (SeeInformation01 == null || SeeInformation01 == '') {
        $("#modal-Information01").modal("show");
        $("#Information01Note").html(
            "مشتریان محترم" + "<br> "+
            "از این پس می توانید جهت برقراری ارتباطی امن تر و سریعتر با شرکت، از نرم افزار اختصاصی KarbordDesk استفاده نمایید." +
            "این نرم افزار به صورت رایگان در اختیار مشتریان گرامی قرار دارد. جهت دریافت نرم افزار، به قسمت «دریافت فایل» مراجعه فرمایید."
            );
    }

    $("#SeeInformation01").click(function () {
        localStorage.setItem("SeeInformation01Karbord_Support", "Ok");
        $("#modal-Information01").modal("hide");
    });




    image = localStorage.getItem("Pic");

    if (image != "null" && image != "undefined") {
        var picUrl = base64Url(image);
        imageUser.src = picUrl;
    }


    if (lockNumber == null) {
        window.location.href = localStorage.getItem("urlLogin");
    }

    var AceMessagesUri = server + '/api/Data/AceMessages/';
    self.AceMessagesList = ko.observableArray([]);

    function getAceMessagesList() {
        ajaxFunction(AceMessagesUri, 'GET', true).done(function (data) {
            var list = '';
            for (var i = 0; i < data.length; i++) {
                list += `<div style=" background-color:#128d35;padding: 5px 20px 5px 20px;border-radius: 25px;margin-bottom: 5px;">
                      <p style="font-size: 12px; color:white;margin-top: 11px;">`+ data[i].Message + `</p>
                 </div>`;
            }
            $('#AceMessages').append(list);

        });
    }
    getAceMessagesList();


    $('#Index_TextLogo').css("color", "#dcdcdc");
    $('#Index_TextBack').css("color", "#dcdcdc");

    $('#Index_TextBack').hide();

    $('#Index_Home').attr('src', '/Content/img/Icon_White/Home.png');
    $('#Index_Tiket').attr('src', '/Content/img/Icon_White/Alarm.png');
    $('#Index_User').attr('src', '/Content/img/Icon_White/User.png');





    //$('#B_Tiket').attr('disabled', 'disabled');
    //if (lockNumber == '10000') {
    //    $('#B_Tiket').removeAttr('disabled', 'disabled');
    //}

    companyName = localStorage.getItem("CompanyName");

    $('#FullName').text(companyName);


    $("#t_CustomerFiles").hide();
    getCustomerFiles();

    //GetCountErjDocXK();

    if ("serviceWorker" in navigator) {
        navigator.serviceWorker.register("/sw.js")
            .then(function (registration) {
                //showNotification("Service Worker ثبت شد.",1);
                console.log("Service Worker ثبت شد.");
                console.log(registration);
            })
            .catch(function (error) {
                console.log("Service Worker Error:" + error);
            });
    }

    $(document).ready(async function () {
        if (!("Notification" in window)) {
            showNotification("مرورگر شما نوتیفیکشن را پشتیبانی نمی‌کند.", 0);
        }

        var per = Notification.permission;
        if (per == "granted") {
            //showNotification("سرویس نوتیفیکشن در حال اجرا",1);
        }
        else {
            if (per == "default") {
                showNotification("جهت فعال سازی نوتیفیکشن دکمه Allow را انتخاب کنید",3);
                permission = await Notification.requestPermission();
                if (permission == "granted") {
                    await EnableNotification();
                }
            }
            else {
                showNotification(
                    "دسترسی اعلان‌ها مسدود شده است. برای فعال‌سازی، روی علامت قفل کنار آدرس سایت کلیک کنید، وارد Site settings شوید و گزینه Notifications را روی Allow قرار دهید؛ سپس صفحه را مجدداً بارگذاری کنید.",3);
                //$("#btnEnable").Show();
            }
        }
    });

};
ko.applyBindings(new ViewModel());
