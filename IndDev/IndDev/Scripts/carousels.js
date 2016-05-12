$(document).ready(function () {
    var owlTop = $('#topSlide');
    var owlHot = $('#hotCarousel');
    var owlAforizm = $('#aforizmi');
    owlTop.owlCarousel({
        items: 1,
        loop: true,
        autoplay: true,
        autoplayTimeout: 10000,
        autoplayHoverPause: true,
        nav: false,
        dots: false
    });
    $('.prevItem').click(function () {
        owlTop.trigger('prev.owl.carousel', [3000]);
    });
    $('.nextItem').click(function () {
        owlTop.trigger('next.owl.carousel', [3000]);
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
    owlAforizm.owlCarousel({
        items: 1,
        autoplay: true,
        autoplayTimeout: 20000,
        autoplayHoverPause: true,
        loop: true
});
});
var stickyOffset = $('#mainNav').offset().top;
$(window).scroll(function() {
    var sticky = $('#mainNav');
    var subStick = $('.subMenuList');
    var scrol = $(window).scrollTop();
    if (scrol >= stickyOffset+700) {
        sticky.addClass('fixTop');
        sticky.addClass('blueList');
        subStick.addClass('blueList');
    } else {
        sticky.removeClass('fixTop');
        sticky.removeClass('blueList');
        subStick.removeClass('blueList');
    }
});