var Admin = function() {
    var client = adminClient();

    var viewmodel = {
        currtab: ko.observable("COMPANY")
    };

    var companys = ko.observableArray();

    var currCompany = ko.observable();

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
        this.data.Region = ko.observable(data.Region);
        this.data.CompDirect = ko.observable(data.CompDirect);
        this.data.Descr = ko.observable(data.Descr);
        this.data.Banks = ko.observableArray();
        this.data.Telephones = ko.observableArray();
    }
    var setView = function(data) {
        viewmodel.currtab(data);
    };

    var currView = function(data) {
        return viewmodel.currtab() === data;
    };

    var newCompany = {
        CompanyName: "Новый контрагент",
        UrAdress: null,
        RealAdress: null,
        Inn: null,
        Kpp: null,
        Ogrn: null,
        Offer: null,
        Director: null,
        Buh: null,
        Region: null,
        CompDirect: null,
        Descr: null,
        Banks: ko.observableArray(),
        Telephones: ko.observableArray()
    };
    var addCompany = function() {
        viewmodel.currtab('EDIT');
        var cp = new company(newCompany);
        var tel = { Id: 0, PhoneNumber: ko.observable(), Title: ko.observable("Test") };
        cp.data.Telephones.push(tel);
        currCompany(cp);
        console.log(ko.toJSON(currCompany));
    }
    var retrieveCompanysCallback = function(data) {
        data.forEach(function (item) {
            console.log(ko.toJSON(item));
            var compData = new company(item);
            item.Telephones.forEach(function(phone) {
                compData.data.Telephones.push(phone);
            });
            item.Banks.forEach(function(bank) {
                compData.data.Banks.push(bank);
            });
            companys.push(compData);

        });
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
        currView: currView,
        companys: companys,
        addCompany:addCompany,
        currCompany:currCompany
    };
}();