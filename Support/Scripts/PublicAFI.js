var server = 'http://localhost:52798';
//var server = 'http://192.168.0.114:1000';

var lockNumber = localStorage.getItem("lockNumber");
var firstName = localStorage.getItem("FirstName");
var lastName = localStorage.getItem("LastName");
var userType = localStorage.getItem("UserType");
var forceToChangePass = localStorage.getItem("ForceToChangePass");
var fullName = firstName + ' ' + lastName;
var companyName = localStorage.getItem("CompanyName");


var ticketUser = 'Ticket';
var ticketPass = '@!B912';

var custAccountUser = 'CustAccount';
var custAccountPass = '@!B913';

var ipw = localStorage.getItem("IPW");


$("#B_CustAccount").attr('disabled', 'disabled');
$("#B_Tiket").attr('disabled', 'disabled');
$("#Index_TextTiket").attr('disabled', 'disabled');


if (userType == '1' && forceToChangePass == 'false') {
    $('#B_CustAccount').removeAttr('disabled');
}

if ((userType == '1' || userType == '2')) {
    $('#B_Tiket').removeAttr('disabled');
    $('#Index_TextTiket').removeAttr('disabled');
}



var Web_CountErjDocXKUri = server + '/api/KarbordData/Web_CountErjDocXK/'; // تعداد تیکت خوانده نشده
function GetCountErjDocXK() {
    if (lockNumber != "" && lockNumber != null) {
        var Object_CountErjDocXK = {
            LockNo: lockNumber,
            ModeCode: '204',
        }
        ajaxFunction(Web_CountErjDocXKUri, 'POST', Object_CountErjDocXK, true).done(function (data) {

            count = data[0];
            $("#notificationCount").text(data[0]);
            localStorage.setItem("notificationCount", data[0]);

            if (count == 0) {
                $("#notificationCount").hide();
            } else {
                $("#notificationCount").show();
            }
        });
    }
}

setInterval(RefreshCountErjDocXK, 60000);

function RefreshCountErjDocXK() {
    GetCountErjDocXK();
}

RefreshCountErjDocXK();
/*
var notificationCount = localStorage.getItem("notificationCount");
if (notificationCount != "null" && notificationCount != null) {
    $("#notificationCount").text(notificationCount);
    if (notificationCount == "0") {
        $("#notificationCount").hide();
    }
} else {
}
*/











function ajaxFunction(uri, method, data, sync, error) {
    return $.ajax({
        type: method,
        async: sync == null ? false : sync,
        encoding: 'UTF-8',
        beforeSend: function () {
            if (sync == true) {
                $('#loadingsite').attr('class', 'page-proccess-wrapper');
                $('#loadingsite').css('display', 'block');
            }
        },
        url: uri,
        dataType: 'json',

        cache: false,
        timeout: 300000,
        //onLoading: showLoad(),
        headers: {
            'userName': ticketUser,
            'password': ticketPass,
            'userKarbord': sessionStorage.userName,
        },
        complete: function () {
            var n = uri.search("ChangeDatabase");
            if (sync == true && n == -1) {
                $('#loadingsite').css('display', 'none');
                $('#loadingsite').attr('class', 'page-loader-wrapper');
            }
        },
        contentType: 'application/json',
        data: data ? JSON.stringify(data) : null
    }).fail(function (jqXHR, textStatus, errorThrown) {
        error != false ?
            showNotification('اشکال در دریافت اطلاعات از سرور . لطفا عملیات را دوباره انجام دهید' + '</br>' + textStatus + ' : ' + errorThrown, 3)
            : null;
    });
}

function ajaxFunctionUpload(uri, data, sync) {

    return $.ajax({
        url: uri,
        type: 'POST',
        data: data,
        cache: false,
        contentType: false,
        processData: false,

        async: sync == null ? false : sync,
        beforeSend: function () {
            if (sync == true) {
                $('#loadingsite').attr('class', 'page-proccess-wrapper');
                $('#loadingsite').css('display', 'block');
            }
        },

        headers: {
            'userName': 0,
            'password': 0,
            'userKarbord': sessionStorage.userName,
        },
        success: function (fileName) {
        },
        complete: function () {
            var n = uri.search("ChangeDatabase");
            if (sync == true && n == -1) {
                $('#loadingsite').css('display', 'none');
                $('#loadingsite').attr('class', 'page-loader-wrapper');
            }
        },
        xhr: function () {
            var fileXhr = $.ajaxSettings.xhr();
            if (fileXhr.upload) {
                $("progress").show();
                fileXhr.upload.addEventListener("progress", function (e) {
                    if (e.lengthComputable) {
                        $("#fileProgress").attr({
                            value: e.loaded,
                            max: e.total
                        });
                    }
                }, false);
            }
            return fileXhr;
        }
    });
}



function ajaxFunctionUploadTiket(uri, data, sync) {

    return $.ajax({
        url: uri,
        type: 'POST',
        data: data,
        cache: false,
        contentType: false,
        processData: false,

        async: sync == null ? false : sync,
        beforeSend: function () {
            if (sync == true) {
                $('#loadingsite').attr('class', 'page-proccess-wrapper');
                $('#loadingsite').css('display', 'block');
            }
        },

        headers: {
            'userName': ticketUser,
            'password': ticketPass,
            'userKarbord': sessionStorage.userName,
        },
        success: function (fileName) {
        },
        complete: function () {
            var n = uri.search("ChangeDatabase");
            if (sync == true && n == -1) {
                $('#loadingsite').css('display', 'none');
                $('#loadingsite').attr('class', 'page-loader-wrapper');
            }
        },
        xhr: function () {
            var fileXhr = $.ajaxSettings.xhr();
            if (fileXhr.upload) {
                $("progress").show();
                fileXhr.upload.addEventListener("progress", function (e) {
                    if (e.lengthComputable) {
                        $("#fileProgress").attr({
                            value: e.loaded,
                            max: e.total
                        });
                    }
                }, false);
            }
            return fileXhr;
        }
    });
}



function ajaxFunctionAccount(uri, method, sync, data) {
    return $.ajax({
        type: method,
        url: uri,
        dataType: 'json',
        async: sync == null ? false : sync,
        beforeSend: function () {
            if (sync == true) {
                $('#loadingsite').attr('class', 'page-proccess-wrapper');
                $('#loadingsite').css('display', 'block');
            }
        },
        cache: false,
        timeout: 30000,
        complete: function () {
            if (sync == true) {
                $('#loadingsite').css('display', 'none');
                $('#loadingsite').attr('class', 'page-loader-wrapper');
            }
        },
        contentType: 'application/json',
        data: data ? JSON.stringify(data) : null
    }).fail(function (jqXHR, textStatus, errorThrown) {
        showNotification('اشکال در دریافت اطلاعات از سرور . لطفا عملیات را دوباره انجام دهید' + '</br>' + textStatus + ' : ' + errorThrown, 3);
    });
}



function sleep(milliseconds) {
    var start = new Date().getTime();
    for (var i = 0; i < 1e7; i++) {
        if ((new Date().getTime() - start) > milliseconds) {
            break;
        }
    }
}


function base64ToArrayBuffer(base64) {
    var binaryString = window.atob(base64);
    var binaryLen = binaryString.length;
    var bytes = new Uint8Array(binaryLen);
    for (var i = 0; i < binaryLen; i++) {
        var ascii = binaryString.charCodeAt(i);
        bytes[i] = ascii;
    }
    return bytes;
}

function saveByteArray(reportName, byte) {
    var blob = new Blob([byte], { type: 'octet/stream' });
    var link = document.createElement('a');
    link.href = window.URL.createObjectURL(blob);
    var fileName = reportName;
    link.download = fileName;
    link.click();
};



function showNotification(text, colorNumber, From, Align, time) {

    placementFrom = From == null ? sessionStorage.placementFrom : From;
    placementAlign = Align == null ? sessionStorage.placementAlign : Align;
    animateEnter = sessionStorage.animateEnter;
    animateExit = sessionStorage.animateExit;
    if (colorNumber == 0)
        colorName = 'alert-danger';
    else if (colorNumber == 1)
        colorName = 'alert-success';
    else if (colorNumber == 2)
        colorName = 'alert-warning';
    else if (colorNumber == 3)
        colorName = 'alert-info';


    if (colorName === null || colorName === '') { colorName = 'bg-black'; }
    if (text === null || text === '') { text = 'خطای برنامه نویسی : متن هشدار وارد نشده است'; }
    if (animateEnter === null || animateEnter === '') { animateEnter = 'animated fadeInDown'; }
    if (animateExit === null || animateExit === '') { animateExit = 'animated fadeOutUp'; }
    var allowDismiss = true;

    $.notify({
        message: text
    },
        {
            type: colorName,
            allow_dismiss: allowDismiss,
            newest_on_top: true,
            timer: time = null ? 1000 : time,
            placement: {
                from: placementFrom,
                align: placementAlign
            },
            animate: {
                enter: animateEnter,
                exit: animateExit
            },

            template: '<div data-notify="container" style="z-index:20000 !important" class="bootstrap-notify-container alert alert-dismissible {0} ' + (allowDismiss ? "p-r-35" : "") + '" role="alert">' +
                '<button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>' +
                '<span data-notify="icon"></span> ' +
                '<span data-notify="title">{1}</span> ' +
                '<span data-notify="message">{2}</span>' +
                '<div class="progress" data-notify="progressbar">' +
                '<div class="progress-bar progress-bar-{0}" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>' +
                '</div>' +
                '<a href="{3}" target="{4}" data-notify="url"></a>' +
                '</div>'
        });
}


/*

$("#Home").click(function () {
    window.location.href = localStorage.getItem("urlIndex");
});

$("#Close").click(function () {
    window.location.href = localStorage.getItem("urlLogin");
});

*/





var viewer = null;
var designer = null;
var options = null;
var report = null;
var dataSet = null;

function createViewer() {
    Stimulsoft.Base.Localization.StiLocalization.addLocalizationFile("/Content/Report/Lang/fa.xml", true, "persion (fa)");
    Stimulsoft.Base.StiFontCollection.addOpentypeFontFile("/Content/fonts/BZiba.ttf", "Karbord_Ziba");
    Stimulsoft.Base.StiFontCollection.addOpentypeFontFile("/Content/fonts/BZAR.ttf", "Karbord_ZAR");
    Stimulsoft.Base.StiFontCollection.addOpentypeFontFile("/Content/fonts/BYEKAN.ttf", "Karbord_YEKAN");
    Stimulsoft.Base.StiFontCollection.addOpentypeFontFile("/Content/fonts/BTITRBD.ttf", "Karbord_TITRBD");
    Stimulsoft.Base.StiFontCollection.addOpentypeFontFile("/Content/fonts/BNAZANIN.ttf", "Karbord_NAZANIN");

    options = new Stimulsoft.Viewer.StiViewerOptions();
    viewer = new Stimulsoft.Viewer.StiViewer(options, "StiViewer", false);

    options.appearance.showSystemFonts = false;
    options.height = "100%";
    options.appearance.fullScreenMode = true;
    options.appearance.scrollbarsMode = true;
    options.toolbar.showSaveButton = true;

    if (lockNumber == 10011 || lockNumber == 10071 || lockNumber == 10000) {
        options.toolbar.showDesignButton = true;
        $('#DesignPrint').attr('style', 'display: unset');
    } else {
        options.toolbar.showDesignButton = false;
        $('#DesignPrint').attr('style', 'display: none');
    }


    options.toolbar.showFullScreenButton = false;
    options.toolbar.printDestination = Stimulsoft.Viewer.StiPrintDestination.Direct;
    options.appearance.htmlRenderMode = Stimulsoft.Report.Export.StiHtmlExportMode.Table;
    options.toolbar.zoom = 100;
    options.toolbar.showCloseButton = true;

    report = new Stimulsoft.Report.StiReport();
    viewer.onDesignReport = function (e) {
        createDesigner();
    };
    viewer.renderHtml("viewerContent");

    var userButton = viewer.jsObject.SmallButton("userButton", "خروج");

    userButton.action = function () {
        $("#modal-Report").modal('hide');
    }

    var toolbarTable = viewer.jsObject.controls.toolbar.firstChild.firstChild;
    var buttonsTable = toolbarTable.rows[0].firstChild.firstChild;
    var userButtonCell = buttonsTable.rows[0].insertCell(0);
    userButtonCell.className = "stiJsViewerClearAllStyles";
    userButtonCell.appendChild(userButton);
}

var DataReport;
function createDesigner() {
    viewer.visible = false;
    designer = null;
    var options = new Stimulsoft.Designer.StiDesignerOptions();
    options.appearance.fullScreenMode = true;
    options.appearance.htmlRenderMode = Stimulsoft.Report.Export.StiHtmlExportMode.Table;

    designer = new Stimulsoft.Designer.StiDesigner(options, "StiDesigner", false);
    designer.renderHtml("designerContent");

    designer.onExit = function (e) {
        this.visible = false;
        viewer.visible = false;
        $("#modal-Report").modal('hide');
    }

    /*designer.onSaveReport = function (e) {
        var jsonStr = e.report.saveToJsonString();
       // SavePrintForm(sessionStorage.ModePrint, e.fileName, jsonStr);
    }

    designer.onSaveAsReport = function (e) {
        var jsonStr = e.report.saveToJsonString();
        var name = e.fileName;
        resTestSavePrintForm = "";
        //SavePrintForm(sessionStorage.ModePrint, e.fileName, jsonStr);
    };*/

    //report._reportFile = printName == null ? 'فرم چاپ' : printName;
    designer.report = report;
    designer.visible = true;

}




function setReport(reportObject, addressMrt, variablesObject) {
    DataReport = reportObject;
    if (DataReport.length == 0 || DataReport == null || DataReport == "") {
        return showNotification('فاکتور بدون بند', 0);
    }

    var dStart = new Date();
    var secondsStart = dStart.getTime();

    report = new Stimulsoft.Report.StiReport();
    report.loadFile(addressMrt);

    report.dictionary.databases.clear();
    dataSet = new Stimulsoft.System.Data.DataSet("Database");
    DataReport = '{"Data":' + JSON.stringify(DataReport) + '}';

    dataSet.readJson(DataReport);
    report.regData(dataSet.dataSetName, "", dataSet);

    variablesDataSet = new Stimulsoft.System.Data.DataSet("variables");
    variablesReport = '{"variables":[{' + variablesObject + '}]}';
    variablesDataSet.readJson(variablesReport);
    report.regData(variablesDataSet.dataSetName, "", variablesDataSet);




    // titlesDataSet = new Stimulsoft.System.Data.DataSet("Titles");
    // titlesReport = '{"Titles":[{' + titlesObject + '}]}';
    // titlesDataSet.readJson(titlesReport);
    // report.regData(titlesDataSet.dataSetName, "", titlesDataSet);


    report.dictionary.synchronize();

    viewer.report = report;
    //report.render();

    viewer.visible = true;
    $('#modal-Report').modal('show');

    viewer.onExit = function (e) {
        this.visible = false;
    }

}

$("#LogOut").click(function () {

    localStorage.removeItem("lockNumber");
    localStorage.removeItem("FirstName");
    localStorage.removeItem("LastName");
    localStorage.removeItem("Email");
    localStorage.removeItem("UserType");

    window.location.href = localStorage.getItem("urlLogin");

});


function showPass(id) {
    var x = document.getElementById(id);
    if (x.type === "password") {
        x.type = "text";
    } else {
        x.type = "password";
    }
}


function getIP() {
    localStorage.setItem("IPW", "Error Get IP");
    sessionStorage.IPW = "Error Get IP";
    ipw = "Error Get IP";

    ajaxFunctionAccount('http://ip-api.com/json/', 'GET').done(function (data) {
        //a = sessionStorage.MacAddress;
        //b = sessionStorage.IP4Address;
        localStorage.setItem("IPW", data.query);
        //localStorage.setItem("CountryLogin", data.country);
        //localStorage.setItem("CityLogin", data.city);

        sessionStorage.IPW = data.query;
        ipw = data.query;

        //sessionStorage.CountryLogin = data.country
        //sessionStorage.CityLogin = data.city
    });
}


var testFile = false;
function TestDownload(mode, lockNo, id) {
    var Download_TestUri = server + '/api/Data/Download_Test/'; // آدرس  تست دانلود
    ajaxFunction(Download_TestUri + mode + "/" + lockNo + '/' + id, 'GET', false).done(function (data) {
        if (data == "NotFound") {
            testFile = false;
            return showNotification('فایل یافت نشد', 0);
        }
        else {
            testFile = true;
        }
    })
}




function base64ToArrayBuffer(base64) {
    var binaryString = window.atob(base64);
    var binaryLen = binaryString.length;
    var bytes = new Uint8Array(binaryLen);
    for (var i = 0; i < binaryLen; i++) {
        var ascii = binaryString.charCodeAt(i);
        bytes[i] = ascii;
    }
    return bytes;
}


function base64Url(data) {
    var bytes = base64ToArrayBuffer(data);
    var blob = new Blob([bytes.buffer], { type: 'image/png' });
    return URL.createObjectURL(blob);
}

