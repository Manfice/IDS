var Admin = function() {
    var client = adminClient();

    var viewmodel = {
        currtab: ko.observable("COMPANY")
    };

    var companys = ko.observableArray();
    var company = function(data) {
        this.data = {};
        this.data.id = ko.observable(data.Id);
        this.data.CompanyName = ko.observable(data.CompanyName);
        this.data.UrAdress = ko.observable(data.UrAdress);
        this.data.RealAdress = ko.observable(data.RealAdress);
        this.data.Inn = ko.observable(data.Inn);
        this.data.Kpp = ko.observable(data.Kpp);
        this.data.Ogrn = ko.observable(data.Ogrn);
        this.data.Offer = ko.observable(data.Offer);
        this.data.Director = ko.observable(data.Director);
        this.data.Buh = ko.observable(data.Buh);
        this.data.Banks = ko.observableArray();
        this.data.Telephones = ko.observableArray();
    }
    var setView = function(data) {
        viewmodel.currtab(data);
    };

    var currView = function(data) {
        return viewmodel.currtab() === data;
    };

    var retrieveCompanysCallback = function(data) {
        data.forEach(function(item) {
            companys.push(new company(item));
        });
        console.log(ko.toJSON(companys));
    };

    var retrieveCompanys = function() {
        console.log("Retrieving companys data...");
        client.getCompanys(retrieveCompanysCallback);
    };

    var init = function() {
        retrieveCompanys();
    };
    $(init);
    return{
        setView: setView,
        currView: currView
    };
}();