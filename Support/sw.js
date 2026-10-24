self.addEventListener("push", function (event) {

    var data = {
        title: "Test Push",
        message: "پیام جدید دریافت شد"
    };

    if (event.data) {
        try {
            data = event.data.json();
        } catch (e) {
            data.message = event.data.text();
        }
    }

    event.waitUntil(
        self.registration.showNotification(data.title, {
            body: data.message,
            icon: "/Content/images/notification.png",
            badge: "/Content/images/notification.png",
            dir: "rtl",
            lang: "fa-IR"
        })
    );
});


self.addEventListener("notificationclick", function (event) {

    event.notification.close();

    event.waitUntil(
        clients.matchAll({
            type: "window",
            includeUncontrolled: true
        }).then(function (clientList) {

            for (var i = 0; i < clientList.length; i++) {
                if ("focus" in clientList[i]) {
                    return clientList[i].focus();
                }
            }

            return clients.openWindow("/");
        })
    );
});