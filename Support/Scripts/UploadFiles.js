var ViewModel = function () {
    var self = this;
    var UploadFilesUri = server + '/api/Data/UploadFile/'; // آدرس فایل


    $("#AddNewFile").on('click', function (e) {
        e.preventDefault();
        $("#upload:hidden").trigger('click');
    });

    this.fileUpload = function (data, e) {
        var file = e.target.files[0];
        var name = file.name;
        var size = file.size;
        var file1 = document.getElementById("upload");
        fileFullName = file1.files[0].name;
        fileData = fileFullName.split(".");
        fileName = fileData[0];
        fileType = '.' + fileData[1];

        var fileData = new File([file], fileFullName, { type: fileType });

        var formData = new FormData();
        formData.append("Atch", fileData);
        formData.append("LockNumber", lockNumber);

        ajaxFunctionUpload(UploadFilesUri, formData, true).done(function (response) {

        })
    }


};


ko.applyBindings(new ViewModel());




