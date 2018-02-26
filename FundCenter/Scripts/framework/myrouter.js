define(['director','routes','jquery'], function (director, routes) {
    var changeRoute = function () {
        require(['page'], function (page) {
            page.changePage(router.getRoute());
        })
    }
    var changeToErrorPage = function () {
        require(['page'], function (page) {
            page.changePage('error404');
        })
    }

    var localRoute = {};
    $.each(routes, function (idx, obj) {
        if (idx.indexOf('*') != -1) {
            localRoute[idx] = changeToErrorPage;
        } else {
            localRoute[idx] = changeRoute;
        }
    })

    var router = new Router(localRoute);
    router.init();
})