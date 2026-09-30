$(document).ready(function () {
    // Common
    const phoneLengths = {
        "+91": 10,
        "+1": 10,
        "+44": 10,
        "+61": 9,
        "+971": 9,
        "+65": 8,
        "+81": 10,
        "+49": 10,
        "+33": 9
    };

    function setError(input, error, message) {
        $(input)
            .removeClass("input-valid")
            .addClass("input-error");
        $(error)
            .text(message)
            .addClass("show");
    }

    function setValid(input, error) {
        $(input)
            .removeClass("input-error")
            .addClass("input-valid");
        $(error)
            .text("")
            .removeClass("show");
    }

    function clearValidation(input, error) {
        $(input)
            .removeClass("input-error input-valid");
        $(error)
            .text("")
            .removeClass("show");
    }

    function isValidEmail(value) {
        const emailRegex = /^[A-Za-z0-9._%+-]+@[A-Za-z0-9-]+(\.[A-Za-z0-9-]+)+$/;
        return emailRegex.test(value);
    }

    // Register - Full Name
    function validateName() {
        const value = $("#FullName").val().trim();

        if (value === "") {
            setError(
                "#FullName",
                "#fullNameError",
                "Full Name is required."
            );
            return false;
        }

        if (value.length < 3) {
            setError(
                "#FullName",
                "#fullNameError",
                "Full Name must be at least 3 characters."
            );
            return false;
        }

        setValid(
            "#FullName",
            "#fullNameError"
        );

        return true;
    }

    // Register - Email
    function validateEmail() {
        const value = $("#Email").val().trim();

        if (value === "") {
            setError(
                "#Email",
                "#emailError",
                "Email address is required."
            );
            return false;
        }

        if (!isValidEmail(value)) {
            setError(
                "#Email",
                "#emailError",
                "Please enter a valid email address."
            );
            return false;
        }

        setValid(
            "#Email",
            "#emailError"
        );

        return true;
    }

    // Register - Phone
    function validatePhone() {
        const code = $("#CountryCode").val();
        const value = $("#Phone").val().trim();
        const requiredLength = phoneLengths[code] || 10;

        if (value === "") {
            setError("#Phone", "#phoneError", "Phone number is required.");
            return false;
        }

        if (!/^[0-9]+$/.test(value)) {
            setError("#Phone", "#phoneError", "Please enter numbers only.");
            return false;
        }

        if (/^(\d)\1+$/.test(value)) {
            setError("#Phone", "#phoneError", "Please enter a valid phone number.");
            return false;
        }

        if (code === "+91" && !/^[6-9][0-9]{9}$/.test(value)) {
            setError("#Phone", "#phoneError", "Please enter a valid 10 digit mobile number.");
            return false;
        }

        if (code !== "+91" && value.length !== requiredLength) {
            setError("#Phone", "#phoneError", "Please enter a valid phone number.");
            return false;
        }

        setValid("#Phone", "#phoneError");
        return true;
    }

    // Register - Password
    function validatePassword() {
        const value = $("#Password").val();

        if (value === "") {
            setError("#Password", "#passwordError", "Password is required.");
            return false;
        }

        if (value.length < 6) {
            setError("#Password", "#passwordError", "Password must be at least 6 characters.");
            return false;
        }

        if (!/[A-Z]/.test(value)) {
            setError("#Password", "#passwordError", "Password must contain one uppercase letter.");
            return false;
        }

        if (!/[a-z]/.test(value)) {
            setError("#Password", "#passwordError", "Password must contain one lowercase letter.");
            return false;
        }

        if (!/[0-9]/.test(value)) {
            setError("#Password", "#passwordError", "Password must contain one number.");
            return false;
        }

        if (!/[^A-Za-z0-9]/.test(value)) {
            setError("#Password", "#passwordError", "Password must contain one special character.");
            return false;
        }

        setValid("#Password", "#passwordError");
        return true;
    }

    // Register - Confirm Password
    function validateConfirmPassword() {
        const password = $("#Password").val();
        const confirmPassword = $("#ConfirmPassword").val();

        if (confirmPassword === "") {
            setError(
                "#ConfirmPassword",
                "#confirmPasswordError",
                "Confirm Password is required."
            );
            return false;
        }

        if (password !== confirmPassword) {
            setError(
                "#ConfirmPassword",
                "#confirmPasswordError",
                "Passwords do not match."
            );
            return false;
        }

        setValid(
            "#ConfirmPassword",
            "#confirmPasswordError"
        );

        return true;
    }

    // Register - Full Name Input
    $("#FullName").on("input", function () {
        if ($(this).val().trim() === "") {
            clearValidation(
                "#FullName",
                "#fullNameError"
            );
        } else {
            validateName();
        }
    });

    // Register - Email Input
    $("#Email").on("input", function () {
        if ($(this).val().trim() === "") {
            clearValidation(
                "#Email",
                "#emailError"
            );
        } else {
            validateEmail();
        }
    });

    // Register - Phone Input
    $("#Phone").on("input", function () {
        const code = $("#CountryCode").val();
        const max = phoneLengths[code] || 10;

        this.value = this.value
            .replace(/[^0-9]/g, "")
            .substring(0, max);

        if (this.value === "") {
            clearValidation(
                "#Phone",
                "#phoneError"
            );
        } else {
            validatePhone();
        }
    });

    // Register - Country Code
    $("#CountryCode").on("change", function () {
        const code = $(this).val();
        const max = phoneLengths[code] || 10;

        $("#Phone")
            .val("")
            .attr("maxlength", max);

        clearValidation(
            "#Phone",
            "#phoneError"
        );
    });

    // Register - Password Input
    $("#Password").on("input", function () {
        if ($(this).val() === "") {
            clearValidation(
                "#Password",
                "#passwordError"
            );
        } else {
            validatePassword();
        }

        if ($("#ConfirmPassword").val() !== "") {
            validateConfirmPassword();
        }
    });

    // Register - Confirm Password Input
    $("#ConfirmPassword").on("input", function () {
        if ($(this).val() === "") {
            clearValidation(
                "#ConfirmPassword",
                "#confirmPasswordError"
            );
        } else {
            validateConfirmPassword();
        }
    });

    // Register Submit
    $("#registerForm").on("submit", function (e) {
        e.preventDefault();

        $("#validationList").empty();

        let valid = true;

        if (!validateName()) {
            $("#validationList")
                .append("<li>Full Name is required or invalid.</li>");
            valid = false;
        }

        if (!validateEmail()) {
            $("#validationList")
                .append("<li>Email address is required or invalid.</li>");
            valid = false;
        }

        if (!validatePhone()) {
            $("#validationList")
                .append("<li>Phone number is required or invalid.</li>");
            valid = false;
        }

        if (!validatePassword()) {
            $("#validationList")
                .append("<li>Password is required or invalid.</li>");
            valid = false;
        }

        if (!validateConfirmPassword()) {
            $("#validationList")
                .append("<li>Confirm Password is invalid.</li>");
            valid = false;
        }

        if (!valid) {
            $("#validationSummary")
                .addClass("show");

            $("html, body").animate({
                scrollTop: $("#validationSummary").offset().top - 30
            }, 300);

            return;
        }

        $("#validationSummary")
            .removeClass("show");

        this.submit();
    });

    // Login
    function validateLoginEmail() {
        const value = $("#LoginEmail").length
            ? $("#LoginEmail").val().trim()
            : $("#Email").val().trim();

        const input = $("#LoginEmail").length
            ? "#LoginEmail"
            : "#Email";

        const error = $("#loginEmailError").length
            ? "#loginEmailError"
            : "#emailError";

        if (value === "") {
            setError(
                input,
                error,
                "Email address is required."
            );
            return false;
        }

        if (!isValidEmail(value)) {
            setError(
                input,
                error,
                "Please enter a valid email address."
            );
            return false;
        }

        setValid(input, error);
        return true;
    }

    function validateLoginPassword() {
        const input = $("#LoginPassword").length
            ? "#LoginPassword"
            : "#Password";

        const error = $("#loginPasswordError").length
            ? "#loginPasswordError"
            : "#passwordError";

        const value = $(input).val();

        if (value === "") {
            setError(
                input,
                error,
                "Password is required."
            );
            return false;
        }

        setValid(input, error);
        return true;
    }

    $("#loginForm").on("submit", function (e) {
        e.preventDefault();

        let valid = true;

        if (!validateLoginEmail()) {
            valid = false;
        }

        if (!validateLoginPassword()) {
            valid = false;
        }

        const loginAs = $("#LoginAs").val();

        if (!loginAs) {
            if ($("#loginAsError").length) {
                $("#loginAsError")
                    .text("Please select Login As.")
                    .addClass("show");
            }

            valid = false;
        } else {
            if ($("#loginAsError").length) {
                $("#loginAsError")
                    .text("")
                    .removeClass("show");
            }
        }

        if (!valid) {
            return;
        }

        this.submit();
    });

    // Forgot Password
    $("#forgotPasswordForm").on("submit", function (e) {
        e.preventDefault();

        const emailInput = $("#forgotPasswordForm [name='email']");
        const email = emailInput.val().trim();

        if (email === "") {
            setError(
                emailInput,
                "#emailError",
                "Email address is required."
            );
            return;
        }

        if (!isValidEmail(email)) {
            setError(
                emailInput,
                "#emailError",
                "Please enter a valid email address."
            );
            return;
        }

        clearValidation(
            emailInput,
            "#emailError"
        );

        this.submit();
    });

    // Verify Reset Code
    $("#verifyResetCodeForm").on("submit", function (e) {
        e.preventDefault();

        let valid = true;

        const codeInput = $("#verifyResetCodeForm [name='code']");
        const passwordInput = $("#verifyResetCodeForm [name='newPassword']");
        const confirmInput = $("#verifyResetCodeForm [name='confirmPassword']");

        const code = codeInput.val().trim();
        const password = passwordInput.val();
        const confirmPassword = confirmInput.val();

        if (code === "") {
            setError(
                codeInput,
                "#codeError",
                "Verification code is required."
            );
            valid = false;
        } else {
            clearValidation(
                codeInput,
                "#codeError"
            );
        }

        if (password === "") {
            setError(
                passwordInput,
                "#newPasswordError",
                "New password is required."
            );
            valid = false;
        } else if (password.length < 6) {
            setError(
                passwordInput,
                "#newPasswordError",
                "Password must be at least 6 characters."
            );
            valid = false;
        } else {
            clearValidation(
                passwordInput,
                "#newPasswordError"
            );
        }

        if (confirmPassword === "") {
            setError(
                confirmInput,
                "#confirmPasswordError",
                "Confirm Password is required."
            );
            valid = false;
        } else if (password !== confirmPassword) {
            setError(
                confirmInput,
                "#confirmPasswordError",
                "Passwords do not match."
            );
            valid = false;
        } else {
            clearValidation(
                confirmInput,
                "#confirmPasswordError"
            );
        }

        if (!valid) {
            return;
        }

        this.submit();
    });

    // Remove Validation On Typing
    $(document).on("input", "input", function () {
        const value = $(this).val().trim();

        if (value !== "") {
            const id = $(this).attr("id");

            if (id === "FullName") {
                validateName();
            }

            if (id === "Email" && !$("#registerForm").length) {
                validateLoginEmail();
            }
        }
    });

    // Success Message
    const successMessage = $("#successMessage").val();

    if (successMessage) {
        Swal.fire({
            icon: "success",
            title: "Success",
            text: successMessage,
            confirmButtonText: "OK"
        });
    }
});