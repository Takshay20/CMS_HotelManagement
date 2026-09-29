$(document).ready(function () {

    var slides = $(".facility-slide");
    var total = slides.length;
    var current = 0;
    var timer = null;

    if (total === 0) {
        return;
    }

    $(".facility-dots").empty();

    for (var i = 0; i < total; i++) {

        $(".facility-dots").append(
            '<span class="facility-dot" data-index="' +
            i +
            '"></span>'
        );

    }

    $("#facilityTotal").text(
        String(total).padStart(2, "0")
    );

    function showSlide(index) {

        if (index >= total) {
            index = 0;
        }

        if (index < 0) {
            index = total - 1;
        }

        slides
            .removeClass("active")
            .eq(index)
            .addClass("active");

        $(".facility-dot")
            .removeClass("active")
            .eq(index)
            .addClass("active");

        $("#facilityCurrent").text(
            String(index + 1).padStart(2, "0")
        );

        var progress =
            ((index + 1) / total) * 100;

        $("#facilityProgress").css(
            "width",
            progress + "%"
        );

        current = index;

    }

    function stopSlider() {

        if (timer !== null) {

            clearInterval(timer);

            timer = null;

        }

    }

    function startSlider() {

        stopSlider();

        if (total <= 1) {
            return;
        }

        timer = setInterval(function () {

            showSlide(current + 1);

        }, 5000);

    }

    function restartSlider() {

        stopSlider();

        startSlider();

    }

    $(".facility-next").on(
        "click",
        function () {

            showSlide(current + 1);

            restartSlider();

        }
    );

    $(".facility-prev").on(
        "click",
        function () {

            showSlide(current - 1);

            restartSlider();

        }
    );

    $(document).on(
        "click",
        ".facility-dot",
        function () {

            var index = parseInt(
                $(this).attr("data-index"),
                10
            );

            if (!isNaN(index)) {

                showSlide(index);

                restartSlider();

            }

        }
    );

    $(".facility-slider").on(
        "mouseenter",
        function () {

            stopSlider();

        }
    );

    $(".facility-slider").on(
        "mouseleave",
        function () {

            startSlider();

        }
    );

    showSlide(0);

    startSlider();

});