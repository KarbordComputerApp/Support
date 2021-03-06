var ViewModel = function () {
    var self = this;
    var serverTiket;
    var aceTiket = "Web2";
    var salTiket = "0000";
    var groupTiket;

    function getAccountDataTiket(lock) {
        ajaxFunctionAccount(AccountUri + 'tiket' + '/' + 'tiket', 'GET').done(function (data) {
            if (data === 0) {
                return showNotification(' اطلاعات تیکت یافت نشد ', 0);
            }
            else {
                serverAddress = data.AddressApi;
                serverTiket = serverAddress;
                groupTiket = data.ERJ_Group;
                localStorage.setItem("groupTiket", data.ERJ_Group);
                localStorage.setItem("ApiAddressTiket", serverAddress);

                /*ajaxFunctionAccount(AccountUri + lock, 'GET').done(function (data) {
                    if (data === 0) {
                        return showNotification(' اطلاعات قفل یافت نشد ', 0);
                    }
                });*/
            }
        });
    }

    getAccountDataTiket(lockNumber);

    self.ErjDocXKList = ko.observableArray([]); // لیست گزارش  

    var ErjDocXKUri = serverTiket + '/api/Web_Data/Web_ErjDocXK/'; // آدرس تیکت ها  
    var TicketStatusUri = serverTiket + '/api/Web_Data/Web_TicketStatus/'; // آدرس وضعیت تیکت ها 

    var RprtColsUri = serverTiket + '/api/Web_Data/RprtCols/'; // آدرس مشخصات ستون ها 
    var DocAttachUri = serverTiket + '/api/Web_Data/DocAttach/'; // آدرس لیست پیوست 
    var DownloadAttachUri = serverTiket + '/api/Web_Data/DownloadAttach/'; // آدرس  دانلود پیوست 
    var ErjSaveTicketUri = serverTiket + '/api/Web_Data/ErjSaveTicket_HI/'; // آدرس  دانلود پیوست 

    var ErjDocAttach_SaveUri = serverTiket + '/api/FileUpload/UploadFile/'; // ذخیره پیوست
    var ErjDocAttach_DelUri = serverTiket + '/api/Web_Data/ErjDocAttach_Del/'; // حذف پیوست

    var serialNumberAttach = 0;
    var serialNumber = 0;
    self.SettingColumnList = ko.observableArray([]); // لیست ستون ها
    self.DocAttachList = ko.observableArray([]); // ليست پیوست

    var counterAttach = 0
    var fileList = [null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null];




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


    getDateServer(serverTiket);

   
    //Get ErjDocXK 
    function getErjDocXK() {
        var ErjDocXKObject = {
            LockNo: lockNumber,
            ModeCode: '204',
        }
        ajaxFunction(ErjDocXKUri + aceTiket + '/' + salTiket + '/' + groupTiket + '/', 'Post', ErjDocXKObject).done(function (dataDocXK) {


            var Object_TicketStatus = {
                SerialNumber: ''
            }
            ajaxFunction(TicketStatusUri + aceTiket + '/' + salTiket + '/' + groupTiket + '/', 'Post', Object_TicketStatus, false).done(function (dataTicketStatus) {

                for (var i = 0; i < dataDocXK.length; i++) {

                    for (var j = 0; j < dataTicketStatus.length; j++) {
                        if (dataDocXK[i].SerialNumber == dataTicketStatus[j].SerialNumber)
                            dataDocXK[i].Status = dataTicketStatus[j].TicketStatusSt;
                    }
                }

                self.ErjDocXKList(dataDocXK);
            });
        });
    }

    getErjDocXK();





    /*self.getTicketStatus = function (serial) {
        v = "";
        var Object_TicketStatus = {
            SerialNumber: serial,
        }
        ajaxFunction(TicketStatusUri + ace + '/' + sal + '/' + group + '/', 'Post', Object_TicketStatus,false).done(function (data) {
            a = self.ErjDocXKList;
            if (data == "")
                v = ""
            else
                v = data[0].TicketStatusSt;
        });
        return v

    }*/


    //Get DocAttach List
    function getDocAttachList(serial) {
        var DocAttachObject = {
            ModeCode: 102,
            SerialNumber: serial
        }
        ajaxFunction(DocAttachUri + aceTiket + '/' + salTiket + '/' + groupTiket, 'POST', DocAttachObject).done(function (data) {
            self.DocAttachList(data);
        });
    }






    self.currentPageIndexErjDocXK = ko.observable(0);

    $('#refreshErjDocXK').click(function () {

        Swal.fire({
            title: 'تایید به روز رسانی',
            text: "لیست تیکت ها به روز رسانی شود ؟",
            type: 'info',
            showCancelButton: true,
            cancelButtonColor: '#3085d6',
            cancelButtonText: 'خیر',

            confirmButtonColor: '#d33',
            confirmButtonText: 'بله'
        }).then((result) => {
            if (result.value) {
                getErjDocXK();
                self.sortTableErjDocXK();
            }
        })
    })


    self.PageCountView = function () {
        select = $('#pageCountSelector').val();
        getErjDocXK();
        self.sortTableErjDocXK();
    }


    var flagupdate = 0;

    self.currentPageErjDocXK = ko.observable();
    pageSizeErjDocXK = localStorage.getItem('pageSizeErjDocXK') == null ? 10 : localStorage.getItem('pageSizeErjDocXK');
    self.pageSizeErjDocXK = ko.observable(pageSizeErjDocXK);

    self.sortType = "ascending";

    self.currentColumn = ko.observable("");
    self.iconType = ko.observable("");


    self.filterDocNo = ko.observable("");
    self.filterDocDate = ko.observable("");
    self.filterStatus = ko.observable("");
    self.filterText = ko.observable("");
    self.filterMotaghazi = ko.observable("");


    self.filterErjDocXKList = ko.computed(function () {
        self.currentPageIndexErjDocXK(0);
        var filterDocNo = self.filterDocNo().toUpperCase();
        var filterDocDate = self.filterDocDate().toUpperCase();
        var filterStatus = self.filterStatus().toUpperCase();
        var filterText = self.filterText().toUpperCase();
        var filterMotaghazi = self.filterMotaghazi().toUpperCase();

        tempData = ko.utils.arrayFilter(self.ErjDocXKList(), function (item) {
            result =
                (item.DocNo == null ? '' : item.DocNo.toString().search(filterDocNo) >= 0) &&
                (item.DocDate == null ? '' : item.DocDate.toString().search(filterDocDate) >= 0) &&
                (item.Status == null ? '' : item.Status.toString().search(filterStatus) >= 0) &&
                (item.Text == null ? '' : item.Text.toString().search(filterText) >= 0) &&
                (item.Motaghazi == null ? '' : item.Motaghazi.toString().search(filterMotaghazi) >= 0)
            return result;
        })
        $("#CountRecord").text(tempData.length);
        return tempData;
    });


    self.currentPageErjDocXK = ko.computed(function () {
        var pageSizeErjDocXK = parseInt(self.pageSizeErjDocXK(), 10),
            startIndex = pageSizeErjDocXK * self.currentPageIndexErjDocXK(),
            endIndex = startIndex + pageSizeErjDocXK;
        localStorage.setItem('pageSizeErjDocXK', pageSizeErjDocXK);
        var a = self.filterErjDocXKList().slice(startIndex, endIndex);
        return self.filterErjDocXKList().slice(startIndex, endIndex);
    });


    self.nextPageErjDocXK = function () {
        if (((self.currentPageIndexErjDocXK() + 1) * self.pageSizeErjDocXK()) < self.filterErjDocXKList().length) {
            self.currentPageIndexErjDocXK(self.currentPageIndexErjDocXK() + 1);
        }
    };

    self.previousPageErjDocXK = function () {
        if (self.currentPageIndexErjDocXK() > 0) {
            self.currentPageIndexErjDocXK(self.currentPageIndexErjDocXK() - 1);
        }
    };

    self.firstPageErjDocXK = function () {
        self.currentPageIndexErjDocXK(0);
    };

    self.lastPageErjDocXK = function () {
        tempCountErjDocXK = parseInt(self.filterErjDocXKList().length / self.pageSizeErjDocXK(), 10);
        if ((self.filterErjDocXKList().length % self.pageSizeErjDocXK()) == 0)
            self.currentPageIndexErjDocXK(tempCountErjDocXK - 1);
        else
            self.currentPageIndexErjDocXK(tempCountErjDocXK);
    };



    self.iconTypeDocNo = ko.observable("");
    self.iconTypeDocDate = ko.observable("");
    self.iconTypeStatus = ko.observable("");
    self.iconTypeText = ko.observable("");
    self.iconTypeMotaghazi = ko.observable("");

    self.sortTableErjDocXK = function (viewModel, e) {

        if (e != null)
            var orderProp = $(e.target).attr("data-column")
        else {
            orderProp = localStorage.getItem("sortTiket");
            self.sortType = localStorage.getItem("sortTypeTiket");
        }

        if (orderProp == null)
            return null

        if (e != null) {
            self.currentColumn(orderProp);
            self.ErjDocXKList.sort(function (left, right) {
                leftVal = FixSortName(left[orderProp]);
                rightVal = FixSortName(right[orderProp]);
                if (self.sortType == "ascending") {
                    return leftVal < rightVal ? 1 : -1;
                }
                else {
                    return leftVal > rightVal ? 1 : -1;
                }
            });

            self.sortType = (self.sortType == "ascending") ? "descending" : "ascending";

            localStorage.setItem("sortTiket", orderProp);
            localStorage.setItem("sortTypeTiket", self.sortType);
        }


        self.iconTypeDocNo('');
        self.iconTypeDocDate('');
        self.iconTypeStatus('');
        self.iconTypeText('');
        self.iconTypeMotaghazi('');

        if (orderProp == 'DocNo') self.iconTypeDocNo((self.sortType == "ascending") ? "glyphicon glyphicon-chevron-up" : "glyphicon glyphicon-chevron-down");
        if (orderProp == 'DocDate') self.iconTypeDocDate((self.sortType == "ascending") ? "glyphicon glyphicon-chevron-up" : "glyphicon glyphicon-chevron-down");
        if (orderProp == 'Status') self.iconTypeStatus((self.sortType == "ascending") ? "glyphicon glyphicon-chevron-up" : "glyphicon glyphicon-chevron-down");
        if (orderProp == 'Text') self.iconTypeText((self.sortType == "ascending") ? "glyphicon glyphicon-chevron-up" : "glyphicon glyphicon-chevron-down");
        if (orderProp == 'Motaghazi') self.iconTypeMotaghazi((self.sortType == "ascending") ? "glyphicon glyphicon-chevron-up" : "glyphicon glyphicon-chevron-down");
    };



    $('#AddNewErjDocXK').click(function () {
        $("#Result").val('');
        $("#motaghazi").val('');
        $('#bodyDocAttach').empty();
        counterAttach = 0;
        fileList = [null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null];
    })



    $("#searchErjDocXK").on("keydown", function search(e) {
        if (allSearchErjDocXK == false) {
            if (e.shiftKey) {
                e.preventDefault();
            }
            else {
                var key = e.charCode || e.keyCode || 0;
                return (
                    key == 8 ||
                    key == 9 ||
                    key == 13 ||
                    key == 46 ||
                    key == 110 ||
                    key == 190 ||
                    (key >= 35 && key <= 40) ||
                    (key >= 48 && key <= 57) ||
                    (key >= 96 && key <= 105)
                );
            }
        }
    });


    //Add   ذخیره تیکت
    async function SaveErjDocXK() {

        natijeh = $("#Result").val();
        motaghazi = $("#motaghazi").val();

        if (natijeh == '' && counterAttach > 0)
            natijeh = 'به پیوست مراجعه شود';

        if (natijeh == '' && counterAttach == 0)
            return showNotification('تیکت خالی است', 0);
        else {
            var ErjSaveTicket_HI = {
                SerialNumber: 0,
                DocDate: DateNow,
                UserCode: 'XXXX',
                Status: "فعال",
                Spec: "",
                LockNo: lockNumber,
                Text: natijeh,
                F01: '',
                F02: '',
                F03: '',
                F04: '',
                F05: '',
                F06: '',
                F07: '',
                F08: '',
                F09: '',
                F10: '',
                F11: '',
                F12: '',
                F13: '',
                F14: '',
                F15: '',
                F16: '',
                F17: '',
                F18: '',
                F19: '',
                F20: '',
                Motaghazi: motaghazi,
            }
            ajaxFunction(ErjSaveTicketUri + aceTiket + '/' + salTiket + '/' + groupTiket + '/', 'POST', ErjSaveTicket_HI).done(function (data) {
                serialNumber = data;
            });

            for (var i = 1; i <= counterAttach; i++) {

                fileAttach = fileList[i];
                fileFullName = fileAttach.name;
                fileData = fileFullName.split(".");
                fileName = fileData[0];
                fileType = '.' + fileData[1];

                let result = await ziped(fileType, fileAttach, fileFullName);

            };

            showNotification('تیکت ارسال شد', 1);
            getErjDocXK();
            $('#modal-ErjDocXK').modal('hide');

        }
    }


    async function ziped(fileType, fileAttach, fileFullName) {
        var zip = new JSZip();


        zip.file('temp' + fileType, fileAttach);
        let result = await zip.generateAsync({ type: "Blob", compression: "DEFLATE" }).then(function (content) {

            var file = new File([content], fileFullName, { type: "zip" });
            var formData = new FormData();
            formData.append("SerialNumber", serialNumber);
            formData.append("ProgName", "ERJ1");
            formData.append("ModeCode", 102);
            formData.append("BandNo", 0);
            formData.append("Code", "");
            formData.append("Comm", "مدرک پیوست - " + DateNow + " - " + fileName);
            formData.append("FName", fileFullName);
            formData.append("Atch", file);

            ajaxFunctionUploadTiket(ErjDocAttach_SaveUri + aceTiket + '/' + salTiket + '/' + groupTiket, formData, false).done(function (response) {

            })
        });

    }


    $('#saveErjDocXK').click(function () {
        SaveErjDocXK();
    })



    self.ViewDocAttach = function (Band) {
        serialNumber = Band.SerialNumber;
        getDocAttachList(Band.SerialNumber);
    }



 


    $('#refreshDocAttach').click(function () {
        Swal.fire({
            title: 'تایید به روز رسانی',
            text: "پیوست ها به روز رسانی شود ؟",
            type: 'info',
            showCancelButton: true,
            cancelButtonColor: '#3085d6',
            cancelButtonText: 'خیر',
            allowOutsideClick: false,
            confirmButtonColor: '#d33',
            confirmButtonText: 'بله'
        }).then((result) => {
            if (result.value) {
                getDocAttachList(serialNumber);
            }
        })
    })






    $('#attachFile').click(function () {
        //if (serialNumber == 0) {
        //     SaveErjDocXK();
        //}

        $('#modal-DocAttachSend').modal('show');
        // getDocAttachList(serialNumber);

    });


    $('#AddAttachs').click(function () {
        //e.preventDefault();
        //$('#AddFiles').val();
        $('#AddFiles').val('').clone(true);
        $("#AddFiles:hidden").trigger('click');

        /*file = 'c:\a\1.png'
        $('#bodyDocAttach').append(
            '<tr>' +
            '<td>' + file + '</td>' +
            '</tr>'
        );
        counterAttach = counterAttach + 1;*/
    });



    this.AddFile = function (data, e) {
        a = e;
        var dataFile;
        var file = e.target.files[0];
        var name = file.name;
        var size = file.size;
        Swal.fire({
            title: 'تایید آپلود ؟',
            text: "آیا " + name + " به پیوست افزوده شود",
            type: 'warning',
            showCancelButton: true,
            cancelButtonColor: '#3085d6',
            cancelButtonText: 'خیر',
            allowOutsideClick: false,
            confirmButtonColor: '#d33',
            confirmButtonText: 'بله'
        }).then((result) => {
            if (result.value) {

                counterAttach = counterAttach + 1;
                a = document.getElementById("AddFiles").files[0];
                fileList[counterAttach] = a;
                fileFullName = fileList[counterAttach].name;
                fileData = fileFullName.split(".");
                fileName = fileData[0];

                $('#bodyDocAttach').append(
                    '<tr>' +
                    '<td style="font-size: 14px;" >' + "مدرک پیوست - " + DateNow + " - " + fileName + '</td>' +
                    '</tr>'
                );

                e.target.value = ""

            }
        })
    }


    $('#DelAllAttach').click(function () {
        $('#bodyDocAttach').empty();
        counterAttach = 0;
        fileList = [null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null];
    });




    self.selectDocAttach = function (item) {

        var fileName = item.FName.split(".");
        var DownloadAttachObject = {
            SerialNumber: item.SerialNumber,
            BandNo: item.BandNo
        }

        ajaxFunction(DownloadAttachUri + aceTiket + '/' + salTiket + '/' + groupTiket, 'POST', DownloadAttachObject, true).done(function (data) {
            var sampleArr = base64ToArrayBuffer(data);
            saveByteArray(fileName[0] + ".zip", sampleArr);
        });
    }

    self.DeleteDocAttach = function (Band) {
        Swal.fire({
            title: 'تایید حذف',
            text: "آیا پیوست انتخابی حذف شود ؟",
            type: 'warning',
            showCancelButton: true,
            cancelButtonColor: '#3085d6',
            cancelButtonText: 'خیر',

            confirmButtonColor: '#d33',
            confirmButtonText: 'بله'
        }).then((result) => {
            if (result.value) {

                Web_DocAttach_Save = {
                    SerialNumber: Band.SerialNumber,
                    ProgName: 'ERJ1',
                    ModeCode: 102,
                    BandNo: Band.BandNo,
                };

                ajaxFunction(ErjDocAttach_DelUri + aceTiket + '/' + salTiket + '/' + groupTiket, 'POST', Web_DocAttach_Save).done(function (response) {
                    getDocAttachList(serialNumber);
                    showNotification('پیوست حذف شد', 1);
                });
            }
        })
    };

    $("#AddNewDocAttach").on('click', function (e) {
        e.preventDefault();
        $("#upload:hidden").trigger('click');
    });

    this.fileUpload = function (data, e) {
        var dataFile;
        var file = e.target.files[0];
        var name = file.name;
        var size = file.size;
        Swal.fire({
            title: 'تایید آپلود ؟',
            text: "آیا " + name + " به پیوست افزوده شود",
            type: 'warning',
            showCancelButton: true,
            cancelButtonColor: '#3085d6',
            cancelButtonText: 'خیر',
            allowOutsideClick: false,
            confirmButtonColor: '#d33',
            confirmButtonText: 'بله'
        }).then((result) => {
            if (result.value) {
                var file = document.getElementById("upload");

                fileFullName = file.files[0].name;
                fileData = fileFullName.split(".");
                fileName = fileData[0];
                fileType = '.' + fileData[1];

                var zip = new JSZip();


                zip.file('temp' + fileType, file.files[0]);
                zip.generateAsync({ type: "Blob", compression: "DEFLATE" }).then(function (content) {

                    var file = new File([content], fileFullName, { type: "zip" });

                    //file = $("#upload")[0].files[0];


                    var formData = new FormData();

                    formData.append("SerialNumber", serialNumber);
                    formData.append("ProgName", "ERJ1");
                    formData.append("ModeCode", 102);
                    formData.append("BandNo", 0);
                    formData.append("Code", "");
                    formData.append("Comm", "مدرک پیوست - " + DateNow + " - " + fileName);
                    formData.append("FName", fileFullName);
                    formData.append("Atch", file);

                    ajaxFunctionUploadTiket(ErjDocAttach_SaveUri + aceTiket + '/' + salTiket + '/' + groupTiket, formData, true).done(function (response) {
                        getDocAttachList(serialNumber);
                    })
                });
            }
        })



    };

    //del DocAttach  حذف پیوست
    function ErjDocAttach_Del(bandNoImput) {
        Web_DocAttach_Del = {
            SerialNumber: serialNumber,
            ProgName: '',
            ModeCode: '',
            BandNo: bandNoImput
        };
        ajaxFunction(ErjDocAttach_DelUri + aceTiket + '/' + salTiket + '/' + groupTiket, 'POST', Web_DocAttach_Del).done(function (response) {
        });
    };


    function dataURItoBlob(dataURI) {
        // convert base64/URLEncoded data component to raw binary data held in a string
        var byteString;
        if (dataURI.split(',')[0].indexOf('base64') >= 0)
            byteString = atob(dataURI.split(',')[1]);
        else
            byteString = unescape(dataURI.split(',')[1]);

        // separate out the mime component
        var mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0];

        // write the bytes of the string to a typed array
        var ia = new Uint8Array(byteString.length);
        for (var i = 0; i < byteString.length; i++) {
            ia[i] = byteString.charCodeAt(i);
        }

        return new Blob([ia], { type: mimeString });
    }


































    $('#modal-Erja').on('shown.bs.modal', function () {

    });




    self.radif = function (index) {
        countShow = self.pageSizeErjDocXK();
        page = self.currentPageIndexErjDocXK();
        calc = (countShow * page) + 1;
        return index + calc;
    }



    self.SearchKeyDown = function (viewModel, e) {
        return KeyPressSearch(e);
    }
    self.sortTableErjDocXK();
};

ko.applyBindings(new ViewModel());

