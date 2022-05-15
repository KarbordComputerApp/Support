var ViewModel = function () {
    var self = this;

    var CustomerFilesCountUri = server + '/api/Data/CustomerFilesCount/'; // آدرس فایل

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
