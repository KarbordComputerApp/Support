var ViewModel = function () {
    var self = this;


    //localStorage.removeItem("SeeInformationKarbord_Support");
    var SeeInformation = localStorage.getItem("SeeInformationKarbord_Support");
    if (SeeInformation == null || SeeInformation == '') {
        $("#modal-Information").modal("show");
    }

    $("#SeeInformation").click(function () {
        localStorage.setItem("SeeInformationKarbord_Support", "Ok");
        $("#modal-Information").modal("hide");
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
        ajaxFunction(AceMessagesUri , 'GET', true).done(function (data) {
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



    var CustomerFilesCountUri = server + '/api/Data/CustomerFilesCount/'; // آدرس فایل



    //$('#B_Tiket').attr('disabled', 'disabled');
    //if (lockNumber == '10000') {
    //    $('#B_Tiket').removeAttr('disabled', 'disabled');
    //}

    companyName = localStorage.getItem("CompanyName");

    $('#FullName').text(companyName);

    function getCustomerFilesCount() {
        var CustomerFilesObject = {
            LockNumber: lockNumber
        }
        ajaxFunction(CustomerFilesCountUri, 'POST', CustomerFilesObject, true).done(function (data) {
            $("#t_CustomerFiles").text('تعداد فایل های موجود : ' + data[0]);
        });
    }
    getCustomerFilesCount();



    //GetCountErjDocXK();




    /* $("#p_information").show();
     $("#LastPadding").hide();
 
     if (window.matchMedia('(max-width: 767px)').matches) {
         $("#LastPadding").show();
         $("#p_information").hide();
         $("#p_CustomerFiles").hide();
         $("#p_UploadFiles").hide();
         $("#p_FinancialDocuments").hide();
         $("#p_FAQ").hide();
 
     }*/



};
ko.applyBindings(new ViewModel());
