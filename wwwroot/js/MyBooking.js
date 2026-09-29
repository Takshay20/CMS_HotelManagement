$(document).ready(function () {

    $("#feedbackForm").on("submit", function (e) {

        e.preventDefault();

        var formData = new FormData(this);

        var $btn = $(this).find("button[type=submit]");

        $btn
            .prop("disabled", true)
            .text("Submitting...");

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