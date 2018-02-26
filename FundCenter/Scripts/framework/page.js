define(['knockout', 'app'], function (ko, app) {
    var page = {
    };
    page.temp = ko.observable({name:'',data:''});
    page.name = ko.observable();
    page.data = ko.observable();

    page.changePage = function (pageName) {
        sessionStorage.setItem("pageName", pageName);
        require([pageName + '-js'], function (pageData) {
            page.temp({ name: pageName + '-html', data: pageData });
            //page.name(pageName + '-html');
            //page.data(pageData);
        });
    }

    page.app = ko.observable(app);

    return page;
})