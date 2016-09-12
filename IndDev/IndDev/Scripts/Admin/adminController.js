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

    return{
        getCompanys: getCompanys
    };
};