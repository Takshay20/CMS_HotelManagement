$(function () {
    var current = 0;
    var slides = $(".slide");
    var total = slides.length;
    var interval;
    var isPaused = false;

    // Show slide
    function showSlide(index) {
        slides.removeClass("active");
        slides.eq(index).addClass("active");
    }

    // Next slide
    function nextSlide() {
        if (isPaused) return;
        current = (current + 1) % total;
        showSlide(current);
    }

    // Prev slide
    function prevSlide() {
        current = (current - 1 + total) % total;
        showSlide(current);
    }

    // Start auto slide
    function startAutoSlide() {
        interval = setInterval(nextSlide, 3000);
    }

    // Reset auto slide
    function resetAutoSlide() {
        clearInterval(interval);
        startAutoSlide();
    }
    startAutoSlide();

    // Next arrow click
    $(".next-arrow").on("click", function () {
        current = (current + 1) % total;
        showSlide(current);
        resetAutoSlide();
    });

    // Prev arrow click
    $(".prev-arrow").on("click", function () {
        prevSlide();
        resetAutoSlide();
    });

    // Slider field hover
    $(".slider").on("mouseenter", function () {
        isPaused = true;
    });

    // Slider field mouseleave
    $(".slider").on("mouseleave", function () {
        isPaused = false;
    });
});






// Page load: setup and event handlers
$(document).ready(function () {
    var current = 0;
    var total = $(".t-slide").length;
    var slider = null;

    // Show slide
    function showSlide() {
        $(".t-wrapper").css("transform",
            "translateX(-" + (current * 100) + "%)");

    }

    // Start slider
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

    // Stop slider
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

// Page load: setup and event handlers
$(function () {

    var slides = $(".room-slide");
    var dots = $(".dot");

    var current = 0;
    var timer;

    // Show slide
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

    // Next slide
    function nextSlide() {
        var next = current + 1;
        if (next >= slides.length) {
            next = 0;
        }
        showSlide(next);
    }

    // Prev slide
    function prevSlide() {
        var prev = current - 1;
        if (prev < 0) {
            prev = slides.length - 1;
        }
        showSlide(prev);
    }

    // Start slider
    function startSlider() {
        timer = setInterval(function () {
            nextSlide();
        }, 4000);
    }

    // Stop slider
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