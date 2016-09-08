var Common = function () {

        $(".authButton").click(function() {
            $(this).next().slideToggle();
        });
        $(".menuBtn").click(function () {
            $(this).next().slideToggle();
        });
        $(".callButton").click(function () {
            $(this).next().slideToggle();
        });
        $(".cartButton").click(function () {
            $(this).next().slideToggle();
        });
        $(".calcItem").click(function () {
            $(this).next().slideToggle();
        });

        var optionsMeneger = ko.observable("NONE");

    var setOption = function(option) {
        optionsMeneger(option);
    };

    var manager = {
        view: ko.observable(false),
        callMe: ko.observable(false)
    };

    var phone = ko.observable();

    var mailMessage = {
        email: ko.observable(),
        message: ko.observable()
    };

    var showDetails = function(det) {
        manager.view(det);
        console.log(manager.view);
    };

    var displayOption = {
        call: "CALL",
        message: "MAIL",
        none: "NONE"
    };

    var isDetails = function(param) {
        return manager.view() === param;
    };

    var init = function() {
        ko.applyBindings();
    };

    $(init);

    return{
        manager: manager,
        isDetails:isDetails,
        showDetails: showDetails,
        optionsMeneger: optionsMeneger,
        setOption: setOption, phone: phone,
        mailMessage:mailMessage
    };
}();
