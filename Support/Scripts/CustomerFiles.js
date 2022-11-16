var ViewModel = function () {
    var self = this;
    var CustomerFilesUri = server + '/api/Data/CustomerFiles/'; // آدرس فایل
    self.CustomerFilesList = ko.observableArray([]); // ليست فایل
    var CustomerDownloadlUri = server + '/api/Data/CustomerDocumentsDownload/'; // آدرس دانلود

    if (lockNumber == null) {
        window.location.href = localStorage.getItem("urlLogin");
    }

    $("#Index_TextLogo").text('دریافت فایل');

    function getCustomerFilesList(Log) {
        var CustomerFilesObject = {
            LockNumber: lockNumber,
            FlagLog:Log
        }
        ajaxFunction(CustomerFilesUri, 'POST', CustomerFilesObject, true).done(function (data) {
            self.CustomerFilesList(data == null ? [] : data);
        });
    }
    getCustomerFilesList(true);


    self.Download = function (item) {
        var a = document.createElement("a");
        a.href = CustomerDownloadlUri + item.LockNumber + '/' + item.Id;
        a.click();
        setTimeout(function () {
            getCustomerFilesList(false);
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



    $('#refreshCustomerFile').click(function () {
        Swal.fire({
            title: 'تایید به روز رسانی',
            text: "لیست فایل ها به روز رسانی شود ؟",
            type: 'info',
            showCancelButton: true,
            cancelButtonColor: '#3085d6',
            cancelButtonText: 'خیر',

            confirmButtonColor: '#d33',
            confirmButtonText: 'بله'
        }).then((result) => {
            if (result.value) {
                getCustomerFilesList(false);
            }
        })
    })


}; 


ko.applyBindings(new ViewModel());




