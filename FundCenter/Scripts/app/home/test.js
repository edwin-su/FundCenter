define(['knockout'], function (ko) {
    function TestModel() {
        var self = this;
        self.userName = ko.observable("edwin");
        self.age = ko.observable(25);
        self.isTrue = ko.observable(false);
        self.returnUserName = ko.observable();
        self.afterRender = function () {
            console.log("test after render");
            $.ajax({
                url: '/Funds/GetTestResult',
                dataType: "json",
                type: "GET",
                contentType: "application/json",
                data: { "userName": "edwin" }
            })
            .done(function (result) {
                if (!!result) {
                    self.isTrue(true);
                    self.returnUserName(result);
                }
                //To do nothing.
            })
            .fail(function (xhr, status) {
                //console.log("Initial Transaction Step Error : " + xhr.message);
            });
        }
        

    }

    return new TestModel();
})