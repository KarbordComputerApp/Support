var ViewModel = function () {

    self.LoginUser = function LoginUser() {
        lockNumber = $("#user").val();
        pass = $("#pass").val();
        if (lockNumber === "" || lockNumber === null) {
            return showNotification('شماره قفل را وارد کنید', 0);
        }
        LoginUri = server + '/api/Data/Login/'; 

        var LoginObject = {
            LockNumber: lockNumber,
            Pass : pass
        }

        ajaxFunction(LoginUri, 'POST', LoginObject).done(function (data) {
            if (data.length > 0) {
                localStorage.setItem("lockNumber", lockNumber);
                localStorage.setItem("FirstName", data[0].FirstName);
                localStorage.setItem("LastName", data[0].LastName);
                localStorage.setItem("Email", data[0].Email);
                window.location.href = localStorage.getItem("urlIndex");
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




