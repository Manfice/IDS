var Shop = function () {
    var client = new ShopClient();
    /*models*/
    var showModel = {
        searchTab:ko.observable(false),
        toolsTab: ko.observable(false),
        topOffer:ko.observable(true)
    };

    var topMenus = ko.observableArray([]);
    var products = ko.observableArray([]);
    var brands = ko.observableArray([]);

    var topMenuData = function(data) {
        var self = this;
        self.Id = ko.observable(data.Id);
        self.Title = ko.observable(data.Title);
        self.Picture = ko.observable(data.Picture);
        self.Priority = ko.observable(data.Priority);
    };
    var productData = function(data) {
        var self = this;
        self.Id = ko.observable(data.Id);
        self.Art = ko.observable(data.Art);
        self.Title = ko.observable(data.Title);
        self.Avatar = ko.observable(data.Avatar);
        self.Price = ko.observable(data.Price);
        self.Rate = ko.observable(data.Rate);
    }
    var brandData = function(data) {
        var self = this;
        self.Id = ko.observable(data.Id);
        self.Path = ko.observable(data.Path);
    }
    /*callbacks*/
    var catCallback = function (data) {
        var cats = [];
        data.forEach(function(item) {
            cats.push(new topMenuData(item));
        });
        topMenus(cats);
        //console.log(ko.toJS(mShop.topMenu));
    }
    var retCallback = function (data) {
        var pr = [];
        data.forEach(function(item) {
            pr.push(new productData(item));
        });
        products(pr);
    };
    var brandsCallback = function(data) {
        var br = [];
        data.forEach(function(item) {
            br.push(new brandData(item));
        });
        brands(br);
    }
    /*functions*/
    var setSearch = function () {
        var p = showModel.searchTab();
        showModel.searchTab(!p);
        if (showModel.toolsTab()) {
            showModel.toolsTab(false);
        }
    }
    var setTool = function () {
        var p = showModel.toolsTab();
        showModel.toolsTab(!p);
        if (showModel.searchTab()) {
            showModel.searchTab(false);
        }
    }
    var hideTopOffer = function() {
        showModel.topOffer(false);
    }
    var retreiveCat = function() {
        client.getMenuItems(catCallback);
    }
    var retrieveProducts = function() {
        client.getTopRetails(retCallback);
    }
    var retrieveBrands = function() {
        client.getBrands(brandsCallback);
    }
    /*return & init*/
    var init = function () {
        retreiveCat();
        retrieveProducts();
        retrieveBrands();
        ko.applyBindings(Shop, document.getElementById("megashop"));
    };
    $(init);
    return {
        showModel: showModel,topMenus:topMenus,
        setSearch: setSearch, setTool: setTool, products: products,brands:brands
    }
}();