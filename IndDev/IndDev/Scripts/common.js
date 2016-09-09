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

    var sendRq = ko.observable();

    var sendData = function () {
        console.log(ko.toJS(sendRq));
        if (sendRq()!=="2") {
            console.log("Лошара... ");
            return;
        } else {
            callMeReq();
        }
    };

    var feedback = {
        phone: ko.observable(),
        email: ko.observable(),
        title: ko.observable(),
        MailMessage: ko.observable()
    };

    var showDetails = function(det) {
        manager.view(det);
    };

    var isDetails = function(param) {
        return manager.view() === param;
    };

    var callMeReq = function () {
        $.ajax({
            type: "POST",
            url: "/api/feedback/CallMe",
            data: feedback,
            success: function (data) {
                console.log(ko.toJSON(data));

            }
        });
    };

    var initMask = function() {
        $.mask.definitions["9"] = "_";
        $.mask.definitions["f"] = "[0-9]";
        $("#fbTl").mask("+7(fff)fff-ffff",{placeholder:"+7(***)***-****"});
    }

    var init = function () {
        initMask();
        ko.applyBindings();
    };

    $(init);

    return{
        manager: manager,
        isDetails:isDetails,
        showDetails: showDetails,
        optionsMeneger: optionsMeneger,
        setOption: setOption, feedback: feedback,
        sendData: sendData,
        sendRq: sendRq
    };
}();
