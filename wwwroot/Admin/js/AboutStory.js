
$(document).ready(function () {

    // About story form submit
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

        // Validate heading
        if (heading === "") {
            $("#err-Heading").text("Heading is required.");
            isValid = false;
        }

        // Validate sub heading
        if (subHeading === "") {
            $("#err-SubHeading").text("Sub Heading is required.");
            isValid = false;
        }

        // Validate description
        if (description === "") {
            $("#err-Description").text("Description is required.");
            isValid = false;
        }

        // Require an image only when no existing image is available
        if (!imageSelected && existingImage.trim() === "") {
            $("#err-ImageFile").text("Image is required.");
            isValid = false;
        }

        // Validate a newly selected image
        if (imageSelected && !validateImage(imageInput)) {
            isValid = false;
        }

        if (!isValid) {
            return;
        }

        var form = this;
        var formData = new FormData(form);

        var $btn = $(form).find("button[type='submit']");

        $btn.prop("disabled", true).text("Updating...");

        // Send form data to the server
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
                        title: response.message || "Unable to update Story Content.",
                        confirmButtonText: "OK"
                    });
                }
            },

            error: function (xhr) {

                console.error(xhr.responseText);

                Swal.fire({
                    icon: "error",
                    title: "Something went wrong.",
                    text: "Unable to update Story Content.",
                    confirmButtonText: "OK"
                });
            },

            complete: function () {
                $btn.prop("disabled", false).text("Update Changes");
            }
        });
    });

    // Validate image format
    function validateImage(input) {

        if (!input.files || input.files.length === 0) {
            $("#err-ImageFile").text("Image is required.");
            return false;
        }

        var file = input.files[0];
        var extension = file.name.toLowerCase().split(".").pop();

        var allowedExtensions = [
            "jpg", "jpeg", "png", "gif", "jfif"
        ];

        var allowedMimeTypes = [
            "image/jpeg", "image/png", "image/gif", "image/jfif"
        ];

        if ($.inArray(extension, allowedExtensions) === -1) {
            $("#err-ImageFile").text(
                "Only JPG, JPEG, PNG, GIF and JFIF images are allowed."
            );
            input.value = "";
            return false;
        }

        if (
            file.type !== "" &&
            $.inArray(file.type, allowedMimeTypes) === -1
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

    // Clear heading error while typing
    $("#Heading").on("input", function () {
        if ($(this).val().trim() !== "") {
            $("#err-Heading").text("");
        }
    });

    // Clear sub heading error while typing
    $("#SubHeading").on("input", function () {
        if ($(this).val().trim() !== "") {
            $("#err-SubHeading").text("");
        }
    });

    // Clear description error while typing
    $("#Description").on("input", function () {
        if ($(this).val().trim() !== "") {
            $("#err-Description").text("");
        }
    });

    // Preview selected image
    $("#ImageFile").on("change", function () {

        var input = this;

        if (!input.files || input.files.length === 0) {
            return;
        }

        if (!validateImage(input)) {
            return;
        }

        var reader = new FileReader();

        reader.onload = function (e) {

            $("#imagePreview").html(
                "<img src='" + e.target.result +
                "' class='img-preview' alt='Story Image' />"
            );
        };

        reader.readAsDataURL(input.files[0]);
    });

});
