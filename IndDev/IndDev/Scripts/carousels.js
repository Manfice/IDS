$(document).ready(function () {
    var owlTop = $("#topSlide");
    var owlHot = $("#hotCarousel");
    var owlPart = $("#pLogos");
    var owlSert = $("#sert");
    var owlAforizm = $("#aforizmi");
    owlTop.owlCarousel({
        items: 1,
        loop: true,
        autoplay: true,
        autoplayTimeout: 10000,
        autoplayHoverPause: true,
        nav: false,
        dots: false,
        autoHeight: false
    });
    $(".prevItem").click(function () {
        owlTop.trigger("prev.owl.carousel", [3000]);
    });
    $(".nextItem").click(function () {
        owlTop.trigger("next.owl.carousel", [3000]);
    });

    owlHot.owlCarousel({
        loop: true,
        margin: 0,
        nav: false,
        dots: false,
        responsiveClass: true,
        autoHeight: false,
        responsive: {
            0: {
                items: 1,
                nav: false
            },
            600: {
                items: 3,
                nav: false
            },
            1400: {
                items: 5,
                nav: false,
                loop: true,
                autoplay: true,
                autoplayTimeout: 5000,
                autoplayHoverPause: true
            }
        }
    });
    owlPart.owlCarousel({
        loop: false,
        margin: 30,
        nav: false,
        dots: false,
        responsiveClass: true,
        autoHeight: false,
        responsive: {
            0: {
                items: 1,
                nav: false
            },
            600: {
                items: 3,
                nav: false
            },
            900: {
                items: 5,
                nav: false
            },
            1200: {
                items: 8,
                nav: false,
                loop: true,
                autoplay: false,
                autoplayTimeout: 15000,
                autoplayHoverPause: true
            }
        }
    });
    owlSert.owlCarousel({
        loop: true,
        margin: 30,
        nav: false,
        dots: false,
        responsiveClass: true,
        autoHeight: false,
        responsive: {
            0: {
                items: 1,
                nav: false
            },
            600: {
                items: 3,
                nav: false
            },
            900: {
                items: 5,
                nav: false
            },
            1200: {
                items: 8,
                nav: false,
                loop: true,
                autoplay: true,
                autoplayTimeout: 11000,
                autoplayHoverPause: true
            }
        }
    });
    $(".fancybox-thumbs").fancybox({
        prevEffect: "none",
        nextEffect: "none",

        closeBtn: false,
        arrows: false,
        nextClick: true,

        helpers: {
            thumbs: {
                width: 50,
                height: 50
            },
            overlay: {
                locked:false
            }
        }
    });

    owlAforizm.owlCarousel({
        items: 1,
        autoplay: true,
        autoplayTimeout: 20000,
        autoplayHoverPause: true,
        loop: true
});
});
var stickyOffset = $("#mainNav").offset().top;
$(window).scroll(function() {
    var sticky = $("#mainNav");
    var subStick = $(".subMenuList");
    var scrol = $(window).scrollTop();
    if (scrol >= stickyOffset) {
        sticky.addClass("fixTop");
        sticky.addClass("blueList");
        subStick.addClass("blueList");
    } else {
        sticky.removeClass("fixTop");
        sticky.removeClass("blueList");
        subStick.removeClass("blueList");
    }
});