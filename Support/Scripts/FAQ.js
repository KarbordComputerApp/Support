var ViewModel = function () {
    var self = this;
    var FAGUri = server + '/api/Data/FAG/';
    self.FAGList = ko.observableArray([]); // ليست فاکتور
    self.filter = ko.observable("");

    $("#Index_TextLogo").text('سوالات متداول');

    function getFAGList() {
        ajaxFunction(FAGUri, 'GET', true).done(function (data) {
            self.FAGList(data == null ? [] : data);
        });

    }
    getFAGList();

    
    self.FilterFag = ko.computed(function () {
        var filter = self.filter();
        if (!filter) {
            return self.FAGList();
        } else {
            tempData = ko.utils.arrayFilter(self.FAGList(), function (item) {
                result =
                    (item.Description == null ? '' : item.Description.toString().search(filter) >= 0)
                return result;
            })
            return tempData;
        }
    })
};


ko.applyBindings(new ViewModel());




