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

        var client = LinkData();

    var manager = {
        view: ko.observable(false),
        botAnsw: ko.observable(),
        botShow: ko.observable("HIDE"),
        succ: ko.observable("HIDE"),
        fale: ko.observable("HIDE"),
        botView: ko.observable("0"),
        botText: ko.observable()
    };

    var botCheck = function(par) {
        manager.botShow(par);
        manager.view(false);
    };
    var feedback = {
        phone: ko.observable(),
        email: ko.observable(),
        title: ko.observable(),
        MailMessage: ko.observable(),
        actionData:ko.observable("NONE")
    };

    var isMailOk = function() {
        return feedback.title() && feedback.email() && feedback.MailMessage();
    };

    var clMod = function() {
        feedback.MailMessage(null);
        feedback.email(null);
        feedback.phone(null);
        feedback.title(null);
        feedback.actionData("NONE");
    };
    var sCallback = function(data) {
        manager.botView("1");
        if (feedback.actionData() === "CALL") {
            manager.botText("Спасибо, "+data.Title.split(" ")[0]+" за, то что обратились к нам. Буквально через 15 минут я Вас наберу на номер: "+data.Phone);
        } else {
            manager.botText("Сообщение отправленно.");
        }
        manager.botAnsw(null);
        clMod();
    };
    var badCallback = function(data) {
        manager.botView("2");
        if (feedback.actionData() === "CALL") {
            manager.botText(data);
        } else {
            manager.botText("Ошибка отправки сообщения. "+data);
        }
        manager.botAnsw(null);
        clMod();
    };
    var closeBot = function(par) {
        manager.botShow("HIDE");
        manager.botView("0");
    };

    var sendData = function () {
        if (manager.botAnsw() !== "2") {
            manager.botView("2");
            manager.botText("Ответ не верный.");
            clMod();
            return;
        } else {
            client.callMeReq(feedback,sCallback, badCallback);
        }
    };

    var setOption = function (option) {
        feedback.actionData(option);
    };

    var showDetails = function(det) {
        manager.view(det);
    };

    var isDetails = function(param) {
        return manager.view() === param;
    };

    var initMask = function() {
        $.mask.definitions["9"] = "_";
        $.mask.definitions["f"] = "[0-9]";
        $("#fbTl").mask("+7(fff)fff-ffff", { placeholder: "+7(***)***-****" });
    };

    var init = function () {
        initMask();
        ko.applyBindings();
    };

    $(init);

    return{
        manager: manager,
        isDetails:isDetails,
        showDetails: showDetails,
        setOption: setOption,
        feedback: feedback,
        sendData: sendData,
        botCheck:botCheck,
        closeBot: closeBot,
        isMailOk: isMailOk
    };
}();
