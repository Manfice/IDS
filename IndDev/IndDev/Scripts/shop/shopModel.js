﻿var Shop = function () {
    var client = new ShopClient();
    /*models*/
    var showModel = {
        searchTab:ko.observable(false),
        toolsTab: ko.observable(false),
        topOffer:ko.observable(true),
        viewGrid: ko.observable(true)
    };

    var productsModel = {
        products: ko.observableArray([]),
        topMenus:ko.observableArray([]),
        brands: ko.observableArray([]),
        filteredProducts: ko.observableArray([]),
        pagedProducts: ko.observableArray([]),
        currentPage:ko.observable(0),
        pageSize: ko.observable(10),
        pageCount: ko.observable(),
        gotoPage: function(p) {
            productsModel.currentPage(p());
        },
        prevPage: function() {
            var data = productsModel.currentPage();
            if (data >= 0) productsModel.currentPage(data-1);
        },
        nextPage: function () {
            var data = productsModel.currentPage();
            var max = productsModel.pageCount();
            if (data < max) productsModel.currentPage(data + 1);
        },
        setPageSize: function(data) {
            productsModel.pageSize(data);
            var pCount = productsModel.pageCount();
            if (productsModel.currentPage()>pCount) {
                productsModel.currentPage(pCount-1);
            }
        },
        currentCategory: ko.observable(null),
        productGroupe:ko.observable(""),
        categorys: ko.observableArray([]),
        navString:ko.observableArray([])
    }
    productsModel.pageCount = ko.computed(function () {
        return Math.ceil(this.products().length / this.pageSize());
    },productsModel);

    productsModel.pagedProducts = ko.computed(function() {
        var size = productsModel.pageSize();
        var start = productsModel.currentPage() * size;
        return productsModel.products().slice(start,start+size);
    }, productsModel);

    var topMenuData = function(data) {
        var self = this;
        self.Id = ko.observable(data.Id);
        self.Title = ko.observable(data.Title);
        self.Picture = ko.observable(data.Picture);
        self.Priority = ko.observable(data.Priority);
        self.updating = ko.observable(false);
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
    var catData = function(data) {
        var self = this;
        self.Id = ko.observable(data.Id);
        self.Title = ko.observable(data.Title);
        self.Count = ko.observable(data.Count);
        self.Path = ko.observable(data.Path);
        self.updating = ko.observable(false);
    }
    /*callbacks*/
    var catCallback = function (data) {
        var cats = [];
        data.forEach(function(item) {
            cats.push(new topMenuData(item));
        });
        productsModel.topMenus(cats);
        //console.log(ko.toJS(mShop.topMenu));
    }
    var retCallback = function (data) {
        var pr = [];
        data.forEach(function(item) {
            pr.push(new productData(item));
        });
        productsModel.products(pr);
    };
    var brandsCallback = function(data) {
        var br = [];
        data.forEach(function(item) {
            br.push(new brandData(item));
        });
        productsModel.brands(br);
    }
    var retCatCallback = function(data, groupe) {
        productsModel.products([]);
        var c = [];
        var p = [];
        data.forEach(function(item) {
            c.push(new catData(item));
            item.products.forEach(function(product) {
                p.push(new productData(product));
            });
        });
        productsModel.categorys(c);
        productsModel.products(p);
        groupe.updating(false);
    };
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
    var setViewStyle = function(data) {
        showModel.viewGrid(data);
    }
    var setGroupe = function (data) {
        data.updating(true);
        productsModel.productGroupe(data.Title());
        productsModel.categorys([]);
        hideTopOffer();
        client.getCategorys(data, retCatCallback);
    }
    var setCategory = function(data) {
        productsModel.currentCategory(data);
        productsModel.navString.push(data);
    }
    /*return & init*/
    var init = function () {
        retreiveCat();
        retrieveProducts();
        retrieveBrands();
        ko.applyBindings(productsModel, document.getElementById("megashop"));
    };
    $(init);
    return {
        showModel: showModel,
        setSearch: setSearch,
        setTool: setTool,setGroupe:setGroupe,setCategory:setCategory,
        productsModel: productsModel, setViewStyle: setViewStyle
    }
}();