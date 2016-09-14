﻿var adminClient = function() {

    var getCompanys = function(callback) {
        $.ajax({
            url: "/api/crm/getcompanys/",
            type: "GET",
            success: function(data) {
                callback(data);
            }
        });
    };

    var updateCompany = function (currCompany, callback) {
        console.log(ko.toJSON(currCompany));
        $.ajax({
            url: "/api/crm/UpdateCompany/",
            type: "POST",
            data: ko.toJSON(currCompany),
            contentType:"application/json",
            success: function (result) {
                callback(result);
            }
        });
    }

    var deletePhone = function(phone,callback) {
        $.ajax({
            url: "/api/crm/DeletePhone/"+phone.Id(),
            type:"DELETE",
            success: function(result) {
                callback(result);
            }
        });
    }
    var deletePerson = function (person, callback) {
        $.ajax({
            url: "/api/crm/DeletePerson/" + person.Id(),
            type: "DELETE",
            success: function (result) {
                callback(result);
            }
        });
    }

    var sendKp = function(pers, callback) {
        $.ajax({
            url: "/api/crm/sendKp/",
            type:"POST",
            contentType: "application/json",
            data:pers,
            success: function(data) {
                callback(data);
            }
        });
    }
    return{
        getCompanys: getCompanys,
        updateCompany: updateCompany,
        deletePhone: deletePhone,deletePerson,
        sendKp: sendKp
    };
};