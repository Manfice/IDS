$(document).ready(function() {
    var shop = $("#reclShop");
    shop.owlCarousel({
        items: 1,
        autoplay: true,
        autoplayTimeout: 20000,
        autoplayHoverPause: true,
        loop: true
    });
    var visible = false;
    $("#call").click(function () {
        if (visible === false) {
            $(".callMe").css({
                'visibility': "visible"
            });
            visible = true;
        } else {
            $(".callMe").css({
                'visibility': "collapse"
            });
            visible = false;
        }
    });
    $(".funShow").fancybox({
        padding: 10,
        openEffect: "elastic",
        openSpeed: 150,
        closeEffect: "elastic",
        closeSpeed: 150,
        closeClick: true,
        helpers: {
            overlay: {
                css: {
                    "background": "rgba(238,238,238,0.85)"
                }
            }
        }
});
});