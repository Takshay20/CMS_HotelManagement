// Page load: setup and event handlers
$(document).ready(function () {

    // Home welcome form form submit
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

        if (heading === "") {
            $("#err-Heading").text("Heading is required.");
            isValid = false;
        }

        if (subHeading === "") {
            $("#err-SubHeading").text("Sub Heading is required.");
            isValid = false;
        }

        if (description === "") {
            $("#err-Description").text("Description is required.");
            isValid = false;
        }

        if (buttonText === "") {
            $("#err-ButtonText").text("Button Text is required.");
            isValid = false;
        }

        if (buttonUrl === "") {
            $("#err-ButtonUrl").text("Button URL is required.");
            isValid = false;
        }

        if (experienceYears === "") {
            $("#err-ExperienceYears").text("Experience Years is required.");
            isValid = false;
        }

        if (!image1Selected && existingImage1 === "") {
            $("#err-Image1File").text("Image 1 is required.");
            isValid = false;
        } else if (image1Selected) {
            if (!validateImage(image1Input, "#err-Image1File")) {
                isValid = false;
            }
        }

        if (!image2Selected && existingImage2 === "") {
            $("#err-Image2File").text("Image 2 is required.");
            isValid = false;
        } else if (image2Selected) {
            if (!validateImage(image2Input, "#err-Image2File")) {
                isValid = false;
            }
        }

        if (!image3Selected && existingImage3 === "") {
            $("#err-Image3File").text("Image 3 is required.");
            isValid = false;
        } else if (image3Selected) {
            if (!validateImage(image3Input, "#err-Image3File")) {
                isValid = false;
            }
        }

        if (!isValid) {
            return;
        }

        var form = this;

        // Prepare form data for upload
        var formData = new FormData(form);

        var $btn = $(form).find("button[type='submit']");

        $btn.prop("disabled", true);
        $btn.text("Updating...");

        // AJAX call to /Admin/HomeWelcome/Update
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

    // Validate image
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

    // Heading field typing
    $("#Heading").on("input", function () {

        if ($(this).val().trim() !== "") {
            $("#err-Heading").text("");
        }
    });

    // Sub heading field typing
    $("#SubHeading").on("input", function () {

        if ($(this).val().trim() !== "") {
            $("#err-SubHeading").text("");
        }
    });

    // Description field typing
    $("#Description").on("input", function () {

        if ($(this).val().trim() !== "") {
            $("#err-Description").text("");
        }
    });

    // Button text field typing
    $("#ButtonText").on("input", function () {

        if ($(this).val().trim() !== "") {
            $("#err-ButtonText").text("");
        }
    });

    // Button url field typing
    $("#ButtonUrl").on("input", function () {

        if ($(this).val().trim() !== "") {
            $("#err-ButtonUrl").text("");
        }
    });

    // Experience years field typing
    $("#ExperienceYears").on("input", function () {

        if ($(this).val().trim() !== "") {
            $("#err-ExperienceYears").text("");
        }
    });

    // Image1 file field change
    $("#Image1File").on("change", function () {

        if (this.files.length === 0) {
            return;
        }

        validateImage(this, "#err-Image1File");
    });

    // Image2 file field change
    $("#Image2File").on("change", function () {

        if (this.files.length === 0) {
            return;
        }

        validateImage(this, "#err-Image2File");
    });

    // Image3 file field change
    $("#Image3File").on("change", function () {

        if (this.files.length === 0) {
            return;
        }

        validateImage(this, "#err-Image3File");
    });

});