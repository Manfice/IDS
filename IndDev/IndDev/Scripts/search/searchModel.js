var SearchModule = function() {

    var client = SearchClient();

    var displayMode = {
        view: "VIEW",
        close: "CLOSE"
    };

    var resultModel = {
        SearchRequest: ko.observable(),
        total: ko.observable(),
        view: ko.observable(displayMode.close),
        products: ko.observableArray()
    };

    var searchReq;

    var searchCallback = function(data) {
        resultModel.total(data.Total);
        resultModel.products.removeAll();
        data.SearchItems.forEach(function(item) {
            resultModel.products.push(item);
        });
        resultModel.view(displayMode.view);
    };

    var closeSearchResult = function () {
        resultModel.view(displayMode.close);
    };

    var isSearchResult = function () {
        return resultModel.view()===displayMode.view;
    };

    var submit = function () {
        if (searchReq === resultModel.SearchRequest()) {
            resultModel.view(displayMode.view);
        } else {
            searchReq = resultModel.SearchRequest();
            client.getSearchresult(resultModel, searchCallback);
        }
    };

    var init = function () {
        //ko.applyBindings();
    };

    $(init);

    return {
        submit: submit,
        closeSearchResult: closeSearchResult,
        isSearchResult: isSearchResult,
        resultModel: resultModel
    };
}();