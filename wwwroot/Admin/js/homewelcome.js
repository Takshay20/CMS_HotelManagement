$(document).ready(function () {

    $("#homeWelcomeForm").on("submit", function (e) {
        e.preventDefault();

        var isValid = true;

        $(".field-error").text("");

        var heading = $("#Heading").val().trim();
        var subHeading = $("#SubHeading").val().trim();
        var description = $("#Description").val().trim();
        var buttonText = $("#ButtonText").val().trim();
        var buttonUrl = $("#ButtonUrl").val().trim();
        var experienceYears = $("#ExperienceYears").val().trim();

        var image1Input = $("#Image1File")[0];
        var image2Input = $("#Image2File")[0];
        var image3Input = $("#Image3File")[0];

        var image1Selected = image1Input.files.length > 0;
        var image2Selected = image2Input.files.length > 0;
        var image3Selected = image3Input.files.length > 0;

        var existingImage1 = $("input[name='HomeWelcome.Image1Path']").val() || "";
        var existingImage2 = $("input[name='HomeWelcome.Image2Path']").val() || "";
        var existingImage3 = $("input[name='HomeWelcome.Image3Path']").val() || "";

        // Heading validation
        if (heading === "") {
            $("#err-Heading").text("Heading is required.");
            isValid = false;
        }

        // Sub Heading validation
        if (subHeading === "") {
            $("#err-SubHeading").text("Sub Heading is required.");
            isValid = false;
        }

        // Description validation
        if (description === "") {
            $("#err-Description").text("Description is required.");
            isValid = false;
        }

        // Button Text validation
        if (buttonText === "") {
            $("#err-ButtonText").text("Button Text is required.");
            isValid = false;
        }

        // Button URL validation
        if (buttonUrl === "") {
            $("#err-ButtonUrl").text("Button URL is required.");
            isValid = false;
        }

        // Experience Years validation
        if (experienceYears === "") {
            $("#err-ExperienceYears").text("Experience Years is required.");
            isValid = false;
        }

        // Image 1 validation
        if (!image1Selected && existingImage1 === "") {
            $("#err-Image1File").text("Image 1 is required.");
            isValid = false;
        } else if (image1Selected) {
            if (!validateImage(image1Input, "#err-Image1File")) {
                isValid = false;
            }
        }

        // Image 2 validation
        if (!image2Selected && existingImage2 === "") {
            $("#err-Image2File").text("Image 2 is required.");
            isValid = false;
        } else if (image2Selected) {
            if (!validateImage(image2Input, "#err-Image2File")) {
                isValid = false;
            }
        }

        // Image 3 validation
        if (!image3Selected && existingImage3 === "") {
            $("#err-Image3File").text("Image 3 is required.");
            isValid = false;
        } else if (image3Selected) {
            if (!validateImage(image3Input, "#err-Image3File")) {
                isValid = false;
            }
        }

        // Stop form submission if validation fails
        if (!isValid) {
            return;
        }

        var form = this;
        var formData = new FormData(form);

        var $btn = $(form).find("button[type='submit']");

        $btn.prop("disabled", true);
        $btn.text("Updating...");

        // AJAX request
        $.ajax({
            url: "/Admin/HomeWelcome/Update",
            type: "POST",
            data: formData,
            processData: false,
            contentType: false,

            success: function (response) {

                if (response.success) {

                    Swal.fire({
                        icon: "success",
                        title: response.message,
                        timer: 1500,
                        showConfirmButton: false
                    }).then(function () {
                        location.reload();
                    });

                } else {

                    Swal.fire({
                        icon: "error",
                        title: response.message,
                        confirmButtonText: "OK"
                    });
                }
            },

            error: function (xhr) {

                console.log(xhr.responseText);

                Swal.fire({
                    icon: "error",
                    title: "Something went wrong.",
                    text: "Unable to process your request.",
                    confirmButtonText: "OK"
                });
            },

            complete: function () {

                $btn.prop("disabled", false);
                $btn.text("Update Changes");
            }
        });
    });

    // Image format validation
    function validateImage(input, errorSelector) {

        if (!input.files || input.files.length === 0) {
            $(errorSelector).text("Image is required.");
            return false;
        }

        var file = input.files[0];
        var fileName = file.name.toLowerCase();
        var extension = fileName.split(".").pop();

        var allowedExtensions = [
            "jpg",
            "jpeg",
            "png",
            "gif",
            "jfif"
        ];

        var allowedMimeTypes = [
            "image/jpeg",
            "image/png",
            "image/gif",
            "image/jfif"
        ];

        if ($.inArray(extension, allowedExtensions) === -1) {
            $(errorSelector).text(
                "Only JPG, JPEG, PNG, GIF and JFIF images are allowed."
            );

            input.value = "";

            return false;
        }

        if ($.inArray(file.type, allowedMimeTypes) === -1) {
            $(errorSelector).text(
                "Invalid image format. Only JPG, JPEG, PNG, GIF and JFIF are allowed."
            );

            input.value = "";

            return false;
        }

        $(errorSelector).text("");

        return true;
    }

    // Heading input validation
    $("#Heading").on("input", function () {

        if ($(this).val().trim() !== "") {
            $("#err-Heading").text("");
        }
    });

    // Sub Heading input validation
    $("#SubHeading").on("input", function () {

        if ($(this).val().trim() !== "") {
            $("#err-SubHeading").text("");
        }
    });

    // Description input validation
    $("#Description").on("input", function () {

        if ($(this).val().trim() !== "") {
            $("#err-Description").text("");
        }
    });

    // Button Text input validation
    $("#ButtonText").on("input", function () {

        if ($(this).val().trim() !== "") {
            $("#err-ButtonText").text("");
        }
    });

    // Button URL input validation
    $("#ButtonUrl").on("input", function () {

        if ($(this).val().trim() !== "") {
            $("#err-ButtonUrl").text("");
        }
    });

    // Experience Years input validation
    $("#ExperienceYears").on("input", function () {

        if ($(this).val().trim() !== "") {
            $("#err-ExperienceYears").text("");
        }
    });

    // Image 1 change validation
    $("#Image1File").on("change", function () {

        if (this.files.length === 0) {
            return;
        }

        validateImage(this, "#err-Image1File");
    });

    // Image 2 change validation
    $("#Image2File").on("change", function () {

        if (this.files.length === 0) {
            return;
        }

        validateImage(this, "#err-Image2File");
    });

    // Image 3 change validation
    $("#Image3File").on("change", function () {

        if (this.files.length === 0) {
            return;
        }

        validateImage(this, "#err-Image3File");
    });

});