var ViewModel = function () {
    var self = this;
    var serialNumber;   // = getUrlParameter('serial');
    var idPersonal;     //= getUrlParameter('personal');

    var DecryptUri = server + '/api/Smob/Decrypt/';
    var Pay_DocHUri = server + '/api/Smob/Pay_DocH/';
    var Pay_DocBUri = server + '/api/Smob/Pay_DocB/';

    self.Pay_DocBList = ko.observableArray([]);
    var Pay_DocB;



    function getDecrypt(value) {
        value = value.substring(0, (value.length - 1)); 
        ajaxFunction(DecryptUri + value, 'GET', false).done(function (data) {
            var data = data.split('&');
            serialNumber = data[0];
            idPersonal = data[1];
        });
    }
    getDecrypt(window.location.search.substring(1));


    function getPay_DocHList() {
        var Pay_DocHObject = {
            SerialNumber: serialNumber,
        }
        ajaxFunction(Pay_DocHUri, 'POST', Pay_DocHObject, true).done(function (data) {
            if (data.length > 0) {
                Pay_DocB = data[0];
                var s = "سال " + Pay_DocB.Sal + " ماه " + Pay_DocB.Mah;
                $("#L_TitleFish").text(s);
            }
        });
    }
    getPay_DocHList();

    function getPay_DocBList() {
        var Pay_DocBObject = {
            SerialNumber: serialNumber,
            IdPersonal: idPersonal
        }
        ajaxFunction(Pay_DocBUri, 'POST', Pay_DocBObject, true).done(function (data) {
            self.Pay_DocBList(data);
        });
    }
    getPay_DocBList();


}


ko.applyBindings(new ViewModel());