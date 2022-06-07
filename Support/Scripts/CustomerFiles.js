var ViewModel = function () {
    var self = this;
    var CustomerFilesUri = server + '/api/Data/CustomerFiles/'; // آدرس فایل
    self.CustomerFilesList = ko.observableArray([]); // ليست فایل
    var CustomerDownloadlUri = server + '/api/Data/CustomerDocumentsDownload/'; // آدرس دانلود

    $("#Index_TextLogo").text('دریافت فایل');

    function getCustomerFilesList() {
        var CustomerFilesObject = {
            LockNumber: lockNumber
        }
        ajaxFunction(CustomerFilesUri, 'POST', CustomerFilesObject, true).done(function (data) {
            self.CustomerFilesList(data == null ? [] : data);
        });
    }
    getCustomerFilesList();


    self.Download = function (item) {
        var a = document.createElement("a");
        a.href = CustomerDownloadlUri + item.LockNumber + '/' + item.Id;
        a.click();
        setTimeout(function () {
            getCustomerFilesList();
        }, 2000);
    }

    self.ReplaceSpec = function (item) {
        return item.replace(/<\/?[^>]+(>|$)/g, "");
    }

    self.ReplaceDate = function (item) {
        date = item.substring(0, 10);
        date = date.replaceAll('-', '/'); 
        return convertDate(date);
    }

    self.ReplaceTime = function (item) {
        return item.substring(11, 100);
    }




}; 


ko.applyBindings(new ViewModel());




