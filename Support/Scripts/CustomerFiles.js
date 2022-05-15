var ViewModel = function () {
    var self = this;
    var CustomerFilesUri = server + '/api/Data/CustomerFiles/'; // آدرس فایل
    self.CustomerFilesList = ko.observableArray([]); // ليست فایل

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
        a = item
    }



}; 


ko.applyBindings(new ViewModel());




