$(document).ready(function () {

    $("#aboutStoryForm").on("submit", function (e) {

        e.preventDefault();

        var isValid = true;

        $(".field-error").text("");

        var heading = $("#Heading").val().trim();

        var subHeading = $("#SubHeading").val().trim();

        var description = $("#Description").val().trim();

        var imageInput = $("#ImageFile")[0];

        var imageSelected =
            imageInput &&
            imageInput.files &&
            imageInput.files.length > 0;

        var existingImage =
            $("input[name='AboutStory.ImagePath']").val() || "";

        // Heading validation
        if (heading === "") {

            $("#err-Heading").text(
                "Heading is required."
            );

            isValid = false;
        }

        // Sub Heading validation
        if (subHeading === "") {

            $("#err-SubHeading").text(
                "Sub Heading is required."
            );

            isValid = false;
        }

        // Description validation
        if (description === "") {

            $("#err-Description").text(
                "Description is required."
            );

            isValid = false;
        }

        // Image validation
        if (!imageSelected &&
            existingImage.trim() === "") {

            $("#err-ImageFile").text(
                "Image is required."
            );

            isValid = false;
        }

        // New image validation
        if (imageSelected) {

            if (!validateImage(imageInput)) {
                isValid = false;
            }
        }

        // Stop submission if validation fails
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

            url: "/Admin/AboutStory/Update",

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

                    text: "Unable to update Story Content.",

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

    // Image validation
    function validateImage(input) {

        if (!input.files ||
            input.files.length === 0) {

            $("#err-ImageFile").text(
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

            $("#err-ImageFile").text(
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

            $("#err-ImageFile").text(
                "Invalid image format. Only JPG, JPEG, PNG, GIF and JFIF are allowed."
            );

            input.value = "";

            return false;
        }

        $("#err-ImageFile").text("");

        return true;
    }

    // Heading validation while typing
    $("#Heading").on("input", function () {

        if ($(this).val().trim() !== "") {

            $("#err-Heading").text("");
        }
    });

    // Sub Heading validation while typing
    $("#SubHeading").on("input", function () {

        if ($(this).val().trim() !== "") {

            $("#err-SubHeading").text("");
        }
    });

    // Description validation while typing
    $("#Description").on("input", function () {

        if ($(this).val().trim() !== "") {

            $("#err-Description").text("");
        }
    });

    // Image change validation and preview
    $("#ImageFile").on("change", function () {

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

            $("#imagePreview").html(

                "<img src='" +
                e.target.result +
                "' class='img-preview' alt='Story Image' />"

            );
        };

        reader.readAsDataURL(file);
    });

});