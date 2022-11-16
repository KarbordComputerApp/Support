var ViewModel = function () {
    var self = this;

    $("#Index_TextLogo").text('ارتباط با بخش فروش');

    self.BoxList = ko.observableArray([]); // لیست ارتباط با بخش فروش ها  


    var DateUri = server + '/api/Data/GetDate/'; // آدرس تاریخ سرور
    var BoxUri = server + '/api/Data/MailBox/'; // آدرس ارتباط با بخش فروش ها
    var DownloadUri = server + '/api/Data/DownloadFileMailBox/'; // دانلود 
    var InsertMailBoxUri = server + '/api/Data/InsertMailBox/'; // افزودن ارتباط با بخش فروش 
    var DownloadFileMailBoxUri = server + '/api/Data/DownloadFileMailBox/'; // دانلود پیوست ارتباط با بخش فروش 

    var DeleteFileUri = server + '/api/Data/DeleteFileMailBox/'; // حذف پیوست ارتباط با بخش فروش 
    var DeleteBoxUri = server + '/api/Data/DeleteMailBox/'; // حذف  ارتباط با بخش فروش 

    var ReadMailBoxUri = server + '/api/Data/ReadMailBox/'; // دیدن پیام 

    //var DateNow = new Date().toLocaleDateString('fa-IR');
    var DateNow = '';

    //Get DocAttach List
    function getDate() {
        ajaxFunction(DateUri, 'GET', false).done(function (data) {
            DateNow = data[0];
        });
    }

    getDate();

    getBoxList(true)
    //Get Box List
    function getBoxList(log) {
        var BoxObject = {
            LockNumber: lockNumber,
            Mode: $("#BoxMode").val(),
            UserCode: sessionStorage.userName,
            FlagLog:log
        }
        ajaxFunction(BoxUri, 'POST', BoxObject).done(function (data) {
            self.BoxList(data == null ? [] : data);
        });
    }


    $("#BoxMode").change(function () {
        getBoxList(false);
    });


    function getBoxAttach(fileName) {
        ajaxFunction(DownloadUri + lockNumber + '/' + fileName, 'GET').done(function (data) {
            a = data;
        });
    }


    $('#refreshBox').click(function () {

        Swal.fire({
            title:  'تایید به روز رسانی',
            text: "لیست ارتباط با بخش فروش به روز رسانی شود ؟",
            type: 'info',
            showCancelButton: true,
            cancelButtonColor: '#3085d6',
            cancelButtonText: 'خیر',
            allowOutsideClick: false,
            confirmButtonColor: '#d33',
            confirmButtonText: 'بله'
        }).then((result) => {
            if (result.value) {
                getBoxList(false);
            }
        })
    })


    self.radif = function (index) {
        return index + 1 ;
    }


    self.ViewBox = function (item) {
        $('#titleBox').val(item.title);
        $('#bodyBox').val(item.body);
        $('#panel_Action').attr('hidden', '');
        $('.fix').attr('class', 'form-line focused fix');

        var ReadBoxObject = {
            Id: item.id,
            ReadSt : 'Y'
        }

        ajaxFunction(ReadMailBoxUri, 'POST', ReadBoxObject).done(function (data) {
            $('#modal-Box').modal('show');
        });
    }


    self.DeleteBox = function (item) {
        Swal.fire({
            title: 'تایید حذف',
            text: "آیا ارتباط با بخش فروش انتخابی حذف شود ؟",
            type: 'warning',
            showCancelButton: true,
            cancelButtonColor: '#3085d6',
            cancelButtonText: text_No,

            confirmButtonColor: '#d33',
            confirmButtonText: text_Yes
        }).then((result) => {
            if (result.value) {
                if (item.namefile != '') {
                    ajaxFunction(DeleteFileUri + '/' + lockNumber + '/' + item.namefile, 'GET', true).done(function (data) {
                    });
                }

                ajaxFunction(DeleteBoxUri + '/' + lockNumber + '/' + item.id, 'GET', true).done(function (data) {
                    getBoxList(false);
                    showNotification('ارتباط با بخش فروش حذف شد', 1);
                });
            }

        })

    }





    $('#AddNewBox').click(function () {
        $('#titleBox').val('');
        $('#bodyBox').val('');
        $('#AddFile').val('');
        $('#nameAttach').val('');
        $('#panel_Action').removeAttr('hidden', '');
        $('#modal-Box').modal('show');
    })


    $('#AddAttach').click(function () {
        $('#nameAttach').val('');
        $('#AddFile').val('').clone(true);
        $("#AddFile:hidden").trigger('click');
    });

    this.fileUpload = function (data, e) {
        var file = document.getElementById("AddFile");

        if (file.files.length > 0) {
            fileFullName = file.files[0].name;
            $('#nameAttach').val(fileFullName);
        }

    };



    //Get DocAttach List
    function getBoxAttach(Id) {

        var DownloadFileMailBoxObject = {
            id: Id
        }

        ajaxFunction(DownloadFileMailBoxUri, 'POST', DownloadFileMailBoxObject).done(function (data) {
            var sampleArr = base64ToArrayBuffer(data[0].Atch);
            saveByteArray(data[0].namefile, sampleArr);
        });
    }

    self.ViewBoxAttach = function (item) {
        getBoxAttach(item.id);

//        addr = DownloadUri + lockNumber + '/' + item.namefile;
     //   window.location.href = addr;

        //getBoxAttach(item.namefile);
    }






    $('#saveBox').click(function () {

        title = $("#titleBox").val();
        body = $("#bodyBox").val();

        if (title == '') {
            return showNotification('عنوان را وارد کنید', 0);
        }

        if (body == '') {
            return showNotification('متن را وارد کنید', 0);
        }

        var file = document.getElementById("AddFile");
        fileFullName = '';

        if (file.files.length > 0) {
            fileFullName = file.files[0].name;
            fileData = fileFullName.split(".");
            fileName = fileData[0];
            fileType = '.' + fileData[1];

            if (file.files[0].size > 5000000) { // بیشتر از 5 مگابایت
                return showNotification('فایل پیوست باید کمتر از 5 مگابایت باشد', 0);
            }

            /*if (fileData[1] == 'exe') {
                return showNotification('ابتدا فایل را فشرده کنید سپس ارسال کنید', 2);
            }*/
        }

        $("#saveBox").hide();

            var formData = new FormData();

            formData.append('mode', 1);
            formData.append('readst', 'N');
            formData.append('lockNumber', lockNumber);
            formData.append('date', DateNow);
            formData.append('title', title);
            formData.append('body', body);
            formData.append('namefile', fileFullName);
            formData.append('Atch', $('#AddFile')[0].files[0]);

            ajaxFunctionUpload(InsertMailBoxUri , formData, true).done(function (data) {
                $('#modal-Box').modal('hide');
                getBoxList(false);
                showNotification('ارتباط با بخش فروش ارسال شد', 1);
                $("#saveBox").show();
            });

    });

    
    self.PageIndexBox = function (item) {
        return CountPage(self.filterBoxList(), self.pageSizeBox(), item);
    };
};

ko.applyBindings(new ViewModel());

