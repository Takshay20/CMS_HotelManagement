$(document).ready(function () {

    // Phone number field typing
    $("#phoneNumber").on("input", function () {
        this.value = this.value.replace(/[^0-9]/g, "");
        $("#phoneError").text("");

        $(this).removeClass(
            "input-error input-valid"
        );
    });

    // Country code field change
    $("#countryCode").on("change", function () {
        var length = parseInt(
            $(this)
                .find(":selected")
                .data("length"),
            10
        );
        $("#phoneNumber")
            .val("")
            .attr("maxlength", length);
        $("#phoneError").text("");
        $("#phoneNumber")
            .removeClass(
                "input-error input-valid"
            );
    });

    // Name field typing
    $("#name").on("input", function () {
        $("#nameError").text("");
        $(this).removeClass(
            "input-error input-valid"
        );
    });

    // Email field typing
    $("#email").on("input", function () {
        $("#emailError").text("");
        $(this).removeClass(
            "input-error input-valid"
        );
    });

    // Subject field change
    $("#subject").on("change", function () {
        $("#subjectError").text("");
        $(this).removeClass(
            "input-error input-valid"
        );
    });

    // Contact form form submit
    $("#contactForm").on("submit", function (e) {
        e.preventDefault();
        var isValid = true;
        var name = $("#name").val().trim();
        var phone = $("#phoneNumber").val().trim();
        var email = $("#email").val().trim();
        var subject = $("#subject").val();
        var countryCode = $("#countryCode").val();
        $(".field-error").text("");
        $("#contactForm")
            .find("input, select, textarea")
            .removeClass(
                "input-error input-valid"
            );
        if (name === "") {
            $("#nameError").text(
                "Full Name is required."
            );
            $("#name").addClass(
                "input-error"
            );
            isValid = false;
        }
        else if (name.length < 3) {
            $("#nameError").text(
                "Full Name must contain at least 3 characters."
            );
            $("#name").addClass(
                "input-error"
            );
            isValid = false;
        }
        else {
            $("#name").addClass(
                "input-valid"
            );
        }
        var requiredPhoneLength =
            parseInt(
                $("#countryCode")
                    .find(":selected")
                    .data("length"),
                10
            );
        if (phone === "") {
            $("#phoneError").text(
                "Phone number is required."
            );
            $("#phoneNumber").addClass(
                "input-error"
            );
            isValid = false;
        }
        else if (!/^[0-9]+$/.test(phone)) {
            $("#phoneError").text(
                "Phone number must contain digits only."
            );
            $("#phoneNumber").addClass(
                "input-error"
            );
            isValid = false;
        }
        else if (
            phone.length !== requiredPhoneLength
        ) {
            $("#phoneError").text(
                "Please enter a valid phone number."
            );
            $("#phoneNumber").addClass(
                "input-error"
            );
            isValid = false;
        }
        else {
            $("#phoneNumber").addClass(
                "input-valid"
            );
        }
        var emailRegex =
            /^[A-Za-z0-9._%+-]+@[A-Za-z0-9-]+(\.[A-Za-z0-9-]+)+$/;
        if (email === "") {
            $("#emailError").text(
                "Email address is required. Format: example@gmail.com"
            );
            $("#email").addClass(
                "input-error"
            );
            isValid = false;
        }
        else if (!emailRegex.test(email)) {
            $("#emailError").text(
                "Invalid email format. Example: example@gmail.com"
            );
            $("#email").addClass(
                "input-error"
            );
            isValid = false;
        }
        else {
            $("#email").addClass(
                "input-valid"
            );
        }
        if (!subject) {
            $("#subjectError").text(
                "Please select a subject."
            );
            $("#subject").addClass(
                "input-error"
            );
            isValid = false;
        }
        else {
            $("#subject").addClass(
                "input-valid"
            );
        }
        if (!isValid) {
            return;
        }
        $("#fullPhone").val(
            countryCode + phone
        );
        var $btn = $(this).find(
            "button[type=submit]"
        );
        $btn
            .prop("disabled", true)
            .text("Sending...");

        // AJAX call to /Contact/Submit
        $.ajax({
            url: "/Contact/Submit",
            type: "POST",
            data: $(this).serialize(),
            success: function (res) {

                if (res.loginRequired) {
                    alert(
                        res.message ||
                        "Please login first."
                    );
                    window.location.href =
                        "/Account/Login";
                    return;
                }
                if (res.success) {
                    $("#contactAlert").html(
                        '<div class="contact-alert success">'
                        + (res.message ||
                            "Message sent successfully.")
                        + "</div>"
                    );
                    $("#contactForm")[0].reset();
                    $("#phoneNumber").val("");
                    $("#fullPhone").val("");
                    $(".field-error").text("");
                    $("#contactForm")
                        .find(
                            "input, select, textarea"
                        )
                        .removeClass(
                            "input-error input-valid"
                        );
                }
                else {
                    $("#contactAlert").html(
                        '<div class="contact-alert error">'
                        + (res.message ||
                            "Something went wrong.")
                        + "</div>"
                    );
                }
            },
            error: function () {
                $("#contactAlert").html(
                    '<div class="contact-alert error">'
                    + "Something went wrong. Please try again."
                    + "</div>"
                );
            },
            complete: function () {
                $btn
                    .prop("disabled", false)
                    .text("Send Message");
            }
        });
    });
});