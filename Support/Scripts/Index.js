var ViewModel = function () {
    var self = this;

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
