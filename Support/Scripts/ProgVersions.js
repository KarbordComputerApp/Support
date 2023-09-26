var ViewModel = function () {
    var self = this;
    var ProgVersionsUri = server + '/api/Data/ProgVersions/K@rbordWeb1234';
    self.ProgVersionsList = ko.observableArray([]);
    self.filter = ko.observable("");
    self.filterGru = ko.observable("");

    //http://localhost:52798/Home/ProgVersions

    $(".continerHead").hide();

    var Gru;

    self.reverseString = function (str)  {
        var splitString = str.split("/");
        var reverseArray = splitString.reverse();
        var joinArray = reverseArray.join("/");
        return joinArray; 
    }

    self.ReplaceSpec = function (str) {
        return str.replaceAll('<br>' , ' ');
    }


    $("#Index_TextLogo").text('تغییرات برنامه ها');

    function getProgVersionsList() {
        ajaxFunction(ProgVersionsUri, 'GET').done(function (data) {
            if (data.length > 10) {
                list = JSON.parse(data);
            }
            self.ProgVersionsList(list == null ? [] : list);
            Gru = [...new Set(list.map(item => item.Name))];

            var textExc = '';

            textExc = '<select class="form-control" style="height: 37px;" id="ProgVersionsSelect">';
            textExc += '<option value="">تمام بخش ها</option>';

            for (var i = 0; i < Gru.length; i++) {
                textExc += '<option value="' + Gru[i] + '"' +
                    '>' + Gru[i] + '</option>';
            }

            textExc += '</select>';
            $("#SelectProgVersions").empty();
            $('#SelectProgVersions').append(textExc);

        });

    }
    getProgVersionsList();


    self.FilterProgVersions = ko.computed(function () {
        var filter = self.filter();
        var filterGru = self.filterGru();
        if (!filter && !filterGru) {
            return self.ProgVersionsList();
        } else {
            tempData = ko.utils.arrayFilter(self.ProgVersionsList(), function (item) {
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

    $("#ProgVersionsSelect").change(function () {
        select = $("#ProgVersionsSelect").val();
        self.filterGru(select);
    })
};


ko.applyBindings(new ViewModel());




