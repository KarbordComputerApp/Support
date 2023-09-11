var ViewModel = function () {
    var self = this;
    var group_CustAccount = localStorage.getItem("Group_CustAccount");

    var loginAccount = "NRlhOcngQl7BwNOhU104";
    $("#Index_TextLogo").text('صورتحساب های من');

    if (lockNumber == null || group_CustAccount == null) {
        window.location.href = localStorage.getItem("urlLogin");
    }


    self.CustAccountList = ko.observableArray([]); // لیست گزارش  
    self.FDocP_CustAcountList = ko.observableArray([]); // لیست چاپ  

    var CustAccountUri = server + '/api/KarbordData/CustAccount/'; // آدرس فاکتور ها  
    var FDocP_CustAcountUri = server + '/api/KarbordData/FDocP_CustAcount/'; // آدرس چاپ فاکتور   
    var SalePaymentUri = server + '/api/Shaparak/SalePayment'; // آدرس پرداخت  
    var CustAccountSaveUri = server + "/api/KarbordData/CustAccountSave/"; // آدرس ذخیره لینک پرداخت 
    var DocAttachUri = server + '/api/KarbordData/DocAttach/'; // آدرس لیست پیوست 



    createViewer();

    function getDateServer() {
        var date;
        if (server != null) {
            var DateUri = server + '/api/KarbordData/Date/'; // آدرس  تاریخ سرور
            ajaxFunction(DateUri, 'GET').done(function (data) {
                listDate = data[0].split("/");
                DateNow = data[0];
                SalNow = listDate[0];
            });
        }
    }


    getDateServer();

    function getCustAccount(flag) {
        var CustAccountObject = {
            LockNo: lockNumber,
            FlagLog: flag,
            IP: ipw,
            CallProg: 'Web'
        }
        ajaxFunction(CustAccountUri , 'Post', CustAccountObject).done(function (data) {
            self.CustAccountList(data)
        });
    }

    getCustAccount(true);


    $('#refreshCustAccount').click(function () {

        Swal.fire({
            title: 'تایید به روز رسانی',
            text: "لیست صورتحساب های من به روز رسانی شود ؟",
            type: 'info',
            showCancelButton: true,
            cancelButtonColor: '#3085d6',
            cancelButtonText: 'خیر',

            confirmButtonColor: '#d33',
            confirmButtonText: 'بله'
        }).then((result) => {
            if (result.value) {
                getCustAccount(false);
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
                ajaxFunction(CustAccountSaveUri, 'Post', CustAccountSaveObject).done(function (dataSave) {
                    getCustAccount(false);
                });


                const win = window.open(uriPay, '_blank');

                const timer = setInterval(() => {
                    if (win.closed) {
                        clearInterval(timer);
                        getCustAccount(false);
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
            Group: group_CustAccount,
            Year: list.Year,
            ModeCode: '2',
            SerialNumber: list.SerialNumber,
            BandNo: 0,
            ByData: 0,
            IP: ipw,
            CallProg: 'Web'
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
                        Group: group_CustAccount,
                        Year: list.Year,
                        ModeCode: '2',
                        SerialNumber: item.SerialNumber,
                        BandNo: item.BandNo,
                        ByData: 1,
                        IP: ipw,
                        CallProg: 'Web'
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
            LockNumber: lockNumber,
            Year: year,
            SerialNumber: serial,
            IP: ipw,
            CallProg: 'Web'
        }
        ajaxFunction(FDocP_CustAcountUri , 'Post', FDocP_CustAcountObject, false).done(function (data) {
            self.FDocP_CustAcountList(data)
        });
    }


    self.ChapFactor = function (list) {

        //count = list.DownloadCount == '' ? 1 : parseInt(list.DownloadCount) + 1;
        Swal.fire({
            title: 'تایید چاپ فاکتور',
            text:'آیا فاکتور انتخابی چاپ شود ؟', // (count == 1 ? "دو" : "یک") + " بار امکان چاپ فاکتور وجود دارد.آیا چاپ شود؟",
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
                setReport(self.FDocP_CustAcountList(), '/Content/Report/SFCT.html?17', printVariable);

                /*var CustAccountSaveObject = {
                    'Year': list.Year,//list.DocDate.substring(0, 4),
                    'SerialNumber': list.SerialNumber,
                    'OnlineParLink': null,
                    'DownloadCount': count,
                }
                ajaxFunction(CustAccountSaveUri, 'Post', CustAccountSaveObject).done(function (dataSave) {
                    getCustAccount(false);
                });*/
            }
        })

    };



};

ko.applyBindings(new ViewModel());

