var adminClient = function() {

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
                callback(currCompany, result);
            }
        });
    }

    return{
        getCompanys: getCompanys,
        updateCompany: updateCompany
    };
};