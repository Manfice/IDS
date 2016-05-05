$(document).ready(function () {
    var owlTop = $('#topSlide');
    var owlHot = $('#hotCarousel');
    owlTop.owlCarousel({
        items: 1,
        loop: true,
        animateOut: 'rollOut',
        animateIn: 'zoomIn',
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
});
var stickyOffset = $('#mainNav').offset().top;
$(window).scroll(function() {
    var sticky = $('#mainNav');
    var scrol = $(window).scrollTop();
    if (scrol >= stickyOffset+700) {
        sticky.addClass('fixTop');
    } else {
        sticky.removeClass('fixTop');
    }
});