var ViewModel = function () {
    var self = this;
    var serverAddress;
    var aceCustAccount = "Web8";
    var salCustAccount = "0000";
    var groupCustAccount;
    var loginAccount = "NRlhOcngQl7BwNOhU104";


    if (lockNumber == null) {
        window.location.href = localStorage.getItem("urlLogin");
    }

    function getAccountDataCustAccount(lock) {
        ajaxFunctionAccount(AccountUri + custAccountUser + '/' + custAccountPass, 'GET').done(function (data) {
            if (data === 0) {
                return showNotification(' اطلاعات لینک پرداخت یافت نشد ', 0);
            }
            else {
                serverAddress = data.AddressApi;
                groupCustAccount = data.AFI8_Group;
                localStorage.setItem("groupCustAccount", data.AFI8_Group);
                localStorage.setItem("ApiAddressCustAccount", serverAddress);
            }
        });
    }

    getAccountDataCustAccount(lockNumber);


    self.CustAccountList = ko.observableArray([]); // لیست گزارش  
    self.FDocP_CustAcountList = ko.observableArray([]); // لیست چاپ  

    var CustAccountUri = serverAddress + '/api/Web_Data/CustAccount/'; // آدرس فاکتور ها  
    var FDocP_CustAcountUri = serverAddress + '/api/Web_Data/FDocP_CustAcount/'; // آدرس چاپ فاکتور   
    var SalePaymentUri = serverAddress + '/api/Shaparak/SalePayment'; // آدرس پرداخت  
    var PaymentConfirmUri = serverAddress + '/api/Shaparak/PaymentConfirm'; // آدرس تایید پرداخت  
    var CustAccountSaveUri = serverAddress + "/api/Web_Data/CustAccountSave/"; // آدرس ذخیره لینک پرداخت 
    var DocAttachUri = serverAddress + '/api/Web_Data/DocAttach/'; // آدرس لیست پیوست 
    //var DownloadAttachUri = serverAddress + '/api/Web_Data/DownloadAttach/'; // آدرس  دانلود پیوست 



    createViewer();

    function getDateServer(server) {
        var date;
        if (server != null) {
            var DateUri = server + '/api/Web_Data/Date/'; // آدرس  تاریخ سرور
            ajaxFunction(DateUri, 'GET').done(function (data) {
                listDate = data[0].split("/");
                DateNow = data[0];
                SalNow = listDate[0];
            });
        }
    }


    getDateServer(serverAddress);

    function getCustAccount() {
        var CustAccountObject = {
            LockNo: lockNumber,
        }
        ajaxFunction(CustAccountUri + aceCustAccount + '/' + salCustAccount + '/' + groupCustAccount + '/', 'Post', CustAccountObject).done(function (data) {
            self.CustAccountList(data)
        });
    }

    getCustAccount();


    $('#refreshCustAccount').click(function () {

        Swal.fire({
            title: 'تایید به روز رسانی',
            text: "لیست صورتحساب من به روز رسانی شود ؟",
            type: 'info',
            showCancelButton: true,
            cancelButtonColor: '#3085d6',
            cancelButtonText: 'خیر',

            confirmButtonColor: '#d33',
            confirmButtonText: 'بله'
        }).then((result) => {
            if (result.value) {
                getCustAccount();
            }
        })
    })




    self.ShowLinkPardakht = function (list) {
        callBackUrl = "https://karbordcomputerapp.ir/Pay/PaymentCallback";
        random = Math.floor(Math.random() * 90000) + 10000;
        var SalePaymentRequestObject = {
            'CallBackUrl': callBackUrl,
            'LoginAccount': loginAccount,
            'Amount': list.TotalValue,
            'AdditionalData': '',
            'OrderId': list.DocDate.substring(0, 4) + list.SerialNumber + random,
            'Originator': '',
        }
        ajaxFunction(SalePaymentUri, 'Post', SalePaymentRequestObject).done(function (dataLink) {

            if (dataLink.Status == 0) {
                token = dataLink.Token;
                uriPay = dataLink.location;

                var CustAccountSaveObject = {
                    'Year': list.Year,//DocDate.substring(0, 4),
                    'SerialNumber': list.SerialNumber,
                    'OnlineParLink': uriPay,
                    'DownloadCount': null,
                }
                ajaxFunction(CustAccountSaveUri + aceCustAccount + '/' + salCustAccount + '/' + groupCustAccount, 'Post', CustAccountSaveObject).done(function (dataSave) {
                    getCustAccount();
                });


                const win = window.open(uriPay, '_blank');

                const timer = setInterval(() => {
                    if (win.closed) {
                        clearInterval(timer);
                        getCustAccount;
                        //ConfirmPayment(token);
                    }
                }, 1000);

                // window.open(uriPay, '_blank');

            }
            else {
                return showNotification(dataLink.Message, 0)
            }
        });

    }



    self.ResidPardakht = function (list) {

        var DocAttachObject = {
            ProgName: 'FCT5',
            Group: groupCustAccount,
            Year: list.Year,
            ModeCode: '2',
            SerialNumber: list.SerialNumber,
            BandNo: 0,
            ByData: 0
        }

        ajaxFunction(DocAttachUri, 'POST', DocAttachObject).done(function (data) {
            item = null;
            if (data.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].Comm == "رسید")
                        item = data[i];
                }
                if (item != null) {
                    var fileName = item.FName.split(".");

                    var DownloadAttachObject = {
                        ProgName: 'FCT5',
                        Group: groupCustAccount,
                        Year: list.Year,
                        ModeCode: '2',
                        SerialNumber: item.SerialNumber,
                        BandNo: item.BandNo,
                        ByData: 1
                    }
                    ajaxFunction(DocAttachUri, 'POST', DownloadAttachObject).done(function (data) {
                        var sampleArr = base64ToArrayBuffer(data[0].Atch);
                        saveByteArray(fileName[0] + ".zip", sampleArr);
                    });
                }
            }

            if (item == null)
                return showNotification(' اطلاعات رسید یافت نشد ', 0);

        });

    }





    function getFDocP_CustAcount(year, serial) {
        var FDocP_CustAcountObject = {
            Year: year,
            SerialNumber: serial
        }
        ajaxFunction(FDocP_CustAcountUri + aceCustAccount + '/' + salCustAccount + '/' + groupCustAccount + '/', 'Post', FDocP_CustAcountObject, false).done(function (data) {
            self.FDocP_CustAcountList(data)
        });
    }


    self.ChapFactor = function (list) {

        count = list.DownloadCount == '' ? 1 : parseInt(list.DownloadCount) + 1;
        Swal.fire({
            title: 'تایید چاپ فاکتور',
            text: (count == 1 ? "دو" : "یک") + " بار امکان چاپ فاکتور وجود دارد.آیا چاپ شود؟",
            type: 'info',
            showCancelButton: true,
            cancelButtonColor: '#3085d6',
            cancelButtonText: 'خیر',

            confirmButtonColor: '#d33',
            confirmButtonText: 'بله'
        }).then((result) => {
            if (result.value) {
                printVariable = '"ReportDate":"' + DateNow + '",';
                getFDocP_CustAcount(list.DocDate.substring(0, 4), list.SerialNumber);
                setReport(self.FDocP_CustAcountList(), '/Content/Report/SFCT.json?10', printVariable);

                var CustAccountSaveObject = {
                    'Year': list.Year,//list.DocDate.substring(0, 4),
                    'SerialNumber': list.SerialNumber,
                    'OnlineParLink': null,
                    'DownloadCount': count,
                }
                ajaxFunction(CustAccountSaveUri + aceCustAccount + '/' + salCustAccount + '/' + groupCustAccount, 'Post', CustAccountSaveObject).done(function (dataSave) {
                    getCustAccount();
                });
            }
        })

    };



};

ko.applyBindings(new ViewModel());

