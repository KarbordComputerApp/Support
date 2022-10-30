var ViewModel = function () {
    $("#Index_TextLogo").text('مشخصات شرکت');

    $("#LName").val(companyName);
    $("#EMail").val(localStorage.getItem("Email"));


       
};


ko.applyBindings(new ViewModel());