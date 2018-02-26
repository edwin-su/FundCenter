define(['knockout', 'jquery'], function (ko) {
    var app = {};
    app.logOut = function () {
        $.ajax({
            url: "/Account/LogOut",
            dataType: "json",
            type: "GET",
            contentType: "application/json",
            data: { userId: sessionStorage.getItem("userId") }
        })
        .done(function () {
            location.href = "/Account/Login";
            //To do nothing.
        })
        .fail(function (xhr, status) {
            //console.log("Initial Transaction Step Error : " + xhr.message);
        });
    }

    return app;
})