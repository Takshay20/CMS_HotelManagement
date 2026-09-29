$(function () {
    var current = 0;
    var slides = $(".slide");
    var total = slides.length;
    var interval;
    var isPaused = false;
    function showSlide(index) {
        slides.removeClass("active");
        slides.eq(index).addClass("active");
    }
    function nextSlide() {
        if (isPaused) return;
        current = (current + 1) % total;
        showSlide(current);
    }
    function prevSlide() {
        current = (current - 1 + total) % total;
        showSlide(current);
    }
    function startAutoSlide() {
        interval = setInterval(nextSlide, 3000);
    }
    function resetAutoSlide() {
        clearInterval(interval);
        startAutoSlide();
    }
    startAutoSlide();
    $(".next-arrow").on("click", function () {
        current = (current + 1) % total;
        showSlide(current);
        resetAutoSlide();
    });
    $(".prev-arrow").on("click", function () {
        prevSlide();
        resetAutoSlide();
    });
    $(".slider").on("mouseenter", function () {
        isPaused = true;
    });
    $(".slider").on("mouseleave", function () {
        isPaused = false;
    });
});






$(document).ready(function () {
    var current = 0;
    var total = $(".t-slide").length;
    var slider = null;
    function showSlide() {
        $(".t-wrapper").css("transform",
            "translateX(-" + (current * 100) + "%)");

    }
    function startSlider() {
        if (slider != null) return;   
        slider = setInterval(function () {
            current++;
            if (current >= total) {
                current = 0;
            }
            showSlide();
        }, 4000);
    }
    function stopSlider() {

        clearInterval(slider);
        slider = null;

    }
    startSlider();
    $(".testimonial-slider").hover(

        function () {      
            stopSlider();
        },

        function () {      
            startSlider();
        }
    );
});

$(function () {

    var slides = $(".room-slide");
    var dots = $(".dot");

    var current = 0;
    var timer;
    function showSlide(index) {
        slides.removeClass("active prev");
        slides.each(function (i) {
            if (i < index) {
                $(this).addClass("prev");            }

        });
        slides.eq(index).addClass("active");
        dots.removeClass("active");
        dots.eq(index).addClass("active");
        current = index;
    }
    function nextSlide() {
        var next = current + 1;
        if (next >= slides.length) {
            next = 0;
        }
        showSlide(next);
    }
    function prevSlide() {
        var prev = current - 1;
        if (prev < 0) {
            prev = slides.length - 1;
        }
        showSlide(prev);
    }
    function startSlider() {
        timer = setInterval(function () {
            nextSlide();
        }, 4000);
    }
    function stopSlider() {
        clearInterval(timer);
    }
    startSlider();
    $(".room-slider").hover(function () {
        stopSlider();
    }, function () {
        startSlider();
    });
    $(".next").click(function () {
        stopSlider();
        nextSlide();
        startSlider();
    });
    $(".prev").click(function () {
        stopSlider();
        prevSlide();
        startSlider();
    });
    dots.click(function () {
        stopSlider();
        showSlide($(this).index());
        startSlider();
    });
});
$(".thumbs img").click(function () {

    var image = $(this).attr("src");

    $("#standardMain").fadeOut(200, function () {

        $(this).attr("src", image).fadeIn(300);

    });

    $(".thumbs img").removeClass("active-thumb");

    $(this).addClass("active-thumb");

});