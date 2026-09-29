$(document).ready(function () {

    var $bookingForm = $("#bookingForm");

    var pricePerNight =
        parseFloat($bookingForm.data("price-per-night")) || 0;

    var roomId =
        parseInt($bookingForm.data("room-id"), 10) || 0;

    var isAuthenticated =
        String($bookingForm.data("is-authenticated")).toLowerCase() === "true";

    var checkAvailabilityUrl =
        $bookingForm.data("check-availability-url")
        || "/Booking/CheckAvailability";

    var createBookingUrl =
        $bookingForm.data("create-booking-url")
        || "/Booking/Create";

    var loginUrl =
        $bookingForm.data("login-url")
        || "/Account/Login";

    var MAX_NIGHTS = 60;
    var isRoomAvailable = false;

    function getTodayString() {

        var today = new Date();

        var year = today.getFullYear();

        var month =
            String(today.getMonth() + 1).padStart(2, "0");

        var day =
            String(today.getDate()).padStart(2, "0");

        return year + "-" + month + "-" + day;

    }

    function formatDate(date) {

        var year = date.getFullYear();

        var month =
            String(date.getMonth() + 1).padStart(2, "0");

        var day =
            String(date.getDate()).padStart(2, "0");

        return year + "-" + month + "-" + day;

    }

    function isValidDateString(value) {

        return /^\d{4}-\d{2}-\d{2}$/.test(value || "");

    }

    function setSubmitEnabled(enabled) {

        $bookingForm
            .find("button[type=submit]")
            .prop("disabled", !enabled);

    }

    function resetAvailability() {

        isRoomAvailable = false;

        $("#availabilityMsg").html("");

        setSubmitEnabled(false);

    }

    $("#checkInDate").attr(
        "min",
        getTodayString()
    );

    $("#checkInDate").on(
        "change",
        function () {

            var checkInVal = $(this).val();

            if (!isValidDateString(checkInVal)) {

                $("#checkOutDate").attr(
                    "min",
                    getTodayString()
                );

                return;

            }

            var nextDay = new Date(
                checkInVal + "T00:00:00"
            );

            nextDay.setDate(
                nextDay.getDate() + 1
            );

            var nextDayStr =
                formatDate(nextDay);

            $("#checkOutDate").attr(
                "min",
                nextDayStr
            );

            var checkOutVal =
                $("#checkOutDate").val();

            if (
                checkOutVal &&
                checkOutVal <= checkInVal
            ) {

                $("#checkOutDate").val("");

            }

        }
    );

    function checkAvailability(
        checkInVal,
        checkOutVal
    ) {

        $("#availabilityMsg").html(
            "<span style='color:#888;'>"
            + "Checking availability..."
            + "</span>"
        );

        isRoomAvailable = false;

        setSubmitEnabled(false);

        $.ajax({

            url: checkAvailabilityUrl,

            type: "GET",

            dataType: "json",

            data: {

                roomId: roomId,

                checkIn: checkInVal,

                checkOut: checkOutVal

            },

            success: function (res) {

                if (res && res.available === true) {

                    $("#availabilityMsg").html(
                        "<span style='color:#198754;'>"
                        + "<i class='fa-solid fa-circle-check'></i> "
                        + "This room is available for the selected dates."
                        + "</span>"
                    );

                    isRoomAvailable = true;

                    setSubmitEnabled(true);

                }
                else {

                    $("#availabilityMsg").html(
                        "<span style='color:#c0392b;'>"
                        + "<i class='fa-solid fa-circle-xmark'></i> "
                        + ((res && res.message)
                            || "This room is not available.")
                        + "</span>"
                    );

                    isRoomAvailable = false;

                    setSubmitEnabled(false);

                }

            },

            error: function () {

                $("#availabilityMsg").html(
                    "<span style='color:#c0392b;'>"
                    + "Unable to check room availability. Please try again."
                    + "</span>"
                );

                isRoomAvailable = false;

                setSubmitEnabled(false);

            }

        });

    }

    function recalcTotal() {

        var checkInVal =
            $("#checkInDate").val();

        var checkOutVal =
            $("#checkOutDate").val();

        resetAvailability();

        if (
            !isValidDateString(checkInVal) ||
            !isValidDateString(checkOutVal)
        ) {

            $("#rdTotal").text(
                "Select dates to see total"
            );

            return;

        }

        var checkIn =
            new Date(
                checkInVal + "T00:00:00"
            );

        var checkOut =
            new Date(
                checkOutVal + "T00:00:00"
            );

        var nights =
            Math.round(
                (checkOut - checkIn)
                / (1000 * 60 * 60 * 24)
            );

        if (nights <= 0) {

            $("#rdTotal").html(
                "<span style='color:#c0392b;'>"
                + "Check-out date must be after check-in date."
                + "</span>"
            );

            return;

        }

        if (nights > MAX_NIGHTS) {

            $("#rdTotal").html(
                "<span style='color:#c0392b;'>"
                + "Please select a valid date range "
                + "(max "
                + MAX_NIGHTS
                + " nights)."
                + "</span>"
            );

            return;

        }

        var totalPrice =
            nights * pricePerNight;

        $("#rdTotal").text(
            nights
            + " Night(s) × ₹"
            + pricePerNight.toLocaleString("en-IN")
            + " = ₹"
            + totalPrice.toLocaleString("en-IN")
        );

        checkAvailability(
            checkInVal,
            checkOutVal
        );

    }

    $("#checkInDate, #checkOutDate").on(
        "change",
        recalcTotal
    );

    $bookingForm.on(
        "submit",
        function (e) {

            e.preventDefault();

            if (!isAuthenticated) {

                window.location.href =
                    loginUrl
                    + "?returnUrl="
                    + encodeURIComponent(
                        window.location.pathname
                        + window.location.search
                    );

                return;

            }

            if (!isRoomAvailable) {

                $("#bookingAlert").html(
                    '<div class="alert alert-danger">'
                    + "Please select valid available dates before submitting."
                    + "</div>"
                );

                return;

            }

            var $form = $(this);

            var $btn =
                $form.find(
                    "button[type=submit]"
                );

            $btn
                .prop("disabled", true)
                .text("Submitting...");

            $.ajax({

                url: createBookingUrl,

                type: "POST",

                data: $form.serialize(),

                success: function (res) {

                    var alertClass =
                        res && res.success
                            ? "alert alert-success"
                            : "alert alert-danger";

                    $("#bookingAlert").html(
                        '<div class="'
                        + alertClass
                        + '">'
                        + ((res && res.message)
                            || "Unable to process booking request.")
                        + "</div>"
                    );

                    if (res && res.success) {

                        $form[0].reset();

                        $("#availabilityMsg").html("");

                        $("#rdTotal").text(
                            "Select dates to see total"
                        );

                        isRoomAvailable = false;

                        setSubmitEnabled(false);

                        $("#checkInDate").attr(
                            "min",
                            getTodayString()
                        );

                        $("#checkOutDate").removeAttr(
                            "min"
                        );

                    }
                    else {

                        setSubmitEnabled(
                            isRoomAvailable
                        );

                    }

                },

                error: function () {

                    $("#bookingAlert").html(
                        '<div class="alert alert-danger">'
                        + "Something went wrong. Please try again."
                        + "</div>"
                    );

                    setSubmitEnabled(
                        isRoomAvailable
                    );

                },

                complete: function () {

                    if (!isRoomAvailable) {

                        $btn
                            .prop("disabled", true)
                            .text("Request Booking");

                    }
                    else {

                        $btn
                            .prop("disabled", false)
                            .text("Request Booking");

                    }

                }

            });

        }
    );

});