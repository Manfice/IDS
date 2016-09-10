var LinkData = function () {

    var callMeReq = function (feed, callback, badcallback) {
        $.ajax({
            type: "POST",
            url: "/api/feedback/CallMe",
            data: feed,
            success: function (data) {
                console.log(ko.toJSON(data));
                callback(data);
            },
            error: function(jqXhr) {
                badcallback(jqXhr.responseText);
            }
        });
    };

    return {
        callMeReq: callMeReq
    };
};