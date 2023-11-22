$("#box-chat").hide();
$("#chat-bell").show();
$("#chat-bell-image-wrapper").click(function () {
    $("#chat-bell").hide();
    $("#box-chat").show();
});

$("#btn-close-chat").click(function () {
    $("#chat-bell").show();
    $("#box-chat").hide();
});

