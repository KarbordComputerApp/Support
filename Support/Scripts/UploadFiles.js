var ViewModel = function () {
    var self = this;

    var file1;
    var file2;
    var file3;

    if (lockNumber == null) {
        window.location.href = localStorage.getItem("urlLogin");
    }

    var UploadFilesUri = server + '/api/Data/UploadFile/'; // آدرس فایل
    var FinalUploadFilesUri = server + '/api/Data/FinalUploadFile/'; // آدرس فایل

    tempText = 'فایل ارسالی را انتخاب کنید';

    $("#Index_TextLogo").text('ارسال فایل');

    iconDelete_White = '/Content/img/Icon_White/delete.png';
    iconDelete_Blue = '/Content/img/Icon_Blue/delete.png';

    $('#DelFile1').attr('src', iconDelete_White);
    $('#DelFile2').attr('src', iconDelete_White);
    $('#DelFile3').attr('src', iconDelete_White);

    //$('#DelFile1').attr('disabled', 'disabled');
    //$('#DelFile2').attr('disabled', 'disabled');
    //$('#DelFile3').attr('disabled', 'disabled');

    //$("#DelFile1").hide();
    //$("#DelFile2").hide();
    //$("#DelFile3").hide();


    $("#AddNewFile1").on('click', function (e) {
        e.preventDefault();
        $("#upload1:hidden").trigger('click');
    });

    $("#AddNewFile2").on('click', function (e) {
        e.preventDefault();
        $("#upload2:hidden").trigger('click');
    });

    $("#AddNewFile3").on('click', function (e) {
        e.preventDefault();
        $("#upload3:hidden").trigger('click');
    });

    this.fileUpload1 = function (data, e) {
        file1 = e.target.files[0];
        var name = file1.name;
        $("#NameFile1").text(name);
        $('#DelFile1').attr('src', iconDelete_Blue);
    }


    this.DelFile1 = function (data, e) {
        file1 = null;
        $("#NameFile1").text(tempText);
        $('#DelFile1').attr('src', iconDelete_White);
    }




    this.fileUpload2 = function (data, e) {
        file2 = e.target.files[0];
        var name = file2.name;
        $("#NameFile2").text(name);
        $("#DelFile2").attr('src', iconDelete_Blue);
    }

    this.DelFile2 = function (data, e) {
        file2 = null;
        $("#NameFile2").text(tempText);
        $('#DelFile2').attr('src', iconDelete_White);
    }

    this.fileUpload3 = function (data, e) {
        file3 = e.target.files[0];
        var name = file3.name;
        $("#NameFile3").text(name);
        $("#DelFile3").attr('src', iconDelete_Blue);
    }

    this.DelFile3 = function (data, e) {
        file3 = null;
        $("#NameFile3").text(tempText);
        $('#DelFile3').attr('src', iconDelete_White);
    }






    $("#SendFiles").on('click', function (e) {
        if (file1 == null && file2 == null && file3 == null) {
            return showNotification('فایل ارسالی را انتخاب کنید', 0);
        } 

        desc = $("#comm").val();
        if (desc == '') {
            return showNotification('توضیحات فایل ارسالی را واردی کنید', 0);
        } 


        if (file1 != null) {
            f = document.getElementById("upload1");
            fileFullName = f.files[0].name;
            fileData = fileFullName.split(".");
            fileName = fileData[0];
            fileType = '.' + fileData[1];
            var fileData = new File([file1], fileFullName, { type: fileType });
            var formData = new FormData();
            formData.append("Atch", fileData);
            formData.append("LockNumber", lockNumber);
            ajaxFunctionUpload(UploadFilesUri, formData, false).done(function (response) {
                file1 = null;
                $("#NameFile1").text(tempText);
                $('#DelFile1').attr('src', iconDelete_White);
            });
        }

        if ($("#NameFile1").text() != tempText) {
            return showNotification('خطا در ارسال فایل 1 ', 0);
        }

        if (file2 != null) {
            f = document.getElementById("upload2");
            fileFullName = f.files[0].name;
            fileData = fileFullName.split(".");
            fileName = fileData[0];
            fileType = '.' + fileData[1];
            var fileData = new File([file2], fileFullName, { type: fileType });
            var formData = new FormData();
            formData.append("Atch", fileData);
            formData.append("LockNumber", lockNumber);
            ajaxFunctionUpload(UploadFilesUri, formData, false).done(function (response) {
                file2 = null;
                $("#NameFile2").text(tempText);
                $('#DelFile2').attr('src', iconDelete_White);
            })
        }

        if ($("#NameFile2").text() != tempText) {
            return showNotification('خطا در ارسال فایل 2 ', 0);
        }

        if (file3 != null) {
            f = document.getElementById("upload3");
            fileFullName = f.files[0].name;
            fileData = fileFullName.split(".");
            fileName = fileData[0];
            fileType = '.' + fileData[1];
            var fileData = new File([file3], fileFullName, { type: fileType });
            var formData = new FormData();
            formData.append("Atch", fileData);
            formData.append("LockNumber", lockNumber);
            ajaxFunctionUpload(UploadFilesUri, formData, false).done(function (response) {
                file3 = null;
                $("#NameFile3").text(tempText);
                $('#DelFile3').attr('src', iconDelete_White);
            })
        }

        if ($("#NameFile3").text() != tempText) {
            return showNotification('خطا در ارسال فایل 3 ', 0);
        }



        var FinalUploadFileObject = {
            LockNumber: lockNumber,
            Desc: desc
        }
        ajaxFunction(FinalUploadFilesUri, 'POST', FinalUploadFileObject, true).done(function (data) {
            $("#comm").val('');

            if ($("#NameFile1").text() == tempText && $("#NameFile2").text() == tempText && $("#NameFile3").text() == tempText) {
                return showNotification('بارگذاری با موفقیت انجام شد', 1);
            }

            


            

        });



    });


};


ko.applyBindings(new ViewModel());




