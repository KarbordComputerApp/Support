var ViewModel = function () {
    var self = this;
    var VideosUri = server + '/api/Data/Videos/';
    var LogVideosUri = server + '/api/Data/LogVideos/';
    self.VideosList = ko.observableArray([]);
    self.filter = ko.observable("");
    self.filterGru = ko.observable("");

    if (lockNumber == null) {
        window.location.href = localStorage.getItem("urlLogin");
    }

    $("#F_Video").attr("src", "");
    var Gru;

    $("#Index_TextLogo").text('ویدیوهای آموزشی');

    function getVideosList() {
        var VideosObject = {
            LockNumber: lockNumber,
            FlagLog: true,
            IP: ipw,
            CallProg: 'Web'
        }
        ajaxFunction(VideosUri, 'POST', VideosObject, true).done(function (data) {
            self.VideosList(data == null ? [] : data);
            Gru = [...new Set(data.map(item => item.Title))];

            var textExc = '';

            textExc = '<select class="form-control" style="height: 37px;" id="VideosSelect">';
            textExc += '<option value="">تمام بخش ها</option>';

            for (var i = 0; i < Gru.length; i++) {
                textExc += '<option value="' + Gru[i] + '"' +
                    '>' + Gru[i] + '</option>';
            }

            textExc += '</select>';
            $("#SelectVideos").empty();
            $('#SelectVideos').append(textExc);

        });

    }
    getVideosList();


    self.FilterVideos = ko.computed(function () {
        var filter = self.filter();
        var filterGru = self.filterGru();
        if (!filter && !filterGru) {
            return self.VideosList();
        } else {
            tempData = ko.utils.arrayFilter(self.VideosList(), function (item) {
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

    $("#VideosSelect").change(function () {
        select = $("#VideosSelect").val();
        self.filterGru(select);
    })

    var videoclip = document.getElementById('videoclip');
    var videosource = document.getElementById('mp4video');
    self.ShowVideo = function (band) {
        $("#P_Aparat").hide();
        $("#videoclip").hide();
        $("#Title_Video").text(band.Title + ' - ' + band.Description);
        src = band.Link;
        isAparat = false;

        if (src.search("aparat.com") > 0) {
            isAparat = true;
            $("#P_Aparat").show();
        }
        else {
            $("#videoclip").show();
        }


        var LogVideosObject = {
            LockNumber: lockNumber,
            IP: ipw,
            CallProg: 'Web',
            Spec: band.Description
        }
        ajaxFunction(LogVideosUri, 'POST', LogVideosObject, true).done(function (data) {
            if (isAparat) {
                $("#F_Video").attr("src", band.Link);
            }
            else {
                videoclip.pause();
                videosource.setAttribute('src', band.Link);
                videoclip.load();
                videoclip.play();
            }

            $("#modal-Video").modal('show');
        });

    }

    $('#modal-Video').on('hide.bs.modal', function () {
        $("#F_Video").attr("src", "");
        videoclip.pause();
        videoclip.currentTime = 0
    });
};


ko.applyBindings(new ViewModel());




