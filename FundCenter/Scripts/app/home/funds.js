define(["knockout","chart"], function (ko) {
    function FundViewModel() {
        var self = this;
        self.searchFundNumber = ko.observable();
        self.searchFundResult = ko.observable(null);
        self.funds = ko.observableArray([]);

        self.afterRender = function () {
            console.log("fund after render");
        };
        self.searchFund = function () {
            var searchFundNumber = self.searchFundNumber();
            if (searchFundNumber > 0 && searchFundNumber.length == 6) {
                $.ajax({
                    url: '/Funds/SearchFundByFundNumber',
                    dataType: "json",
                    type: "GET",
                    contentType: "application/json",
                    data: {"searchFundNumber": searchFundNumber}
                })
                .done(function (result) {
                    if (!!result) {
                        var searchFundResult = {};
                        searchFundResult.FundNumber = ko.observable(result.FundNumber);
                        searchFundResult.FundName = ko.observable(result.FundName);
                        searchFundResult.CurrentChangeValue = ko.observable(result.CurrentChangeValue + '%');
                        searchFundResult.LastRefreshTime = ko.observable(result.LastRefreshTime);
                        searchFundResult.OwnedValue = ko.observable("");

                        self.searchFundResult(searchFundResult);
                    }
                    //To do nothing.
                })
                .fail(function (xhr, status) {
                    //console.log("Initial Transaction Step Error : " + xhr.message);
                });
            }
        }

        self.addFundInList = function () {
            $.ajax({
                url: '/Funds/AddFundInList',
                dataType: "json",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({ "fundViewModel": ko.mapping.toJS(self.searchFundResult()) })
            })
            .done(function (result) {
                var funds = self.funds();
                funds.push(self.searchFundResult());
                self.searchFundResult(null);
                self.funds(funds);
                self.searchFundNumber("");
            })
            .fail(function (xhr, status) {
                //console.log("Initial Transaction Step Error : " + xhr.message);
            });
        }

        self.getFunds = function () {
            $.ajax({
                url: '/Funds/GetFunds',
                dataType: "json",
                type: "GET",
            })
            .done(function (result) {
                var funds = [];
                $.each(result, function (key,value) {
                    var searchFundResult = {};
                    searchFundResult.FundNumber = ko.observable(value.FundNumber);
                    searchFundResult.FundName = ko.observable(value.FundName);
                    searchFundResult.CurrentChangeValue = ko.observable(value.CurrentChangeValue + '%');
                    searchFundResult.LastRefreshTime = ko.observable(value.LastRefreshTime);
                    searchFundResult.OwnedValue = ko.observable(value.OwnedValue);
                    funds.push(searchFundResult);
                })
                self.funds(funds);
            })
            .fail(function () {

            })
        }

        self.showHistory = function (data) {
            $.ajax({
                url: '/Funds/GetFundHistory',
                dataType: "json",
                type: "GET",
                contentType: "application/json",
                data: { "fundNumber": data.FundNumber() }
            })
            .done(function (result) {
                var timeList = [];
                var valueList = [];
                $.each(result, function (key, value) {
                    timeList.push(value.LastRefreshTime);
                    valueList.push(value.CurrentChangeValue);
                });
                var ctx = document.getElementById('myChart').getContext('2d');
                var chart = new Chart(ctx, {
                    // The type of chart we want to create
                    type: 'line',

                    // The data for our dataset
                    data: {
                        labels: timeList,
                        datasets: [{
                            label: "My First dataset",
                            backgroundColor: 'rgb(255, 99, 132)',
                            borderColor: 'rgb(255, 99, 132)',
                            data: valueList,
                        }]
                    },

                    // Configuration options go here
                    options: {}
                });
            })
            .fail(function () {

            })
        }

        setTimeout(function () {
            self.getFunds();
        },1000)
    }

    return new FundViewModel();
})