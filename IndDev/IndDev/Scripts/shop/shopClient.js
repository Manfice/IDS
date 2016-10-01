var ShopClient = function() {
    var getMenuItems = function(callback) {
        $.ajax({
            type:"GET",
            url: "/shop/GetTopMenus",
            success: function(data) {
                callback(data);
            }
        });
    };
    var getTopRetails = function (callback) {
        $.ajax({
            type: "GET",
            url: "/shop/GetTopRetails",
            success: function (data) {
                callback(data);
            }
        });
    };
    var getBrands = function (callback) {
        $.ajax({
            type: "GET",
            url: "/shop/GetBrandsPic",
            success: function (data) {
                callback(data);
            }
        });
    };
    var getCategorys = function (id,callback) {
        $.ajax({
            type: "GET",
            url: "/shop/GetCategorys/"+id,
            success: function (data) {
                callback(data);
            }
        });
    };

    return {
        getMenuItems: getMenuItems, getTopRetails: getTopRetails,
        getBrands, getCategorys: getCategorys
    }
};