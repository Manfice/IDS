var adminClient = function() {

    var getCompanys = function(callback) {
        $.ajax({
            url: "/api/crm/getcompanys/",
            type: "GET",
            success: function (data) {
                callback(data);
            }
        });
    };
    var retrieveCompanyFromServer = function (comp, callback) {
        $.ajax({
            url: "/api/crm/GetCompany/"+comp.Id(),
            type: "GET",
            success: function (data) {
                callback(data);
            }
        });
    };
    var updateCompany = function(currCompany, callback) {
        $.ajax({
            url: "/api/crm/UpdateCompany/",
            type: "POST",
            data: ko.toJSON(currCompany),
            contentType: "application/json",
            success: function(result) {
                callback(result);
            }
        });
    };
    var updatePhone = function (phone, callback) {
        $.ajax({
            url: "/api/crm/UpdatePhone/",
            type: "POST",
            data: ko.toJSON(phone),
            contentType: "application/json",
            success: function (result) {
                callback(phone, result);
            }
        });
    };
    var addEvent = function (ev, callback) {
        $.ajax({
            url: "/api/crm/PostEvent/",
            type: "POST",
            data: ko.toJSON(ev),
            contentType: "application/json",
            success: function (result) {
                callback(ev, result);
            }
        });
    };
    var updatePerson = function (person, callback) {
        $.ajax({
            url: "/api/crm/UpdatePerson/",
            type: "POST",
            data: ko.toJSON(person),
            contentType: "application/json",
            success: function (result) {
                callback(person, result);
            }
        });
    };

    var deletePhone = function(phone, callback) {
        $.ajax({
            url: "/api/crm/DeletePhone/" + phone.Id(),
            type: "DELETE",
            success: function(result) {
                callback(phone);
            }
        });
    };
    var deletePerson = function(person, callback) {
        $.ajax({
            url: "/api/crm/DeletePerson/" + person.Id(),
            type: "DELETE",
            success: function(result) {
                callback(person);
            }
        });
    };

    var sendKp = function(pers, callback) {
        $.ajax({
            url: "/api/crm/sendKp/",
            type: "POST",
            //contentType: "application/json",
            data: pers,
            success: function(data) {
                callback(data, pers);
            }
        });
    };

    return{
        getCompanys: getCompanys,
        updateCompany: updateCompany,
        deletePhone: deletePhone,deletePerson,
        sendKp: sendKp,addEvent:addEvent,
        retrieveCompanyFromServer: retrieveCompanyFromServer,
        updatePhone: updatePhone,updatePerson:updatePerson
    };
};