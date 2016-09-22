var Admin = function () {
    var client = adminClient();

    var viewmodel = {
        currtab: ko.observable("COMPANY"),
        showCompanyDetails:ko.observable("CLOSE")
    };
    var companysViewModel = {
        companys: ko.observableArray(),
        regions: ko.observableArray([]),
        curentRegion: ko.observable(null),
        filteredCompanys: ko.observableArray()
    };
    var regionData = function(data, quantity) {
        this.title = data;
        this.quantity = quantity;
    };
    var filterCompanysByRegion = function() {
        var region = companysViewModel.curentRegion();
        companysViewModel.filteredCompanys.removeAll();
        companysViewModel.filteredCompanys.push.apply(companysViewModel.filteredCompanys,
        companysViewModel.companys().filter(function (p) {
            return region === null || p.Region() === region;
        }));
    };

    companysViewModel.companys.subscribe(function(newCompanys) {
        filterCompanysByRegion();
        companysViewModel.regions.removeAll();
        var tempArr = [];
        tempArr.push.apply(tempArr,
            companysViewModel.companys().map(function (p) {
                return p.Region();
            }).filter(function (value, index, self) {
                return self.indexOf(value) === index;
            }).sort());

        tempArr.forEach(function (item) {
            var arr =  companysViewModel.companys().filter(function(p) {
                return p.Region() === item;
            }).length;
            companysViewModel.regions.push(new regionData(item, arr));
        });
    });

    var setFilterRegion = function (region) {
        var r = region === null ? null : region.title;
        companysViewModel.curentRegion(r);
        filterCompanysByRegion();
    };

    var currCompany = ko.observable();

    var company = function (data) {
        this.Id = ko.observable(data.Id);
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
        this.Events = ko.observableArray();
    };

    var telData = function(phone,mode, detId) {
        this.Id = ko.observable(phone.Id);
        this.PhoneNumber = ko.observable(phone.PhoneNumber);
        this.Title = ko.observable(phone.Title);
        this.Details = ko.observable(detId);
        this.mode = ko.observable(mode);
    };

    var canKp = function(pr) {
        return pr !== 0;
    };

    var persData = function(pers, mode) {
        this.Id = ko.observable(pers.Id);
        this.PersonName = ko.observable(pers.PersonName);
        this.Email = ko.observable(pers.Email);
        this.Phone = ko.observable(pers.Phone);
        this.Details = ko.observable(pers.Details.Id);
        this.mode = ko.observable(mode);
        this.kp = canKp(pers.Id);
    };
    var eventData = function(event) {
        this.Id = ko.observable(event.Id);
        this.EventDate = ko.observable(event.EventDate);
        this.EventInit = ko.observable(event.EventInit);
        this.Priority = ko.observable(event.Priority);
        this.RemindMe = ko.observable(event.RemindMe);
        this.Descr = ko.observable(event.Descr);
        this.Meneger = ko.observable(event.Meneger);
        this.Details = ko.observable(event.Details.Id);
    }
    var setView = function(data) {
        viewmodel.currtab(data);
    };

    var currView = function(data) {
        return viewmodel.currtab() === data;
    };

    var canSendKp = function(pers) {
        return pers.Id !== "0";
    };
    var newCompany = {
        Id: 0,
        CompanyName: null,
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
        Banks: ko.observableArray([]),
        Telephones: ko.observableArray([]),
        PersonContacts: ko.observableArray([])
    };
    var displayMode = {
        view: "VIEW",
        edit: "EDIT"
    };
    var addCompany = function() {
        viewmodel.currtab("EDIT");
        var cp = new company(newCompany);
        currCompany(cp);
    };
    var addTell = function(comp) {
        var tel = { Id: 0, PhoneNumber: null, Title: null };
        var tell = new telData(tel, displayMode.edit);
        comp.Telephones.push(tell);
        currCompany(comp);
    };
    var addPerson = function(com) {
        var pers = { Id: 0, PersonName: null, Email: null, Phone: null };
        var prs = new persData(pers, displayMode.edit);
        com.PersonContacts.push(prs);
        currCompany(com);
    };
    var retrievCompany = function (item) {
        var compData = new company(item);
        var detId = item.Id;
        item.Telephones.forEach(function (phone) {
            var phoneD = new telData(phone, displayMode.view, detId);
            compData.Telephones.push(phoneD);
        });
        item.Banks.forEach(function (bank) {
            compData.Banks.push(bank);
        });
        item.PersonContacts.forEach(function (pers) {
            compData.PersonContacts.push(new persData(pers, displayMode.view));
        });
        item.Events.forEach(function (e) {
            compData.Events.push(new eventData(e));
        });
        currCompany(compData);
        companysViewModel.companys.remove(function(i) {
            return i.Id() === compData.Id();
        });
        companysViewModel.companys.push(compData);
        filterCompanysByRegion();
    };
    var updCompCallback = function (result) {
        retrievCompany(result);
    };
    var editCompany = function () {
        viewmodel.currtab("EDIT");
    };
    var viewCompany = function (comp) {
        currCompany(null);
        client.retrieveCompanyFromServer(comp, updCompCallback);
        viewmodel.currtab("SHOW_COMPANY");
    };
    var retrieveCompanysCallback = function (data) {
        var listData = [];
        data.forEach(function (item) {
            var compData = new company(item);
            //item.Telephones.forEach(function(phone) {
            //    compData.Telephones.push(new telData(phone, displayMode.view));
            //});
            //item.Banks.forEach(function(bank) {
            //    compData.Banks.push(bank);
            //});
            //item.PersonContacts.forEach(function(pers) {
            //    compData.PersonContacts.push(new persData(pers,displayMode.view));
            //});
            listData.push(compData);
        });
        companysViewModel.companys(listData);
    };

    var saveCompanyCallback = function(result) {
        retrievCompany(result);
        var compData = new company(result);
        result.Telephones.forEach(function (phone) {
            compData.Telephones.push(new telData(phone, displayMode.view));
        });
        result.Banks.forEach(function (bank) {
            compData.Banks.push(bank);
        });
        result.PersonContacts.forEach(function (pers) {
            compData.PersonContacts.push(new persData(pers, displayMode.view));
        });
        companysViewModel.companys.unshift(compData);
    };

    var postCompany = function() {
        client.updateCompany(currCompany, saveCompanyCallback);
    };
    var putCompany = function() {
        client.updateCompany(currCompany, updCompCallback);
    };
    var delPhoneCallback = function(comp) {
        retrievCompany(comp);
    };
    var deletePhone = function(phone) {
        client.deletePhone(phone, delPhoneCallback);
    };
    var updatePhone = function (phone) {
        phone.mode(displayMode.edit);
    };
    var updatePerson = function (phone) {
        phone.mode(displayMode.edit);
    };
    var savePhone = function (phone) {
        console.log(phone);
        phone.mode(displayMode.view);
    };
    var savePerson = function(phone) {
        phone.mode(displayMode.view);
    };
    var retrieveCompanys = function () {
        client.getCompanys(retrieveCompanysCallback);
    };
    var deletePerson = function(person) {
        client.deletePerson(person, delPhoneCallback);
    };
    var sndPrCallback = function(data, cont) {
        alert("КП отправленно на адрес: " + cont.Email());
        retrievCompany(data);
    };
    var sndKp = function(par) {
        client.sendKp(par, sndPrCallback);
    };
    var saveAndClose = function() {
        client.updateCompany(currCompany, updCompCallback);
        setView("COMPANY");
    };
    var showCompDet = function(p) {
        viewmodel.showCompanyDetails(p);
    };
    var showDet = function(p) {
        return viewmodel.showCompanyDetails() === p;
    }
    var init = function () {
        retrieveCompanys();
        ko.applyBindings(companysViewModel,document.getElementById("companysBlock"));
    };
    $(init);
    return{
        setView: setView,showCompDet: showCompDet,showDet:showDet,
        currView: currView,
        companysViewModel: companysViewModel,
        addCompany: addCompany,
        setFilterRegion:setFilterRegion,
        currCompany: currCompany,
        editCompany: editCompany,viewCompany:viewCompany,
        displayMode:displayMode,
        postCompany:postCompany,
        putCompany:putCompany,
        addTell: addTell, addPerson: addPerson, deletePerson: deletePerson,
        deletePhone: deletePhone, updatePhone: updatePhone,savePhone:savePhone,
        updatePerson: updatePerson,savePerson:savePerson,
        canSendKp: canSendKp, sndKp: sndKp, saveAndClose: saveAndClose
    };
}();