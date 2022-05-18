var ViewModel = function () {
    var self = this;
    var FinancialUri = server + '/api/Data/FinancialDocuments/'; // آدرس فاکتور
    var FinancialDownloadlUri = server + '/api/Data/FinancialDocumentsDownload/'; // آدرس دانلود
    self.FinancialList = ko.observableArray([]); // ليست فاکتور

    $("#Index_TextLogo").text('اسناد و مستندات');


    function getFinancialList() {
        var FinancialDocumentsObject = {
            LockNumber: lockNumber
        }
        ajaxFunction(FinancialUri, 'POST', FinancialDocumentsObject, true).done(function (data) {
            self.FinancialList(data == null ? [] : data);
        });
    }
    getFinancialList();


    self.Download = function (item) {
        var a = document.createElement("a");
        a.href = FinancialDownloadlUri + item.LockNumber + '/' + item.Id;
        a.click();
        setTimeout(function () {
            getFinancialList();
        }, 2000);
        //window.location.reload(true);
    }
}; 


ko.applyBindings(new ViewModel());




