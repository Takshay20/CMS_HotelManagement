$(document).ready(function () {

    var slides =
        $(".luxury-slide");

    var thumbs =
        $(".luxury-thumb");

    var total =
        slides.length;

    var current =
        0;

    var timer;

    $("#luxuryTotal").text(

        total < 10
            ? "0" + total
            : total

    );

    function showSlide(index) {

        if (total === 0) {

            return;

        }

        slides.removeClass("active");

        thumbs.removeClass("active");

        slides
            .eq(index)
            .addClass("active");

        thumbs
            .eq(index)
            .addClass("active");

        var number =
            index + 1;

        $("#luxuryCurrent").text(

            number < 10
                ? "0" + number
                : number

        );

        var progress =
            ((index + 1) / total) * 100;

        $("#luxuryProgress").css(
            "height",
            progress + "%"
        );

        current =
            index;

    }

    $("#luxuryNext").click(function () {

        current++;

        if (current >= total) {

            current = 0;

        }

        showSlide(current);

        restartSlider();

    });

    $("#luxuryPrev").click(function () {

        current--;

        if (current < 0) {

            current =
                total - 1;

        }

        showSlide(current);

        restartSlider();

    });

    thumbs.click(function () {

        var index =
            parseInt(
                $(this).attr("data-slide")
            );

        showSlide(index);

        restartSlider();

    });

    function startSlider() {

        timer =
            setInterval(
                function () {

                    current++;

                    if (current >= total) {

                        current = 0;

                    }

                    showSlide(current);

                },
                5000
            );

    }

    function restartSlider() {

        clearInterval(timer);

        startSlider();

    }

    $(".luxury-slider")
        .mouseenter(function () {

            clearInterval(timer);

        })
        .mouseleave(function () {

            startSlider();

        });

    if (total > 0) {

        showSlide(0);

        startSlider();

    }

});