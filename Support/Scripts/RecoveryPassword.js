var ViewModel = function () {

    self.Recovery = function RecoveryPasswordUser() {
        email = $("#email").val();
        if (email == "" || email == null) {
            return showNotification('ایمیل خود را وارد کنید', 0);
        }

        RecoveryPasswordUri = server + '/api/Data/RecoveryPassword/';
        ipw = localStorage.getItem("IPW");
        lockNumber = localStorage.getItem("lockNumber");

        var RecoveryPasswordObject = {
            LockNumber: lockNumber,
            Email: email,
            IP: ipw,
            CallProg: 'Web'
        }

        ajaxFunction(RecoveryPasswordUri, 'POST', RecoveryPasswordObject).done(function (data) {
            if (data == 1) {
                //localStorage.setItem("lockNumber", lockNumber);
                showNotification('کلمه ورود جدید به ایمیل شما ارسال شد ', 1);
                setTimeout(() => {
                    window.location.href = localStorage.getItem("urlLogin");
                }, 3000);
               
            }
            else if (data == 0) {
                return showNotification('شماره قفل یا ایمیل اشتباه است', 0);
            }
            else {
                return showNotification('خطا در ارسال ایمیل ' + '<br>' +  data, 0);
            }
        });
    }

};


ko.applyBindings(new ViewModel());




