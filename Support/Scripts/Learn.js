var ViewModel = function () {
    var self = this;
    var LearnUri = server + '/api/Data/Learn/';

    if (lockNumber == null) {
        window.location.href = localStorage.getItem("urlLogin");
    }

    $("#Index_TextLogo").text('آموزش');

};


ko.applyBindings(new ViewModel());




