var ViewModel = function () {
    $("#Index_TextLogo").text('تغییر رمز ورود به پنل کاربری');

    $("#Index_Home").show();
    $("#BM_InformationCompany").show();
    $("#BM_ChangePassword").show();

    changePass = localStorage.getItem("ForceToChangePass");

    if (changePass == '1') {
        $("#Index_Home").hide();
        $("#BM_InformationCompany").hide();
        $("#BM_ChangePassword").hide();
    }

    $('#B_ChangePassword').click(function (e) {
        var oldPass = $('#oldPass').val();
        var newPass1 = $('#newPass1').val();
        var newPass2 = $('#newPass2').val();

        if (oldPass == '') {
            return showNotification('کلمه عبور قدیم را وارد کنید',0)
        }

        if (newPass1 == '') {
            return showNotification('کلمه عبور جدید را وارد کنید', 0)
        }

        if (newPass2 == '') {
            return showNotification('تکرار کلمه عبور جدید را وارد کنید', 0)
        }


        val = [false, false, false, false]

        var lowerCaseLetters = /[a-z]/g;
        if (newPass1.length < 8) {
            return showNotification('پسورد جدید باید حداقل 8 کاراکتر باشد', 0);
        }

        

        if (newPass1.match(lowerCaseLetters) == null) {
            val[0] = true;
            return showNotification('پسورد جدید باید شامل کاراکتر کوچک باشد', 0);
        }

        var upperCaseLetters = /[A-Z]/g
        if (newPass1.match(upperCaseLetters) == null) {
            val[1] = true;
            return showNotification('پسورد جدید باید شامل کاراکتر بزرگ باشد', 0);
        }


        var numbers = /[0-9]/g;
        if (newPass1.match(numbers) == null) {
            val[2] = true;
            return showNotification('پسورد جدید باید شامل اعداد باشد', 0);
        }

        if (newPass1 != newPass2) {
            val[3] = true;
            return showNotification('کلمه عبور جدید را درست تکرار کنید', 0);
        }


       // if (val[0] && val[1] && val[2] && val[3]) {

            ChangePasswordUri = server + '/api/Data/ChangePassword/';

            var ChangePasswordObject = {
                LockNumber: lockNumber,
                OldPass: oldPass,
                NewPass: newPass1
            }

            ajaxFunction(ChangePasswordUri, 'POST', ChangePasswordObject, false).done(function (data) {
                if (data == 1) {
                    localStorage.removeItem("ForceToChangePass");
                    showNotification('تغییر کلمه عبور انجام شد', 1);
                    window.location.href = localStorage.getItem("urlLogin");
                }
                else {
                    showNotification('تغییر کلمه عبور انجام نشد ، کلمه عبور قدیم را بررسی کنید', 0);
                }

            });
        //}
    })


};


ko.applyBindings(new ViewModel());


$(".showHidePass").click(function () {
    var id = this.id;
    if (id == "toggle_oldPass") {
        var input = $("#oldPass").attr("type");
        if (input == 'text')
            $("#oldPass").attr('type', 'password')
        else
            $("#oldPass").attr('type', 'text');
    }

    if (id == "toggle_newPass1") {
        var input = $("#newPass1").attr("type");
        if (input == 'text')
            $("#newPass1").attr('type', 'password')
        else
            $("#newPass1").attr('type', 'text');
    }

    if (id == "toggle_newPass2") {
        var input = $("#newPass2").attr("type");
        if (input == 'text')
            $("#newPass2").attr('type', 'password')
        else
            $("#newPass2").attr('type', 'text');
    }

})

