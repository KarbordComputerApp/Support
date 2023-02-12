var ViewModel = function () {
    var self = this;
    var FinancialUri = server + '/api/Data/FinancialDocuments/'; // آدرس فاکتور
    var FinancialDownloadlUri = server + '/api/Data/FinancialDocumentsDownload/'; // آدرس دانلود
    var FinancialDocumentsDownload_TesttUri = server + '/api/Data/FinancialDocumentsDownload_Test/'; // آدرس  تست دانلود
    self.FinancialList = ko.observableArray([]); // ليست فاکتور

    $("#Index_TextLogo").text('اسناد و مستندات');


    function getFinancialList(log) {
        var FinancialDocumentsObject = {
            LockNumber: lockNumber,
            FlagLog: log,
            IP: ipw,
            CallProg: 'Web'
        }
        ajaxFunction(FinancialUri, 'POST', FinancialDocumentsObject, true).done(function (data) {
            self.FinancialList(data == null ? [] : data);
        });
    }
    getFinancialList(true);


    self.Download = function (item) {
        ajaxFunction(FinancialDocumentsDownload_TesttUri + item.LockNumber + '/' + item.Id, 'GET', true).done(function (data) {
            if (data == "NotFound") {
                return showNotification('فایل یافت نشد', 0);
            }
            else {
                Swal.fire({
                    title: 'تایید دانلود',
                    text: "آیا فایل انتخابی دانلود شود ؟",
                    type: 'warning',
                    showCancelButton: true,
                    cancelButtonColor: '#3085d6',
                    cancelButtonText: 'خیر',
                    allowOutsideClick: false,
                    confirmButtonColor: '#d33',
                    confirmButtonText: 'بله'
                }).then((result) => {
                    if (result.value) {
                        var a = document.createElement("a");
                        a.href = FinancialDownloadlUri + item.LockNumber + '/' + item.Id + '/' + ipw.replaceAll('.', '-') + '/Web';
                        a.click();
                        setTimeout(function () {
                            getFinancialList(false);
                        }, 2000);
                    }
                });
            }
        })
    }

    $('#refreshFinancial').click(function () {

        Swal.fire({
            title: 'تایید به روز رسانی',
            text: "لیست اسناد و مستندات به روز رسانی شود ؟",
            type: 'info',
            showCancelButton: true,
            cancelButtonColor: '#3085d6',
            cancelButtonText: 'خیر',

            confirmButtonColor: '#d33',
            confirmButtonText: 'بله'
        }).then((result) => {
            if (result.value) {
                getFinancialList(false);
            }
        })
    })


};


ko.applyBindings(new ViewModel());




