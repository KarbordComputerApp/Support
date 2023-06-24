var ViewModel = function () {
    var self = this;
    var FAQUri = server + '/api/Data/FAQ/';
    self.FAQList = ko.observableArray([]);
    self.filter = ko.observable("");
    self.filterGru = ko.observable("");

    if (lockNumber == null) {
        window.location.href = localStorage.getItem("urlLogin");
    }

    var Gru;

    $("#Index_TextLogo").text('سوالات متداول');

    function getFAQList() {
        var FAQObject = {
            LockNumber: lockNumber,
            FlagLog: true,
            IP: ipw,
            CallProg: 'Web'
        }
        ajaxFunction(FAQUri, 'POST', FAQObject, true).done(function (data) {
            self.FAQList(data == null ? [] : data);
            Gru = [...new Set(data.map(item => item.Title))];

            var textExc = '';

            textExc = '<select class="form-control" style="height: 37px;" id="FAQSelect">';
            textExc += '<option value="">تمام بخش ها</option>';

            for (var i = 0; i < Gru.length; i++) {
                textExc += '<option value="' + Gru[i] + '"' +
                    '>' + Gru[i] + '</option>';
            }

            textExc += '</select>';
            $("#SelectFAQ").empty();
            $('#SelectFAQ').append(textExc);

        });

    }
    getFAQList();


    self.FilterFAQ = ko.computed(function () {
        var filter = self.filter();
        var filterGru = self.filterGru();
        if (!filter && !filterGru) {
            return self.FAQList();
        } else {
            tempData = ko.utils.arrayFilter(self.FAQList(), function (item) {
                result =
                    (item.Title == null ? '' : item.Title.toString().search(filterGru) >= 0) &&

                    (

                        (item.Description == null ? '' : item.Description.toString().search(filter) >= 0) ||
                        (item.Body == null ? '' : item.Body.toString().search(filter) >= 0)
                    )
                return result;
            })
            return tempData;
        }
    })

    $("#FAQSelect").change(function () {
        select = $("#FAQSelect").val();
        self.filterGru(select);
    })
};


ko.applyBindings(new ViewModel());




