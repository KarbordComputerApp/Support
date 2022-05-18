var ViewModel = function () {

    self.Recovery = function LoginUser() {
        email = $("#email").val();
        if (email == "" || email == null) {
            return showNotification('ایمیل خود را وارد کنید', 0);
        }
       /* LoginUri = server + '/api/Data/Login/';

        var LoginObject = {
            LockNumber: lockNumber,
            Pass: pass
        }

        ajaxFunction(LoginUri, 'POST', LoginObject).done(function (data) {
            if (data.length > 0) {
                localStorage.setItem("lockNumber", lockNumber);
                window.location.href = localStorage.getItem("urlIndex");
            }
            else {
                return showNotification('شماره قفل یا کلمه عبور اشتباه است', 0);
            }
        });*/
    }

};


ko.applyBindings(new ViewModel());




