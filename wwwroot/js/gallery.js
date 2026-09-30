// Page load: setup and event handlers
$(document).ready(function () {

    var slides = $(".rp-cinema-slide");
    var totalSlides = slides.length;
    var currentSlide = 0;
    var sliderTimer = null;

    if (totalSlides === 0) {
        return;
    }

    $("#rpTotal").text(
        totalSlides.toString().padStart(2, "0")
    );

    $("#rpThumbnails").empty();

    slides.each(function (index) {

        var slide = $(this);

        var image = slide.attr("data-image") || "";
        var title = slide.attr("data-title") || "Royal Paradise";

        var thumb = $("<button>", {
            type: "button",
            class: "rp-cinema-thumb",
            "data-index": index
        });

        var thumbImg = $("<img>", {
            src: image,
            alt: title
        });

        var number = $("<span>", {
            class: "rp-cinema-thumb-number",
            text: (index + 1).toString().padStart(2, "0")
        });

        thumb.append(thumbImg);
        thumb.append(number);

        $("#rpThumbnails").append(thumb);
    });

    // Show slide
    function showSlide(index) {

        if (index >= totalSlides) {
            index = 0;
        }

        if (index < 0) {
            index = totalSlides - 1;
        }

        slides
            .removeClass("active")
            .eq(index)
            .addClass("active");

        $(".rp-cinema-thumb")
            .removeClass("active")
            .eq(index)
            .addClass("active");

        $("#rpCurrent").text(
            (index + 1).toString().padStart(2, "0")
        );

        var percentage =
            ((index + 1) / totalSlides) * 100;

        $("#rpProgress").css(
            "width",
            percentage + "%"
        );

        currentSlide = index;
    }

    // Stop slider
    function stopSlider() {

        if (sliderTimer !== null) {

            clearInterval(sliderTimer);
            sliderTimer = null;
        }
    }

    // Start slider
    function startSlider() {

        stopSlider();

        if (totalSlides <= 1) {
            return;
        }

        sliderTimer = setInterval(function () {

            showSlide(
                currentSlide + 1
            );

        }, 5000);
    }

    // Restart slider
    function restartSlider() {

        stopSlider();
        startSlider();
    }

    // Slider next button click
    $("#rpNext").on("click", function () {

        showSlide(
            currentSlide + 1
        );

        restartSlider();
    });

    // Slider previous button click
    $("#rpPrev").on("click", function () {

        showSlide(
            currentSlide - 1
        );

        restartSlider();
    });

    $("#rpThumbnails").on(
        "click",
        ".rp-cinema-thumb",
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

    $("#rpCinemaSlider").on(
        "mouseenter",
        function () {

            stopSlider();
        }
    );

    $("#rpCinemaSlider").on(
        "mouseleave",
        function () {

            startSlider();
        }
    );

    showSlide(0);
    startSlider();

    $(".rp-gallery-filter").on(
        "click",
        function () {

            var filter = $(this).attr(
                "data-filter"
            );

            $(".rp-gallery-filter")
                .removeClass("active");

            $(this).addClass("active");

            $(".rp-gallery-item").each(
                function () {

                    var category = $(this).attr(
                        "data-category"
                    );

                    if (
                        filter === "all" ||
                        category === filter
                    ) {

                        $(this).removeClass("hide");

                    }
                    else {

                        $(this).addClass("hide");
                    }
                }
            );
        }
    );

    var galleryItems = $(".rp-gallery-item");
    var lightboxIndex = 0;

    // Open lightbox
    function openLightbox(index) {

        if (galleryItems.length === 0) {
            return;
        }

        if (index >= galleryItems.length) {
            index = 0;
        }

        if (index < 0) {
            index = galleryItems.length - 1;
        }

        var item = galleryItems.eq(index);

        var image = item.attr(
            "data-image"
        );

        var title = item.attr(
            "data-title"
        );

        $("#rpLightboxImage").attr(
            "src",
            image || ""
        );

        $("#rpLightboxTitle").text(
            title || ""
        );

        $("#rpLightbox").addClass(
            "show"
        );

        $("body").css(
            "overflow",
            "hidden"
        );

        lightboxIndex = index;
    }

    // Close lightbox
    function closeLightbox() {

        $("#rpLightbox").removeClass(
            "show"
        );

        $("body").css(
            "overflow",
            ""
        );
    }

    $(".rp-gallery-item").on(
        "click",
        function () {

            var index = galleryItems.index(
                this
            );

            openLightbox(index);
        }
    );

    $("#rpLightboxClose").on(
        "click",
        function (e) {

            e.stopPropagation();
            closeLightbox();
        }
    );

    $("#rpLightbox").on(
        "click",
        function (e) {

            if (e.target === this) {

                closeLightbox();
            }
        }
    );

    $("#rpLightboxNext").on(
        "click",
        function (e) {

            e.stopPropagation();

            openLightbox(
                lightboxIndex + 1
            );
        }
    );

    $("#rpLightboxPrev").on(
        "click",
        function (e) {

            e.stopPropagation();

            openLightbox(
                lightboxIndex - 1
            );
        }
    );

    $(document).on(
        "keydown",
        function (e) {

            if (
                !$("#rpLightbox")
                    .hasClass("show")
            ) {
                return;
            }

            if (e.key === "Escape") {

                closeLightbox();

            }
            else if (
                e.key === "ArrowRight"
            ) {

                openLightbox(
                    lightboxIndex + 1
                );

            }
            else if (
                e.key === "ArrowLeft"
            ) {

                openLightbox(
                    lightboxIndex - 1
                );
            }
        }
    );
});