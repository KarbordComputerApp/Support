"use strict";
function getNoUISliderValue(e, i) {
    e.noUiSlider.on("update", function () {
        var t = e.noUiSlider.get();
        i && (t = parseInt(t), t += "%"),
            $(e).parent().find("span.js-nouislider-value").text(t)
    })
} function initBasicSelect() {
    $("select").formSelect(),
        $("#sel").formSelect();
    $.each([{ id: 1, name: "Option 1" },
    { id: 2, name: "Option 2" },
    { id: 3, name: "Option 3" }],
        function (e, i) {
            $("#sel").append("<option value='" + i.id + "'>" + i.name + "</option>"),
                $("#sel").formSelect()
        })
} function initGroupSelect() {
    $("#sel-grp").formSelect();
    $("#sel-grp").append("<optgroup label='Picnic'>"),
        $.each([{ id: 1, name: "Option 1" },
        { id: 2, name: "Option 2" },
        { id: 3, name: "Option 3" }],
            function (e, i) {
                $("#sel-grp").append("<option value='" + i.id + "'>" + i.name + "</option>"),
                    $("#sel-grp").formSelect()
            }),
        $("#sel-grp").append("</optgroup >"),
        $("#sel-grp").append("<optgroup label='Picnic1'>"),
        $.each([{ id: 4, name: "Option 11" },
        {
            id: 5, name: "Option 21"
        },
        { id: 6, name: "Option 31" }],
            function (e, i) {
                $("#sel-grp").append("<option value='" + i.id + "'>" + i.name + "</option>"), $("#sel-grp").formSelect()
            }),
        $("#sel-grp").append("</optgroup >")
} function initMultiSelect() {
    $("#multisel").formSelect();
    $.each([{ id: 1, name: "Option 1" },
    { id: 2, name: "Option 2" },
    { id: 3, name: "Option 3" }],
        function (e, i) {
            $("#multisel").append("<option value='" + i.id + "'>" + i.name + "</option>"),
                $("#multisel").formSelect()
        })
} function initImageSelect() {
    $("#imgsel").formSelect();
    $.each([{
        id: 1, name: "Option 1", url: "../../assets/images/user/user1.jpg"
    },
    {
        id: 2, name: "Option 2", url: "../../assets/images/user/user2.jpg"
    },
    {
        id: 3, name: "Option 3", url: "../../assets/images/user/user3.jpg"
    }], function (e, i) {
        $("#imgsel").append("<option value='" + i.id + "' data-icon='" + i.url + "'>" + i.name + "</option>"), $("#imgsel").formSelect()
    })
} $(function () {
    autosize($("textarea.auto-growth")),

        $("input#input_text, textarea#textarea2").characterCounter(),
       // $(".colorpicker").colorpicker(),
       // initBasicSelect(),
      //  initGroupSelect(),
      //  initMultiSelect(),
      //  initImageSelect(),
        Dropzone.options.frmFileUpload = {
            paramName: "file", maxFilesize: 2
        };
    var e = $(".demo-masked-input");
    e.find(".date").inputmask("dd/mm/yyyy", { placeholder: "__/__/____" }),
        e.find(".time12").inputmask("hh:mm t", { placeholder: "__:__ _m", alias: "time12", hourFormat: "12" }),
        e.find(".time24").inputmask("hh:mm", { placeholder: "__:__ _m", alias: "time24", hourFormat: "24" }),
        e.find(".datetime").inputmask("d/m/y h:s", { placeholder: "__/__/____ __:__", alias: "datetime", hourFormat: "24" }),
        e.find(".mobile-phone-number").inputmask("+99 (999) 999-99-99", { placeholder: "+__ (___) ___-__-__" }),
        e.find(".phone-number").inputmask("+99 (999) 999-99-99", { placeholder: "+__ (___) ___-__-__" }),
        e.find(".money-dollar").inputmask("99,99 $",
            { placeholder: "__,__ $" }),
        e.find(".money-euro").inputmask("99,99 €",
            { placeholder: "__,__ €" }),
        e.find(".ip").inputmask("999.999.999.999",
            { placeholder: "___.___.___.___" }),
        e.find(".credit-card").inputmask("9999 9999 9999 9999", { placeholder: "____ ____ ____ ____" }),
        e.find(".email").inputmask({ alias: "email" }),
        e.find(".key").inputmask("****-****-****-****",
            { placeholder: "____-____-____-____" }),
        $("#optgroup").multiSelect({ selectableOptgroup: !0 })
});