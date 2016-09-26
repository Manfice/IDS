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
        filteredCompanys: ko.observableArray(),
        innSearch:ko.observable(null),
        nameSearch:ko.observable(null)
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

    var checkInn = function(p, x) {
        var c = false;
        if (!p.Inn) return false;
        var s = p.Inn();
        if (s.indexOf(x.toString()) !== -1) c = true;
        return c;
    }
    var checkname = function (p, x) {
        var c = false;
        if (!p.CompanyName) return false;
        var s = p.CompanyName().toLowerCase();
        if (s.indexOf(x.toString().toLowerCase()) !== -1) c = true;
        return c;
    }

    companysViewModel.innSearch.subscribe(function (newValue) {
        if (!newValue) return filterCompanysByRegion();
        filterCompanysByRegion();
        companysViewModel.filteredCompanys.removeAll();
        var fl = companysViewModel.companys().filter(function (p) {
            return checkInn(p, newValue);
        });
        companysViewModel.filteredCompanys(fl);
        console.log(fl);
        return null;
    });
    companysViewModel.nameSearch.subscribe(function (newValue) {
        if (!newValue) return filterCompanysByRegion();
        filterCompanysByRegion();
        companysViewModel.filteredCompanys.removeAll();
        var fl = companysViewModel.companys().filter(function (p) {
            return checkname(p, newValue);
        });
        companysViewModel.filteredCompanys(fl);
        console.log(fl);
        return null;
    });
    companysViewModel.companys.subscribe(function (newCompanys) {
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
        var r = region == null ? null : region.title;
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

    var persData = function(pers, mode,detId) {
        this.Id = ko.observable(pers.Id);
        this.PersonName = ko.observable(pers.PersonName);
        this.Email = ko.observable(pers.Email);
        this.Phone = ko.observable(pers.Phone);
        this.Details = ko.observable(detId);
        this.mode = ko.observable(mode);
        this.kp = canKp(pers.Id);
    };
    var eventData = function (event, mode, detId) {
        this.Id = ko.observable(event.Id);
        this.EventDate = ko.observable(new Date(event.EventDate).toLocaleDateString("ru-RU"));
        this.EventInit = ko.observable(event.EventInit);
        this.EventType = ko.observable(event.EventType);
        this.Priority = ko.observable(event.Priority);
        this.RemindMe = ko.observable(event.RemindMe);
        this.Descr = ko.observable(event.Descr);
        this.Meneger = ko.observable(event.Meneger);
        this.Details = ko.observable(detId);
        this.mode = ko.observable(mode);
    };
    var eventClass = {
        Id:0,
        EventDate:new Date(),
        EventInit:true,
        Priority:3,
        RemindMe:true,
        Descr:"",
        Meneger:0,
        Details:null
    }
    var setView = function(data) {
        viewmodel.currtab(data);
    };
    var addEvent = function(et) {
        var e = new eventData(eventClass);
        e.EventType = et;
        e.Details = currCompany().Id;
        console.log(ko.toJSON(e));
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
        PersonContacts: ko.observableArray([]),
        Events: ko.observableArray([])
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
        var tel = { Id: 0, PhoneNumber: null, Title: null, Details:comp.Id() };
        var tell = new telData(tel, displayMode.edit, comp.Id());
        comp.Telephones.push(tell);
        currCompany(comp);
    };
    var addPerson = function(com) {
        var pers = { Id: 0, PersonName: null, Email: null, Phone: null };
        var prs = new persData(pers, displayMode.edit, com.Id);
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
            compData.PersonContacts.push(new persData(pers, displayMode.view,detId));
        });
        item.Events.forEach(function (e) {
            compData.Events.push(new eventData(e, displayMode.view, detId));
        });
        currCompany(compData);
        companysViewModel.companys.remove(function(i) {
            return i.Id() === compData.Id();
        });
        companysViewModel.companys.push(compData);
        console.log("Retrieve Compny: "+ko.toJSON(compData));
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
            if (item.Inn == null) item.Inn = "";
            var compData = new company(item);
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
    var delPhoneCallback = function (comp) {
        var data = currCompany();
        data.Telephones.remove(function(p) {
            return p.Id() === comp.Id();
        });
        currCompany(data);
    };
    var deletePhone = function(phone) {
        client.deletePhone(phone, delPhoneCallback);
    };
    var updatePerson = function (phone) {
        phone.mode(displayMode.edit);
    };
    var svPhoneCb = function(phone, id) {
        phone.Id(id);
        phone.mode(displayMode.view);
        alert("Телефон сохранен:" + phone.PhoneNumber());
    };
    var savePhone = function (phone) {
        client.updatePhone(phone, svPhoneCb);
    };
    var updatePhone = function (phone) {
        phone.mode(displayMode.edit);
    };
    var svPersCb = function(person, id) {
        person.Id(id);
        person.mode(displayMode.view);
        alert("Контакт сохранен:" + person.PersonName());
    };
    var savePerson = function (person) {
        client.updatePerson(person, svPersCb);
    };
    var retrieveCompanys = function () {
        client.getCompanys(retrieveCompanysCallback);
    };
    var delPersCb = function(person) {
        var data = currCompany();
        data.PersonContacts.remove(function(p) {
            return p.Id() === person.Id();
        });
        currCompany(data);
    };
    var deletePerson = function(person) {
        client.deletePerson(person, delPersCb);
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
    };
    var addNewComment = function() {
        var line = currCompany();
        var dt = new Date();
        var txt = "\n\n^^^----- " + dt.toLocaleString() + " ----^^^\n";
        txt += line.Descr();
        line.Descr(txt);
        console.log(ko.toJS(line));
        currCompany(line);
    };
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
        canSendKp: canSendKp, sndKp: sndKp, saveAndClose: saveAndClose,
        addNewComment: addNewComment, addEvent: addEvent
    };
}();