var ViewModel = function () {
    var self = this;

    $("#Index_TextLogo").text('ارتباط با بخش فروش');

    self.BoxList = ko.observableArray([]); // لیست ارتباط با بخش فروش ها  


    var BoxUri = server + '/api/Data/MailBox/'; // آدرس ارتباط با بخش فروش ها
    var DownloadUri = server + '/api/Data/DownloadFileMailBox/'; // دانلود 
    var AddBoxUri = server + '/api/Data/InsertMailBox/'; // افزودن ارتباط با بخش فروش 
    var UploadUri = server + '/api/Data/UploadFileMailBox/'; // افزودن ارتباط با بخش فروش 
    var DeleteFileUri = server + '/api/Data/DeleteFileMailBox/'; // حذف پیوست ارتباط با بخش فروش 
    var DeleteBoxUri = server + '/api/Data/DeleteMailBox/'; // حذف  ارتباط با بخش فروش 

    DateNow = new Date().toLocaleDateString('fa-IR');

    getBoxList()
    //Get Box List
    function getBoxList() {
        var BoxObject = {
            LockNumber: lockNumber,
            Mode: $("#BoxMode").val(),
            UserCode: sessionStorage.userName,
        }
        ajaxFunction(BoxUri, 'POST', BoxObject).done(function (data) {
            self.BoxList(data == null ? [] : data);
        });
    }


    $("#BoxMode").change(function () {
        getBoxList();
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
                getBoxList();
            }
        })
    })


    self.radif = function (index) {
        return index + 1 ;
    }


    self.ViewBox = function (item) {
        $('#titleBox').val(item.title);
        $('#bodyBox').val(item.body);

        /* if (item.mode == 2) { // دریافتی
             $('#panel_Action').attr('hidden', '');
         }
         else {
             $('#panel_Action').removeAttr('hidden', '');
         }*/
        $('#panel_Action').attr('hidden', '');
        $('.fix').attr('class', 'form-line focused fix');
        $('#modal-Box').modal('show');
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
                    getBoxList();
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


    self.ViewBoxAttach = function (item) {

        addr = DownloadUri + lockNumber + '/' + item.namefile;
        window.location.href = addr;

        //getBoxAttach(item.namefile);
    }






    $('#saveBox').click(function () {

        title = $("#titleBox").val();
        body = $("#bodyBox").val();

        var file = document.getElementById("AddFile");
        fileFullName = '';
        if (file.files.length > 0) {
            fileFullName = file.files[0].name;
            fileData = fileFullName.split(".");
            fileName = fileData[0];
            fileType = '.' + fileData[1];

            if (fileData[1] == 'exe') {
                return showNotification('ابتدا فایل را فشرده کنید سپس ارسال کنید', 2);
            }

            var formData = new FormData();
            formData.append('fileName', $('#AddFile')[0].files[0]);

            ajaxFunctionUpload(UploadUri + '/' + lockNumber, formData, false).done(function (data) {
                fileFullName = data;
            });
        }




        var InsertBoxObject = {
            Mode: 1,
            LockNumber: lockNumber,
            Date: DateNow,
            Title: title,
            Body: body,
            NameFile: fileFullName,
            UserCode: sessionStorage.userName,
        }
        ajaxFunction(AddBoxUri, 'POST', InsertBoxObject).done(function (data) {
            $('#modal-Box').modal('hide');
            getBoxList();
            showNotification('ارتباط با بخش فروش ارسال شد', 1);
        });


    });

    
    self.PageIndexBox = function (item) {
        return CountPage(self.filterBoxList(), self.pageSizeBox(), item);
    };
};

ko.applyBindings(new ViewModel());

