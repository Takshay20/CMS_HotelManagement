$(document).ready(function () {

    // Feedback form form submit
    $("#feedbackForm").on("submit", function (e) {

        e.preventDefault();

        // Prepare form data for upload
        var formData = new FormData(this);

        var $btn = $(this).find("button[type=submit]");

        $btn
            .prop("disabled", true)
            .text("Submitting...");

        // AJAX call to /Home/SubmitFeedback
        $.ajax({

            url: "/Home/SubmitFeedback",

            type: "POST",

            data: formData,

            contentType: false,

            processData: false,

            success: function (res) {

                var alertClass =
                    res.success
                        ? "alert alert-success"
                        : "alert alert-danger";

                $("#feedbackAlert").html(
                    '<div class="' +
                    alertClass +
                    '">' +
                    (res.message || "") +
                    "</div>"
                );

                if (res.success) {
                    $("#feedbackForm")[0].reset();
                }

            },

            error: function () {

                $("#feedbackAlert").html(
                    '<div class="alert alert-danger">' +
                    "Something went wrong. Please try again." +
                    "</div>"
                );

            },

            complete: function () {

                $btn
                    .prop("disabled", false)
                    .text("Submit Feedback");

            }

        });

    });


    window.respondRoomChange = function (bookingId, accept) {

        var title = accept
            ? "Continue With New Room?"
            : "Cancel Booking?";

        var text = accept
            ? "Are you sure you want to continue with the proposed room?"
            : "Are you sure you want to reject the room change and cancel this booking?";

        Swal.fire({
            title: title,
            text: text,
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: accept
                ? "Yes, Continue"
                : "Yes, Cancel Booking",
            cancelButtonText: "No",
            reverseButtons: true
        }).then(function (result) {

            if (!result.isConfirmed) {
                return;
            }

            Swal.fire({
                title: "Processing...",
                text: "Please wait.",
                allowOutsideClick: false,
                allowEscapeKey: false,
                didOpen: function () {
                    Swal.showLoading();
                }
            });

            // AJAX call to /Booking/RespondRoomChange
            $.ajax({

                url: "/Booking/RespondRoomChange",

                type: "POST",

                data: {
                    id: bookingId,
                    accept: accept
                },

                success: function (res) {

                    if (res.success) {

                        Swal.fire({
                            title: "Success!",
                            text: res.message ||
                                "Room change response submitted successfully.",
                            icon: "success",
                            confirmButtonText: "OK"
                        }).then(function () {

                            location.reload();

                        });

                    }
                    else {

                        Swal.fire({
                            title: "Error!",
                            text: res.message ||
                                "Unable to process room change.",
                            icon: "error"
                        });

                    }

                },

                error: function () {

                    Swal.fire({
                        title: "Error!",
                        text: "Something went wrong. Please try again.",
                        icon: "error"
                    });

                }

            });

        });

    };


    window.cancelBooking = function (
        bookingId,
        checkInDate,
        totalPrice
    ) {

        Swal.fire({
            title: "Cancel Booking?",
            text: "Are you sure you want to cancel this booking?",
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: "Yes, Cancel",
            cancelButtonText: "No",
            reverseButtons: true
        }).then(function (result) {

            if (!result.isConfirmed) {
                return;
            }

            Swal.fire({
                title: "Cancelling...",
                text: "Please wait.",
                allowOutsideClick: false,
                allowEscapeKey: false,
                didOpen: function () {
                    Swal.showLoading();
                }
            });

            // AJAX call to /Booking/Cancel
            $.ajax({

                url: "/Booking/Cancel",

                type: "POST",

                data: {
                    id: bookingId
                },

                success: function (res) {

                    if (res.success) {

                        Swal.fire({
                            title: "Cancelled!",
                            text: res.message ||
                                "Booking cancelled successfully.",
                            icon: "success",
                            confirmButtonText: "OK"
                        }).then(function () {

                            location.reload();

                        });

                    }
                    else {

                        Swal.fire({
                            title: "Error!",
                            text: res.message ||
                                "Unable to cancel booking.",
                            icon: "error"
                        });

                    }

                },

                error: function () {

                    Swal.fire({
                        title: "Error!",
                        text: "Something went wrong. Please try again.",
                        icon: "error"
                    });

                }

            });

        });

    };

});

// Booking Form Validation
function setBookingError(input, error, message) {
    $(input).addClass("input-error");
    $(error).text(message).addClass("show");
}

function clearBookingError(input, error) {
    $(input).removeClass("input-error");
    $(error).text("").removeClass("show");
}

function validateBookingForm() {
    let valid = true;

    var name = $("#FullName").val().trim();
    var email = $("#Email").val().trim();
    var phone = $("#Phone").val().trim();
    var checkIn = $("#CheckInDate").val();
    var checkOut = $("#CheckOutDate").val();
    var guests = $("#Guests").val();

    clearBookingError("#FullName", "#fullNameError");
    clearBookingError("#Email", "#emailError");
    clearBookingError("#Phone", "#phoneError");
    clearBookingError("#CheckInDate", "#checkInError");
    clearBookingError("#CheckOutDate", "#checkOutError");
    clearBookingError("#Guests", "#guestsError");

    if (name === "") {
        setBookingError("#FullName", "#fullNameError", "Full Name is required.");
        valid = false;
    }

    if (email === "") {
        setBookingError("#Email", "#emailError", "Email is required.");
        valid = false;
    } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
        setBookingError("#Email", "#emailError", "Please enter a valid email address.");
        valid = false;
    }

    if (phone === "") {
        setBookingError("#Phone", "#phoneError", "Phone number is required.");
        valid = false;
    } else if (!/^[0-9]+$/.test(phone)) {
        setBookingError("#Phone", "#phoneError", "Please enter numbers only.");
        valid = false;
    }

    if (!checkIn) {
        setBookingError("#CheckInDate", "#checkInError", "Check-in date is required.");
        valid = false;
    }

    if (!checkOut) {
        setBookingError("#CheckOutDate", "#checkOutError", "Check-out date is required.");
        valid = false;
    }

    if (checkIn && checkOut && new Date(checkOut) <= new Date(checkIn)) {
        setBookingError("#CheckOutDate", "#checkOutError", "Check-out must be after check-in.");
        valid = false;
    }

    if (!guests || parseInt(guests) < 1) {
        setBookingError("#Guests", "#guestsError", "Please enter valid number of guests.");
        valid = false;
    }

    return valid;
}

$("#bookingForm").on("submit", function (e) {
    e.preventDefault();

    if (!validateBookingForm()) {
        return;
    }

    var form = this;
    var formData = new FormData(form);
    var $btn = $(form).find("button[type='submit']");

    $btn.prop("disabled", true).text("Submitting...");

    $.ajax({
        url: "/Booking/Create",
        type: "POST",
        data: formData,
        contentType: false,
        processData: false,
        success: function (res) {
            if (res.success) {
                Swal.fire({
                    icon: "success",
                    title: "Booking Submitted",
                    text: "Your booking request has been submitted successfully. After your booking is confirmed, you can make the payment from the Booking page.",
                    confirmButtonText: "OK"
                }).then(function () {
                    window.location.href = "/Booking/MyBookings";
                });
            } else {
                Swal.fire({
                    icon: "error",
                    title: "Booking Failed",
                    text: res.message || "Unable to submit booking."
                });
            }
        },
        error: function () {
            Swal.fire({
                icon: "error",
                title: "Error",
                text: "Something went wrong. Please try again."
            });
        },
        complete: function () {
            $btn.prop("disabled", false).text("Submit Booking");
        }
    });
});