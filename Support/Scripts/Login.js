var ViewModel = function () {

    getIP();

    self.LoginUser = function LoginUser() {
        localStorage.removeItem("ForceToChangePass");

        lockNumber = $("#user").val();
        pass = $("#pass").val();
        if (lockNumber === "" || lockNumber === null) {
            return showNotification('شماره قفل را وارد کنید', 0);
        }

        ipw = localStorage.getItem("IPW");

        LoginUri = server + '/api/Data/Login/';

        var LoginObject = {
            LockNumber: lockNumber,
            Pass: pass,
            IP: ipw,
            CallProg: 'Web'
        }

        ajaxFunction(LoginUri, 'POST', LoginObject, false).done(function (data) {
            if (data.length > 0) {
                localStorage.setItem("IdUser", data[0].Id);
                localStorage.setItem("lockNumber", lockNumber);
                localStorage.setItem("FirstName", data[0].FirstName);
                localStorage.setItem("LastName", data[0].LastName);
                localStorage.setItem("Email", data[0].Email);
                localStorage.setItem("UserType", data[0].UserType);
                localStorage.setItem("ForceToChangePass", data[0].ForceToChangePass);
                localStorage.setItem("TrsDownload", data[0].TrsDownload);

                localStorage.setItem("Name", data[0].Name);
                localStorage.setItem("Tel", data[0].Tel);
                localStorage.setItem("Mobile", data[0].Mobile);
                localStorage.setItem("Address", data[0].Address);
                localStorage.setItem("Pic", data[0].Pic);


                localStorage.removeItem("UsersChat");
                GetRepFromUsers();

                getDataChat();

                var LockNumbersObject = {
                    LockNumber: lockNumber
                }

                LockNumbersUri = server + '/api/Data/LockNumbers/';
                ajaxFunction(LockNumbersUri, 'POST', LockNumbersObject, false).done(function (dataLock) {
                    if (dataLock.length > 0) {
                        cName = dataLock[0].CompanyName.split("-");
                        localStorage.setItem("CompanyName", cName[0]);
                        if (data[0].ForceToChangePass == 1) {
                            localStorage.setItem("ForceToChangePass", '1');
                            window.location.href = localStorage.getItem("urlChangePassword");
                        } else {
                            window.location.href = localStorage.getItem("urlIndex");
                        }
                    }
                })


            }
            else {
                return showNotification('شماره قفل یا کلمه عبور اشتباه است', 0);
            }
        });
    }

    self.RecoveryPassword = function LoginUser() {
        lockNumber = $("#user").val();

        if (lockNumber === "" || lockNumber === null) {
            return showNotification('شماره قفل را وارد کنید', 0);
        }

        localStorage.setItem("lockNumber", lockNumber);
        window.location.href = localStorage.getItem("urlRecoveryPassword");

    }

    $('#user').keyup(function (e) {
        if (e.which == 13) {
            $('#pass').focus()
        }
    })

    $('#pass').keyup(function (e) {
        if (e.which == 13) {
            self.LoginUser();
        }
    })





};


ko.applyBindings(new ViewModel());




$(".showHidePass").click(function () {
    var id = this.id;
    if (id == "toggle_Pass") {
        var input = $("#pass").attr("type");
        if (input == 'text')
            $("#pass").attr('type', 'password')
        else
            $("#pass").attr('type', 'text');
    }
})
