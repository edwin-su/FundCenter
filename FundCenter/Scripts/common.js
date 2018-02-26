var paths = {
    'jquery': 'lib/jquery',
    'director': 'lib/director',
    'knockout-amd-helpers': 'lib/knockout-amd-helpers',
    'knockout': 'lib/knockout',
    'text': 'lib/text',
    'mapping': 'lib/knockout.mapping-latest',
    'chart':'lib/chart',

    'page': 'framework/page',
    'router': 'framework/myrouter',
    'routes': 'framework/routes',

    'app': 'app/app',
    'error404-js': 'app/share/error404',
    'error404-html': '../templates/app/share/error404.html',
    'login-js': 'app/account/login',
    'login-html': '../templates/app/account/login.html',
    'signup-js': 'app/account/signup',
    'signup-html': '../templates/app/account/signup.html',
    'dashboard-html': '../templates/app/home/dashboard.html',
    'dashboard-js': 'app/home/dashboard',
    'funds-html': '../templates/app/home/funds.html',
    'funds-js': 'app/home/funds',
    'test-html': '../templates/app/home/test.html',
    'test-js':'app/home/test',
}

var baseUrl = '../';

require.config({
    baeuUrl: baseUrl,
    paths: paths,
    shim: {
        '': {
            exports:''
        }
    },
    deps: ['knockout', 'mapping'],
callback: function (ko, mapping) {
    ko.mapping = mapping;
}
});

console.log("start");

require(['knockout', 'page', 'text', 'knockout-amd-helpers', 'router'], function (ko,page) {
    ko.applyBindings(page);
    var pageName = sessionStorage.getItem("pageName");
    if (!!pageName) {
        location.href = '#/' + pageName;
    } else {
        location.href = '#/dashboard';
    }
})