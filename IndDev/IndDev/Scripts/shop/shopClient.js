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
    var getCategorys = function (groupe,callback) {
        $.ajax({
            type: "GET",
            url: "/shop/GetCategorys/"+groupe.Id(),
            success: function (data) {
                callback(data, groupe);
            }
        });
    };

    return {
        getMenuItems: getMenuItems, getTopRetails: getTopRetails,
        getBrands: getBrands, getCategorys: getCategorys
    }
};