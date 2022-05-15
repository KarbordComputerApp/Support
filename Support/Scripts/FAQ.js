var ViewModel = function () {
    var self = this;
    var FinancialUri = server + '/api/Data/FinancialDocuments/'; // آدرس فاکتور
    var FinancialDownloadlUri = server + '/api/Data/FinancialDocumentsDownload/'; // آدرس دانلود
    self.FinancialList = ko.observableArray([]); // ليست فاکتور

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
            //var file = new Blob([content], { type: contentType });
        a.href = FinancialDownloadlUri + item.LockNumber + '/' + item.Id;
           // a.download = fileName;
            a.click();



    }



}; 


ko.applyBindings(new ViewModel());




