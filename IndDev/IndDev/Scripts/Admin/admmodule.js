var Admin = function() {
    var client = adminClient();

    var viewmodel = {
        currtab: ko.observable("COMPANY")
    };

    var companys = ko.observableArray();

    var currCompany = ko.observable();

    //var company = function(data) {
    //    this.data = {};
    //    this.data.id = ko.observable(data.Id);
    //    this.data.CompanyName = ko.observable(data.CompanyName);
    //    this.data.UrAdress = ko.observable(data.UrAdress);
    //    this.data.RealAdress = ko.observable(data.RealAdress);
    //    this.data.Inn = ko.observable(data.Inn);
    //    this.data.Kpp = ko.observable(data.Kpp);
    //    this.data.Ogrn = ko.observable(data.Ogrn);
    //    this.data.Offer = ko.observable(data.Offer);
    //    this.data.Director = ko.observable(data.Director);
    //    this.data.Buh = ko.observable(data.Buh);
    //    this.data.Region = ko.observable(data.Region);
    //    this.data.CompDirect = ko.observable(data.CompDirect);
    //    this.data.Descr = ko.observable(data.Descr);
    //    this.data.Banks = ko.observableArray();
    //    this.data.Telephones = ko.observableArray();
    //    this.data.PersonContacts = ko.observableArray();
    //};
    var company = function (data) {
        this.id = ko.observable(data.Id);
        this.CompanyName = ko.observable(data.CompanyName);
        this.UrAdress = ko.observable(data.UrAdress);
        this.RealAdress = ko.observable(data.RealAdress);
        this.Inn = ko.observable(data.Inn);
        this.Kpp = ko.observable(data.Kpp);
        this.Ogrn = ko.observable(data.Ogrn);
        this.Offer = ko.observable(data.Offer);
        this.Director = ko.observable(data.Director);
        this.Buh = ko.observable(data.Buh);
        this.Region = ko.observable(data.Region);
        this.CompDirect = ko.observable(data.CompDirect);
        this.Descr = ko.observable(data.Descr);
        this.Banks = ko.observableArray();
        this.Telephones = ko.observableArray();
        this.PersonContacts = ko.observableArray();
    };
    var telData = function(phone, mode) {
        this.Id = ko.observable(phone.Id);
        this.PhoneNumber = ko.observable(phone.PhoneNumber);
        this.Title = ko.observable(phone.Title);
        this.mode = ko.observable(mode);
    }
    var setView = function(data) {
        viewmodel.currtab(data);
    };

    var currView = function(data) {
        return viewmodel.currtab() === data;
    };
    var isNewCompany = function() {
        return currCompany.Id === 0;
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
        Telephones: ko.observableArray(),
        PersonContacts: ko.observableArray()
    };
    var displayMode = {
        view: "VIEW",
        edit: "EDIT"
    };
    var addCompany = function() {
        viewmodel.currtab("EDIT");
        var cp = new company(newCompany);
        var tel = { Id: 0, PhoneNumber: null, Title: "Новый" };
        var tell = new telData(tel, displayMode.edit);
        var pers = { Id: 0, PersonName: ko.observable(), Email: ko.observable("Test@test"), Phone: ko.observable("Test") };
        cp.Telephones.push(tell);
        cp.PersonContacts.push(pers);
        currCompany(cp);
        console.log(ko.toJSON(currCompany));
    };
    var editCompany = function(comp) {
        viewmodel.currtab("EDIT");
        var cp = comp;
        var tel = { Id: 0, PhoneNumber: null, Title: "Новый" };
        var tell = new telData(tel, displayMode.edit);
        var pers = { Id: 0, PersonName: ko.observable(), Email: ko.observable("Test@test"), Phone: ko.observable("Test") };
        cp.Telephones.push(tell);
        cp.PersonContacts.push(pers);
        currCompany(cp);
    };
    var retrieveCompanysCallback = function(data) {
        data.forEach(function (item) {
            console.log(ko.toJSON(item));
            var compData = new company(item);
            item.Telephones.forEach(function(phone) {
                compData.Telephones.push(new telData(phone, displayMode.view));
            });
            item.Banks.forEach(function(bank) {
                compData.Banks.push(bank);
            });
            item.PersonContacts.forEach(function(pers) {
                compData.PersonContacts.push(pers);
            });
            companys.push(compData);

        });
    };
    var saveCompanyCallback = function(result, id) {
        result.Id(id);
        companys.push(result);
    };
    var updCompCallback = function(result, id) {
        alert("Изменения сохранены");
    }
    var postCompany = function() {
        client.updateCompany(currCompany, saveCompanyCallback);
    };
    var putCompany = function() {
        client.updateCompany(currCompany,updCompCallback);
    }
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
        currCompany: currCompany,
        editCompany: editCompany,
        displayMode:displayMode,
        postCompany:postCompany,
        putCompany:putCompany,
        isNewCompany:isNewCompany
    };
}();