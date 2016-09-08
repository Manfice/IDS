var CustomerClient = function() {

    var getCustomer = function(callback) {
        $.ajax({
            url:"/api/custapi/getcustomer",
            type: "GET",
            success: function(data) {
                callback(data);
            }
        });
    };

    var saveAvatar = function(ava, callback, erroecallback) {
        $.ajax({
            url: "/api/custapi/saveavatar/",
            type: "POST",
            data: ava,
            contentType: false,
            processData: false,
            beforeSend: function() {
                $("#loading").css("visibility", "visible");
            },
            success: function(data) {
                callback(data);
            },
            error: function(jqXHR) {
                erroecallback(jqXHR.responseText);
            }
        });
    };

    return {
        getCustomer: getCustomer,
        saveAvatar: saveAvatar
    };
};