var CustomerModule = function() {

    var client = CustomerClient();

    var activePage = ko.observable("MAIN");

    var editPersonal = ko.observable("VIEW");

    var modelCustomer = {
        Id: ko.observable(null),
        Title: ko.observable(null),
        Name: ko.observable("Не задано"),
        LastName: ko.observable("Не задано"),
        Photo: ko.observable(null),
        Register: ko.observable("Не задано"),
        PhoneCell: ko.observable("Не задано"),
        PhoneWork: ko.observable("Не задано"),
        Email: ko.observable(null),
        CustomerStatus: {
            id: ko.observable(),
            Title: ko.observable(),
            Discount: ko.observable(),
            PriceType: ko.observable()
        },
        Details: {
            Id: ko.observable(null),
            CompanyName: ko.observable(null),
            Inn: ko.observable("Не задано"),
            Kpp: ko.observable("Не задано"),
            Ogrn: ko.observable("Не задано"),
            RealAdress: ko.observable("Не задано"),
            UrAdress: ko.observable("Не задано"),
            Telephones:ko.observableArray([]),
            Banks:ko.observableArray([])
        }
    };

    var displayItem = {
        main: "MAIN",
        company: "COMPANY",
        orders: "ORDERS",
        reports: "REPORTS",
        help: "HELP"
    };

    var setView = function(view) {
        activePage(view);
    };

    var isActivePage = function(page) {
        return activePage() === page;
    };

    var customerCallback = function (customer) {
        console.log(ko.toJSON(customer));
        var date = new Date(customer.Register);
        modelCustomer.Id(customer.Id);
        modelCustomer.Name(customer.FirstName);
        modelCustomer.LastName(customer.MiddleName);
        modelCustomer.Photo(customer.Logo.Path);
        modelCustomer.PhoneCell(customer.PhoneCell);
        modelCustomer.PhoneWork(customer.PhoneWork);
        modelCustomer.Email(customer.Email);
        modelCustomer.Register(date.toLocaleDateString());
        modelCustomer.CustomerStatus.id(customer.CustomerStatus.Id);
        modelCustomer.CustomerStatus.Title(customer.CustomerStatus.Title);
        modelCustomer.CustomerStatus.Discount(customer.CustomerStatus.Discount);
        modelCustomer.CustomerStatus.PriceType(customer.CustomerStatus.PriceType);
        modelCustomer.Details.Id(customer.Details.Id);
        modelCustomer.Details.CompanyName(customer.Details.CompanyName);
        modelCustomer.Details.Inn(customer.Details.Inn);
        modelCustomer.Details.Kpp(customer.Details.Kpp);
        modelCustomer.Details.Ogrn(customer.Details.Ogrn);
        modelCustomer.Details.UrAdress(customer.Details.UrAdress);
        modelCustomer.Details.RealAdress(customer.Details.RealAdress);
        modelCustomer.Details.Telephones.removeAll();
        customer.Details.Telephones.forEach(function(data) {
            modelCustomer.Details.Telephones.push({
                Id: data.Id, Title: data.Title, PhoneNumber: data.PhoneNumber
            });
        });
        customer.Details.Banks.forEach(function (data) {
            modelCustomer.Details.Banks.push({
                Id: data.Id, Name: data.Name, Account: data.Account, Korr: data.Korr, Bik: data.Bik, Main:data.Main
            });
        });
        console.log(ko.toJSON(modelCustomer));
    };

    var saveAvatarCallback = function(data) {
        modelCustomer.Photo(data);
        var svBtn = $("#saveImage");
        svBtn.css("display", "none");
        $("#loading").css("visibility", "hidden");
        console.log("Saved success:" + ko.toJSON(modelCustomer));
    };

    var saveErrorCallback = function(data) {
        alert(data);
        $("#loading").css("visibility", "hidden");
    };

    var saveAvatar = function() {
        //console.log("SaveImg");
        var ava = new FormData();
        var img = $("#imgInput").get(0).files;
        if (img.length > 0) {
            ava.append("avatar", img[0]);
            client.saveAvatar(ava, saveAvatarCallback, saveErrorCallback);
        }
    };

    var retrieveCustomer = function() {
        client.getCustomer(customerCallback);
    };

    var editPersonalInfo = function(data) {
        editPersonal(data);
    };

    var isInEditModePersonal = function () {
        return editPersonal();
    };

    function imgPreview (input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            var saveBtn = $("#saveImage");
            reader.onload = function(e) {
                modelCustomer.Photo(e.target.result);
            };
            reader.readAsDataURL(input.files[0]);
            saveBtn.css("display","block");
        }
    }

    $("#imgInput").change(function() {
        imgPreview(this);
    });

    var init = function () {
        retrieveCustomer();
        //ko.applyBindings();
    };

    $(init);
    return {
        modelCustomer: modelCustomer,
        setView: setView,
        isActivePage: isActivePage,
        editPersonalInfo: editPersonalInfo,
        isInEditModePersonal: isInEditModePersonal,
        saveAvatar: saveAvatar
    };
}();