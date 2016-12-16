var SearchClient = function() {
    var baseUrl = "/api/search";

    var getSearchresult = function(requestString, callback) {
        console.log("search start");

        $.ajax({
            url: baseUrl + "/postsearch/",
            data: requestString,
            type: "POST",
            success: function (data) {
                console.log("search success");
                console.log(ko.toJS(data));
                callback(data);
            }
        });
    };

    return {
        getSearchresult: getSearchresult
    };
};