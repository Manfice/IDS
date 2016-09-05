var SearchClient = function() {
    var baseUrl = "/api/search";

    var getSearchresult = function (requestString, callback) {
        
        $.ajax({
            url: baseUrl + "/postsearch/",
            data: requestString,
            type:"POST",
            success: function(data) {
                callback(data);
            }
        });
    };

    return {
        getSearchresult: getSearchresult
    };
}