var ViewModel = function () {
    var self = this;

    
    var AceMessagesUri = server + '/api/Data/AceMessages/';
    self.AceMessagesList = ko.observableArray([]);

    function getAceMessagesList() {
        ajaxFunction(AceMessagesUri, 'GET', true).done(function (data) {
            var list = '';
            for (var i = 0; i < data.length; i++) {
                list += `<div style=" background-color:#128d35;padding: 5px 20px 5px 20px;border-radius: 25px;margin-bottom: 5px;">
                      <p style="font-size: 12px; color:white;margin-top: 11px;">`+ data[i].Message+`</p>
                 </div>`;
            }
            $('#AceMessages').append(list);

        });
    }
    getAceMessagesList();
    



    $('#Index_TextLogo').css("color", "#dcdcdc");
    $('#Index_TextBack').css("color", "#dcdcdc");

    $('#Index_Pic_Alarm').attr('src', '/Content/img/Icon_White/Alarm.png');
    $('#Index_User').attr('src', '/Content/img/Icon_White/User.png');

    var CustomerFilesCountUri = server + '/api/Data/CustomerFilesCount/'; // آدرس فایل


    $('#FullName').text(fullName + ' خوش آمدید');

    function getCustomerFilesCount() {
        var CustomerFilesObject = {
            LockNumber: lockNumber
        }
        ajaxFunction(CustomerFilesCountUri, 'POST', CustomerFilesObject, true).done(function (data) {
            $("#t_CustomerFiles").text('تعداد فایل های موجود : ' + data[0]);
        });
    }
    getCustomerFilesCount();





};
ko.applyBindings(new ViewModel());
