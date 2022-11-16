var ViewModel = function () {
    var self = this;
    var FinancialUri = server + '/api/Data/FinancialDocuments/'; // آدرس فاکتور
    var FinancialDownloadlUri = server + '/api/Data/FinancialDocumentsDownload/'; // آدرس دانلود
    self.FinancialList = ko.observableArray([]); // ليست فاکتور

    $("#Index_TextLogo").text('اسناد و مستندات');


    function getFinancialList(log) {
        var FinancialDocumentsObject = {
            LockNumber: lockNumber,
            FlagLog: log
        }
        ajaxFunction(FinancialUri, 'POST', FinancialDocumentsObject, true).done(function (data) {
            self.FinancialList(data == null ? [] : data);
        });
    }
    getFinancialList(true);


    self.Download = function (item) {
        var a = document.createElement("a");
        a.href = FinancialDownloadlUri + item.LockNumber + '/' + item.Id;
        a.click();
        setTimeout(function () {
            getFinancialList(false);
        }, 2000);
        //window.location.reload(true);
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




