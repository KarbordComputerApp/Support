var ViewModel = function () {
    $("#Index_TextLogo").text('اطلاعات کاربری');

    idUser = localStorage.getItem("IdUser");
    $("#Name").val(localStorage.getItem("Name"));
    $("#Tel").val(localStorage.getItem("Tel"));
    $("#Mobile").val(localStorage.getItem("Mobile"));
    $("#EMail").val(localStorage.getItem("Email"));
    $("#Address").val(localStorage.getItem("Address"));
    image = localStorage.getItem("Pic");

    if (image != "null" && image != "undefined") {
        var picUrl = base64Url(image);
        imageUser.src = picUrl;
    }

    var noImage = '/Content/img/user.png';

    //$("#Pic").val(localStorage.getItem("Pic"));

    var addImage = false;
    var file = null;
    var delImage = false;

    $("#AddImage").on('click', function (e) {
        e.preventDefault();
        $("#upload:hidden").trigger('click');
    });

    $("#DelImage").on('click', function (e) {
        file = null;
        delImage = true;
        imageUser.src = noImage;
    });

    this.fileUpload = function (data, e) {
        file = e.target.files[0];

        if (file) {
            addImage = true;
            imageUser.src = URL.createObjectURL(file);
        }
    }


    $("#save").click(function () {
        ChangeUserUri = server + '/api/Data/ChangeUser/';

        var ChangeUserObject = {
            IdUser: idUser,
            Name: $("#Name").val(),
            Tel: $("#Tel").val(),
            Mobile: $("#Mobile").val(),
            Address: $("#Address").val(),
            Email: $("#EMail").val(),
            DelImage: delImage
        }

        if (delImage == true) {
            localStorage.setItem("Pic", null);
        }

        ajaxFunction(ChangeUserUri, 'POST', ChangeUserObject, false).done(function (data) {

            if (file != null && addImage == true) {
                size = file.size;
                fileFullName = file.name;

                var formData = new FormData();

                formData.append("IdUser", idUser);
                formData.append("Atch", file);
                var SaveUserImageUri = server + '/api/Data/SaveUserImage/'; // ذخیره عکس

                ajaxFunctionUpload(SaveUserImageUri , formData, false).done(function (response) {
                    localStorage.setItem("Pic", response);
                })
            }

            localStorage.setItem("Email", ChangeUserObject.Email);
            localStorage.setItem("Name", ChangeUserObject.Name);
            localStorage.setItem("Tel", ChangeUserObject.Tel);
            localStorage.setItem("Mobile", ChangeUserObject.Mobile);
            localStorage.setItem("Address", ChangeUserObject.Address);
            localStorage.setItem("Address", ChangeUserObject.Address);
            window.location.href = localStorage.getItem("urlIndex");
        });


      /*  UserUri = server + '/api/Data/User/';
        var UserObject = {
            LockNumber: lockNumber,
            Pass: localStorage.getItem("pass"),
        }

        ajaxFunction(UserUri, 'POST', UserObject, false).done(function (data) {
            if (data.length > 0) {
                localStorage.setItem("Email", data[0].Email);
                localStorage.setItem("Name", data[0].Name);
                localStorage.setItem("Tel", data[0].Tel);
                localStorage.setItem("Mobile", data[0].Mobile);
                localStorage.setItem("Address", data[0].Address);
            }
            else {
                return showNotification('شماره قفل یا کلمه عبور اشتباه است', 0);
            }
        });*/
    })

};


ko.applyBindings(new ViewModel());