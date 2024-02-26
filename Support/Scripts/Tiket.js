var ViewModel = function () {
    var self = this;
    var flagSend = false;
    //var group_Tiket = localStorage.getItem("Group_Ticket");
    //if (group_Tiket.length == 1) {
    //    group_Tiket = '0' + group_Tiket;
    //}

    var loginLink = false;
    $("#Index_TextLogo").text('تیکت پشتیبانی');

    var LockInput = $("#LockInput").data("value");
    var PassInput = $("#PassInput").data("value");

    if (LockInput == 'NotAccess') {
        alert('شما به این بخش دسترسی ندارید');
        window.location.href = localStorage.getItem("urlLogin");
    }
    else if (LockInput != '') {
        loginLink = true;
        getIP();
        localStorage.removeItem("lockNumber");
        lockNumber = LockInput;

        LockNumbersUri = server + '/api/Data/LockNumbers/';

        var LockNumbersObject = {
            LockNumber: lockNumber,
            IP: ipw,
            CallProg: 'Web'
        }
        ajaxFunction(LockNumbersUri, 'POST', LockNumbersObject, false).done(function (dataLock) {
            if (dataLock.length > 0) {
                companyName = dataLock[0].CompanyName.split("-")[0];
            }
        })
    }


    //if (lockNumber == null || group_Tiket == null) {
    //    window.location.href = localStorage.getItem("urlLogin");
    //}


    self.ErjDocXKList = ko.observableArray([]); // لیست گزارش  

    var ErjDocXKUri = server + '/api/KarbordData/Web_ErjDocXK/'; // آدرس تیکت ها  
    var TicketStatusUri = server + '/api/KarbordData/Web_TicketStatus/'; // آدرس وضعیت تیکت ها 
    var DocAttachUri = server + '/api/KarbordData/DocAttach/'; // آدرس لیست پیوست 
    var ErjSaveTicketUri = server + '/api/KarbordData/ErjSaveTicket_HI/'; // آدرس  دانلود پیوست 
    var ErjDocAttach_SaveUri = server + '/api/KarbordData/UploadFile/'; // ذخیره پیوست
    var ErjDocAttach_DelUri = server + '/api/KarbordData/ErjDocAttach_Del/'; // حذف پیوست
    var Ticket_UpdateResultUri = server + '/api/KarbordData/Ticket_UpdateResult/'; // خواندن نتیجه تیکت

    var ChatCountTiketUri = server + '/api/Data/ChatCountTiket/';


    var serialNumberAttach = 0;
    var serialNumber = 0;
    self.SettingColumnList = ko.observableArray([]); // لیست ستون ها
    self.DocAttachList = ko.observableArray([]); // ليست پیوست

    self.AddAttachList = ko.observableArray([]);





    function getDateServer() {
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


    var videoclip = document.getElementById('videoclip');
    var videosource = document.getElementById('mp4video');

    self.ViewResultSt = function (Band) {
        $('#LinkSt').empty();
        if (Band.ResultSt.length > 0) {
            $('#titleComm').text('نتیجه');
            $('#modal-Comm').modal('show');
            $('#ResultSt').text(Band.ResultSt);
            list = Band.LinkSt.split("\r\n");
            item = "";
            for (var i = 0; i < list.length; i++) {

                //downloadVideoUri = server + '/api/Data/DownloadVideo/' + lockNumber + "/Web/" + ipw + "/";
                linkSt = list[i].toLowerCase();
                isVideo = linkSt.search("/content/video/") > 0;

                textVideo = "ویدیو آموزشی";

                if (list[i] != "") {
                    div1 = $('<div class="row" style="margin - top: 5px; margin - bottom: 5px!important;">');
                    span = $('<span class="col-1" style="margin-left: 5px;">لینک<span style="padding-right: 4px;padding-left: 3px;">' + (i + 1) + '</span>:</span>');
                    div2 = $('<div class="col" style="text-overflow: ellipsis;overflow: hidden;white-space: nowrap;direction: ltr;max-width: 440px;">');
                    if (isVideo == true) {
                        a = $('<a href = "#" name = "' + list[i] + '"><span>' + textVideo + '</span></a>');
                    }
                    else {
                        a = $('<a href="' + list[i] + '" target="_blank" name = "' + list[i] + '"><span>' + linkSt + '  </span></a>');
                    }

                    div2.append(a);
                    div1.append(span);
                    div1.append(div2);
                    $('#LinkSt').append(div1);

                    a.click(function () {
                        var href = $(this).attr("href");
                        var name = $(this).attr("name");

                        var LogLinkTiketUri = server + '/api/Data/LogLinkTiket/';
                        var LogLinkTiketObject = {
                            LockNumber: lockNumber,
                            IP: ipw,
                            CallProg: 'Web',
                            Link: name
                        }
                        ajaxFunction(LogLinkTiketUri, 'POST', LogLinkTiketObject, false).done(function (data) {
                            if (href == "#") {
                                videoclip.pause();
                                videosource.setAttribute('src', name);
                                videoclip.load();
                                videoclip.play();
                                $("#Title_Video").text(textVideo);
                                $("#modal-Video").modal('show');
                            }
                        });
                    });

                    /* item +=
                         '<div class="row" style="margin-top: 5px;margin-bottom: 5px !important;">' +
                         '<span class="col-1" style="margin-left: 5px;">لینک<span style="padding-right: 4px;padding-left: 3px;">' + (i + 1) + '</span>:</span>' +
                         '<div class="col" style="text-overflow: ellipsis;overflow: hidden;white-space: nowrap;direction: ltr;max-width: 440px;">' +
                         '<a class="LinkTiket"><span>' + list[i] + '  </span></a>' +
                         '</div>' +
                         '</div>';*/
                    //item += '<a href ="' + list[i] + '">لینک<span>' + (i + 1) + '  </span>:</a><br>';
                }
            }


            $('#modal-Video').on('hide.bs.modal', function () {
                videoclip.pause();
                videoclip.currentTime = 0
            });


            //$('#LinkSt').append(item);

            var Object_Ticket_UpdateResult = {
                SerialNumber: Band.SerialNumber,
            }
            ajaxFunction(Ticket_UpdateResultUri, 'Post', Object_Ticket_UpdateResult).done(function (dataDocXK) {
                GetCountErjDocXK();
                getErjDocXK(false);
            });
        }

    }


    self.ViewText = function (Band) {
        $('#LinkSt').empty();
        if (Band.Text.length > 15) {
            $('#titleComm').text('توضیحات');
            $('#modal-Comm').modal('show');
            $('#ResultSt').text(Band.Text);
        }
    }

    //Get ErjDocXK 
    function getErjDocXK(log) {
        var ErjDocXKObject = {
            LockNo: lockNumber,
            ModeCode: '204',
            FlagLog: log,
            IP: ipw,
            CallProg: 'Web',
            LoginLink: loginLink
        }
        ajaxFunction(ErjDocXKUri, 'Post', ErjDocXKObject).done(function (dataDocXK) {

            var Object_TicketStatus = {
                SerialNumber: '',
                LockNumber: lockNumber,
                IP: ipw,
                CallProg: 'Web'
            }
            ajaxFunction(TicketStatusUri, 'Post', Object_TicketStatus, false).done(function (dataTicketStatus) {

                for (var i = 0; i < dataDocXK.length; i++) {
                    dataDocXK[i].ChatCount = 0;
                    for (var j = 0; j < dataTicketStatus.length; j++) {
                        if (dataDocXK[i].SerialNumber == dataTicketStatus[j].SerialNumber)
                            dataDocXK[i].Status = dataTicketStatus[j].TicketStatusSt;
                    }
                }

                //self.ErjDocXKList(dataDocXK);
            });




            var ChatCountTiketObject = {
                LockNumber: lockNumber,
            }
            ajaxFunction(ChatCountTiketUri, 'Post', ChatCountTiketObject, false).done(function (dataChatCount) {

                for (var i = 0; i < dataDocXK.length; i++) {
                    for (var j = 0; j < dataChatCount.length; j++) {
                        if (dataDocXK[i].SerialNumber == dataChatCount[j].SerialNumber)
                            dataDocXK[i].ChatCount = dataChatCount[j].ChatCount;
                    }
                }

                
            });

            self.ErjDocXKList(dataDocXK);




        });
    }

    getErjDocXK(true);





    //Get DocAttach List
    function getDocAttachList(serial) {
        var DocAttachObject = {
            ProgName: 'ERJ1',
            ModeCode: '102',
            //Group: group_Tiket,
            Year: '0000',
            SerialNumber: serial,
            BandNo: 0,
            ByData: 0,
            IP: ipw,
            CallProg: 'Web'
        }

        ajaxFunction(DocAttachUri, 'POST', DocAttachObject).done(function (data) {
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
                getErjDocXK(false);
            }
        })
    })


    $('#AddNewErjDocXK').click(function () {
        $("#Result").val('');
        $("#motaghazi").val('');
        $("#companyNameTiket").val(companyName);
        flagSend = false;
        self.AddAttachList([]);

        lastSend = localStorage.getItem("SendTiket");
        if (lastSend != null) {
            const d = new Date();
            let time = d.getTime();

            t = (time - parseInt(lastSend)) / 60000;
            if (t < 1 && lockNumber != "10000") {
                return showNotification('فاصله زمانی بین دو تیکت باید حداقل یک دقیقه باشد', 0);
            }
            else {
                localStorage.removeItem("SendTiket");
            }

        }



        $('#modal-ErjDocXK').modal('show');
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
        if (flagSend == false) {
            natijeh = $("#Result").val();
            motaghazi = $("#motaghazi").val();


            $("#FM_Select").val() == 'M' ? fm_Select = 'آقای ' : fm_Select = 'خانم '



            if (natijeh == '' && self.AddAttachList().length > 0)
                natijeh = 'به پیوست مراجعه شود';

            if (motaghazi == '')
                return showNotification('نام درخواست کننده را وارد کنید', 0);


            if (natijeh == '' && self.AddAttachList().length == 0)
                return showNotification('تیکت خالی است', 0);
            else {
                flagSend = true;
                var ErjSaveTicket_HI = {
                    SerialNumber: 0,
                    DocDate: DateNow,
                    UserCode: 'ESTIRI',
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
                    Motaghazi: fm_Select + motaghazi,
                    IP: ipw,
                    CallProg: 'Web',
                    LoginLink: loginLink,
                    ChatMode: 0
                }
                ajaxFunction(ErjSaveTicketUri, 'POST', ErjSaveTicket_HI).done(function (data) {
                    serialNumber = data;
                });

                for (var i = 0; i <= self.AddAttachList().length - 1; i++) {

                    fileAttach = self.AddAttachList()[i];
                    fileFullName = fileAttach.File.name;
                    fileData = fileFullName.split(".");
                    fileName = fileData[0];
                    fileType = '.' + fileData[1];

                    let result = await ziped(fileType, fileAttach.File, fileFullName);

                };

                const d = new Date();
                let time = d.getTime();

                localStorage.setItem("SendTiket", time);

                showNotification('تیکت ارسال شد', 1);
                getErjDocXK(false);
                $('#modal-ErjDocXK').modal('hide');

            }
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

            ajaxFunctionUploadTiket(ErjDocAttach_SaveUri, formData, false).done(function (response) {

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

    self.ViewChat = function (Band) {
        maxIdMessage = 0;
        $("#box-send").hide();
        $("#chatbox").empty();
        isLast = true;
        refresh(Band.SerialNumber, isLast);

        $("#chat-bell").hide();
        $("#box-chat").show();
        CalcHeight();
        $(".dragandrophandler").scrollTop(1000000);
        clearInterval(timer);
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

        if (self.AddAttachList().length == 0) {
            $('#AddFiles').val('').clone(true);
            $("#AddFiles:hidden").trigger('click');
        }
        // getDocAttachList(serialNumber);

    });


    $('#AddAttachs').click(function () {
        $('#AddFiles').val('').clone(true);
        $("#AddFiles:hidden").trigger('click');
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
                a = document.getElementById("AddFiles").files[0];
                fileFullName = a.name;
                fileData = fileFullName.split(".");
                fileName = fileData[0];
                const att = { id: self.AddAttachList().length, File: a, name: "مدرک پیوست - " + DateNow + " - " + fileName };
                self.AddAttachList.push(att);
            }
        })
    }



    self.DelAddAttach = function (Band) {
        Swal.fire({
            title: 'تایید حذف',
            text: "آیا پیوست انتخابی حذف شودند ؟",
            type: 'warning',
            showCancelButton: true,
            cancelButtonColor: '#3085d6',
            cancelButtonText: 'خیر',
            allowOutsideClick: false,
            confirmButtonColor: '#d33',
            confirmButtonText: 'بله'
        }).then((result) => {
            if (result.value) {
                self.AddAttachList.remove(function (att) {
                    return att.id == Band.id;
                });
            }
        });
    }



    self.selectDocAttach = function (item) {



        Swal.fire({
            title: 'تایید دانلود',
            text: "آیا پیوست انتخابی دانلود شود ؟",
            type: 'warning',
            showCancelButton: true,
            cancelButtonColor: '#3085d6',
            cancelButtonText: 'خیر',
            allowOutsideClick: false,
            confirmButtonColor: '#d33',
            confirmButtonText: 'بله'
        }).then((result) => {
            if (result.value) {
                var fileName = item.FName.split(".");


                var DownloadAttachObject = {
                    ProgName: 'ERJ1',
                    ModeCode: '102',
                    //Group: group_Tiket,
                    Year: '0000',
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
                    IP: ipw,
                    CallProg: 'Web'
                };

                ajaxFunction(ErjDocAttach_DelUri, 'POST', Web_DocAttach_Save).done(function (response) {
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

                    ajaxFunctionUploadTiket(ErjDocAttach_SaveUri, formData, true).done(function (response) {
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
            BandNo: bandNoImput,
            IP: ipw,
            CallProg: 'Web'
        };
        ajaxFunction(ErjDocAttach_DelUri, 'POST', Web_DocAttach_Del).done(function (response) {
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
        return index + 1;
    }


    self.SearchKeyDown = function (viewModel, e) {
        return KeyPressSearch(e);
    }


    setInterval(RefreshTiket, 60000);

    function RefreshTiket() {
        getErjDocXK(false);
    }





};

ko.applyBindings(new ViewModel());

