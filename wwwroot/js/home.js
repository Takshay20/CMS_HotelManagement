$(document).ready(function () {

    // Feedback form form submit
    $("#feedbackForm").on("submit", function (e) {
        e.preventDefault();

        var $form = $(this);
        var isValid = true;

        $(".validation-error").remove();

        var $name = $form.find("[name='Name']");
        var $rating = $form.find("[name='Rating']");
        var $message = $form.find("[name='Message']");
        var $image = $form.find("[name='ImageFile']");

        var name = $.trim($name.val());
        var rating = $rating.val();
        var message = $.trim($message.val());
        var image = $image[0].files[0];

        if (name === "") {
            $name.parent().append(
                '<span class="validation-error" style="color:red;display:block;text-align:left;font-size:13px;margin-top:4px;">Name is required.</span>'
            );
            isValid = false;
        }

        if (rating === "") {
            $rating.after(
                '<span class="validation-error" style="color:red;display:block;text-align:left;font-size:13px;margin-top:4px;">Please select a rating.</span>'
            );
            isValid = false;
        }

        if (message === "") {
            $message.after(
                '<span class="validation-error" style="color:red;display:block;text-align:left;font-size:13px;margin-top:4px;">Message is required.</span>'
            );
            isValid = false;
        }

        if (image) {
            var allowedTypes = [
                "image/jpeg",
                "image/png",
                "image/webp"
            ];

            if ($.inArray(image.type, allowedTypes) === -1) {
                $image.after(
                    '<span class="validation-error" style="color:red;display:block;text-align:left;font-size:13px;margin-top:4px;">Only JPG, PNG and WEBP images are allowed.</span>'
                );
                isValid = false;
            }
            else if (image.size > 5 * 1024 * 1024) {
                $image.after(
                    '<span class="validation-error" style="color:red;display:block;text-align:left;font-size:13px;margin-top:4px;">Image size must be less than 5 MB.</span>'
                );
                isValid = false;
            }
        }

        if (!isValid) {
            return;
        }

        // Prepare form data for upload
        var formData = new FormData(this);
        var $btn = $form.find("button[type='submit']");

        $btn.prop("disabled", true).text("Submitting...");

        // AJAX call to server
        $.ajax({
            url: '@Url.Action("SubmitFeedback", "Home")',
            type: "POST",
            data: formData,
            contentType: false, 
            processData: false,

            success: function (res) {

                var alertClass = res.success
                    ? "alert alert-success"
                    : "alert alert-danger";

                $("#feedbackAlert").html(
                    '<div class="' + alertClass + '">' + res.message + '</div>'
                );

                if (res.success) {
                    $form[0].reset();
                    $(".validation-error").remove();
                }
            },

            error: function () {
                $("#feedbackAlert").html(
                    '<div class="alert alert-danger">Something went wrong. Please try again.</div>'
                );
            },

            complete: function () {
                $btn.prop("disabled", false).text("Submit Feedback");
            }
        });
    });

});