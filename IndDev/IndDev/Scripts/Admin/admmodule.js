﻿var Admin = function() {
    var client = adminClient();

    var viewmodel = {
        currtab: ko.observable("COMPANY")
    };

    var companys = ko.observableArray();

    var currCompany = ko.observable();

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
    };

    var canKp = function(pr) {
        return pr !== 0;
    }

    var persData = function(pers, mode) {
        this.Id = ko.observable(pers.Id);
        this.PersonName = ko.observable(pers.PersonName);
        this.Email = ko.observable(pers.Email);
        this.Phone = ko.observable(pers.Phone);
        this.mode = ko.observable(mode);
        this.kp = canKp(pers.Id);
    };

    var setView = function(data) {
        viewmodel.currtab(data);
    };

    var currView = function(data) {
        return viewmodel.currtab() === data;
    };
    var isNewCompany = function() {
        return currCompany.Id === 0;
    };
    var canSendKp = function (pers) {
        alert(pers.Id);
        return pers.Id !== "0";
    }
    var newCompany = {
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
        currCompany(cp);
    };
    var addTell = function (company) {
        var tel = { Id: 0, PhoneNumber: null, Title: null };
        var tell = new telData(tel, displayMode.edit);
        company.Telephones.push(tell);
        currCompany(company);
    }
    var addPerson = function(com) {
        var pers = { Id: 0, PersonName: null, Email:null, Phone:null };
        var prs = new persData(pers, displayMode.edit);
        com.PersonContacts.push(prs);
        currCompany(com);
    }
    var editCompany = function(comp) {
        viewmodel.currtab("EDIT");
        currCompany(comp);
    };
    var retrieveCompanysCallback = function(data) {
        data.forEach(function (item) {
            var compData = new company(item);
            item.Telephones.forEach(function(phone) {
                compData.Telephones.push(new telData(phone, displayMode.view));
            });
            item.Banks.forEach(function(bank) {
                compData.Banks.push(bank);
            });
            item.PersonContacts.forEach(function(pers) {
                compData.PersonContacts.push(new persData(pers,displayMode.view));
            });
            companys.push(compData);

        });
    };
    var retrievCompany = function(item) {
        var compData = new company(item);
        item.Telephones.forEach(function(phone) {
            compData.Telephones.push(new telData(phone, displayMode.view));
        });
        item.Banks.forEach(function(bank) {
            compData.Banks.push(bank);
        });
        item.PersonContacts.forEach(function(pers) {
            compData.PersonContacts.push(new persData(pers, displayMode.view));
        });
        currCompany(compData);
    };
    var saveCompanyCallback = function(result, id) {
        retrievCompany(result);
        var compData = new company(item);
        item.Telephones.forEach(function (phone) {
            compData.Telephones.push(new telData(phone, displayMode.view));
        });
        item.Banks.forEach(function (bank) {
            compData.Banks.push(bank);
        });
        item.PersonContacts.forEach(function (pers) {
            compData.PersonContacts.push(new persData(pers, displayMode.view));
        });
        companys.push(compData);
    };
    var updCompCallback = function(result) {
        alert("Изменения сохранены");
        retrievCompany(result);
    }
    var postCompany = function() {
        client.updateCompany(currCompany, saveCompanyCallback);
    };
    var putCompany = function() {
        client.updateCompany(currCompany,updCompCallback);
    }
    var delPhoneCallback = function(comp) {
        retrievCompany(comp);
    };
    var deletePhone = function(phone) {
        client.deletePhone(phone,delPhoneCallback);
    }
    var updatePhone = function (phone) {
        phone.mode(displayMode.edit);
    };
    var updatePerson = function (phone) {
        phone.mode(displayMode.edit);
    };
    var savePhone = function (phone) {
        phone.mode(displayMode.view);
    }
    var savePerson = function (phone) {
        phone.mode(displayMode.view);
    }
    var retrieveCompanys = function () {
        client.getCompanys(retrieveCompanysCallback);
    };
    var deletePerson = function(person) {
        client.deletePerson(person,delPhoneCallback);
    }
    var sndPrCallback = function(data) {
        console.log(ko.toJSON(data));
        alert("КП отправленно на адрес: "+data.Email);
    }
    var sndKp = function(par) {
        client.sendKp(par, sndPrCallback);
    }
    var saveAndClose = function() {
        client.updateCompany(currCompany, updCompCallback);
        setView("COMPANY");
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
        isNewCompany:isNewCompany,
        addTell: addTell, addPerson: addPerson, deletePerson: deletePerson,
        deletePhone: deletePhone, updatePhone: updatePhone,savePhone:savePhone,
        updatePerson: updatePerson,savePerson:savePerson,
        canSendKp: canSendKp, sndKp: sndKp, saveAndClose: saveAndClose
    };
}();