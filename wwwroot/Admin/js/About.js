$(document).ready(function () {

    $("#aboutForm").on("submit", function (e) {

        e.preventDefault();

        var isValid = true;

        $(".field-error").text("");

        var title = $("#Title").val().trim();
        var subTitle = $("#SubTitle").val().trim();
        var shortDescription = $("#ShortDescription").val().trim();
        var fullDescription = $("#FullDescription").val().trim();
        var yearsOfExperience = $("#YearsOfExperience").val().trim();
        var totalRooms = $("#TotalRooms").val().trim();
        var happyGuests = $("#HappyGuests").val().trim();

        var imageInput = $("#Image1File")[0];

        var imageSelected =
            imageInput &&
            imageInput.files &&
            imageInput.files.length > 0;

        var existingImage =
            $("input[name='About.Image']").val() || "";

        // Title validation
        if (title === "") {

            $("#err-Title").text(
                "Title is required."
            );

            isValid = false;
        }

        // Sub Title validation
        if (subTitle === "") {

            $("#err-SubTitle").text(
                "Sub Title is required."
            );

            isValid = false;
        }

        // Short Description validation
        if (shortDescription === "") {

            $("#err-ShortDescription").text(
                "Short Description is required."
            );

            isValid = false;
        }

        // Full Description validation
        if (fullDescription === "") {

            $("#err-FullDescription").text(
                "Full Description is required."
            );

            isValid = false;
        }

        // Years of Experience validation
        if (yearsOfExperience === "") {

            $("#err-YearsOfExperience").text(
                "Years of Experience is required."
            );

            isValid = false;

        } else if (!/^\d+$/.test(yearsOfExperience)) {

            $("#err-YearsOfExperience").text(
                "Years of Experience must contain numbers only."
            );

            isValid = false;
        }

        // Total Rooms validation
        if (totalRooms === "") {

            $("#err-TotalRooms").text(
                "Total Rooms is required."
            );

            isValid = false;

        } else if (!/^\d+$/.test(totalRooms)) {

            $("#err-TotalRooms").text(
                "Total Rooms must contain numbers only."
            );

            isValid = false;
        }

        // Happy Guests validation
        if (happyGuests === "") {

            $("#err-HappyGuests").text(
                "Happy Guests is required."
            );

            isValid = false;

        } else if (!/^\d+$/.test(happyGuests)) {

            $("#err-HappyGuests").text(
                "Happy Guests must contain numbers only."
            );

            isValid = false;
        }

        // Existing image and new image validation
        if (!imageSelected && existingImage.trim() === "") {

            $("#err-Image1File").text(
                "Image is required."
            );

            isValid = false;
        }

        // New image format validation
        if (imageSelected) {

            if (!validateImage(imageInput)) {
                isValid = false;
            }
        }

        // Stop submit if validation fails
        if (!isValid) {
            return;
        }

        var form = this;

        var formData = new FormData(form);

        var $btn =
            $(form).find("button[type='submit']");

        $btn
            .prop("disabled", true)
            .text("Updating...");

        // AJAX request
        $.ajax({

            url: "/Admin/About/Update",

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

                    text: "Unable to update About Page.",

                    confirmButtonText: "OK"

                });
            },

            complete: function () {

                $btn
                    .prop("disabled", false)
                    .text("Update Changes");
            }
        });
    });

    // Image format validation
    function validateImage(input) {

        if (!input.files ||
            input.files.length === 0) {

            $("#err-Image1File").text(
                "Image is required."
            );

            return false;
        }

        var file = input.files[0];

        var fileName =
            file.name.toLowerCase();

        var extension =
            fileName.split(".").pop();

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

        // Extension validation
        if ($.inArray(
            extension,
            allowedExtensions
        ) === -1) {

            $("#err-Image1File").text(
                "Only JPG, JPEG, PNG, GIF and JFIF images are allowed."
            );

            input.value = "";

            return false;
        }

        // MIME type validation
        if (
            file.type !== "" &&
            $.inArray(
                file.type,
                allowedMimeTypes
            ) === -1
        ) {

            $("#err-Image1File").text(
                "Invalid image format. Only JPG, JPEG, PNG, GIF and JFIF are allowed."
            );

            input.value = "";

            return false;
        }

        $("#err-Image1File").text("");

        return true;
    }

    // Title input validation
    $("#Title").on("input", function () {

        if ($(this).val().trim() !== "") {

            $("#err-Title").text("");
        }
    });

    // Sub Title input validation
    $("#SubTitle").on("input", function () {

        if ($(this).val().trim() !== "") {

            $("#err-SubTitle").text("");
        }
    });

    // Short Description input validation
    $("#ShortDescription").on("input", function () {

        if ($(this).val().trim() !== "") {

            $("#err-ShortDescription").text("");
        }
    });

    // Full Description input validation
    $("#FullDescription").on("input", function () {

        if ($(this).val().trim() !== "") {

            $("#err-FullDescription").text("");
        }
    });

    // Years of Experience input validation
    $("#YearsOfExperience").on("input", function () {

        var value = $(this).val().trim();

        if (value === "") {

            $("#err-YearsOfExperience").text(
                "Years of Experience is required."
            );

            return;
        }

        if (!/^\d+$/.test(value)) {

            $("#err-YearsOfExperience").text(
                "Years of Experience must contain numbers only."
            );

            return;
        }

        $("#err-YearsOfExperience").text("");
    });

    // Total Rooms input validation
    $("#TotalRooms").on("input", function () {

        var value = $(this).val().trim();

        if (value === "") {

            $("#err-TotalRooms").text(
                "Total Rooms is required."
            );

            return;
        }

        if (!/^\d+$/.test(value)) {

            $("#err-TotalRooms").text(
                "Total Rooms must contain numbers only."
            );

            return;
        }

        $("#err-TotalRooms").text("");
    });

    // Happy Guests input validation
    $("#HappyGuests").on("input", function () {

        var value = $(this).val().trim();

        if (value === "") {

            $("#err-HappyGuests").text(
                "Happy Guests is required."
            );

            return;
        }

        if (!/^\d+$/.test(value)) {

            $("#err-HappyGuests").text(
                "Happy Guests must contain numbers only."
            );

            return;
        }

        $("#err-HappyGuests").text("");
    });

    // Image change validation and preview
    $("#Image1File").on("change", function () {

        var input = this;

        if (!input.files ||
            input.files.length === 0) {

            return;
        }

        if (!validateImage(input)) {
            return;
        }

        var file = input.files[0];

        var reader = new FileReader();

        reader.onload = function (e) {

            $("#image1Preview").html(

                "<img src='" +
                e.target.result +
                "' class='img-preview' alt='About Image' />"

            );
        };

        reader.readAsDataURL(file);
    });

});