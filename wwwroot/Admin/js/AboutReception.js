// Page load: setup and event handlers
$(document).ready(function () {

    // About reception form form submit
    $("#aboutReceptionForm").on("submit", function (e) {
        e.preventDefault();

        var ok = validateForm([
            { id: "Title", label: "Title", required: true },
            { id: "Description", label: "Description", required: true }
        ]);
        if (!ok) return;

        var form = this;

        // Prepare form data for upload
        var formData = new FormData(form);
        var $btn = $(form).find("button[type='submit']");
        $btn.prop("disabled", true).text("Updating...");

        // AJAX call to /Admin/AboutReception/Update
        $.ajax({
            url: "/Admin/AboutReception/Update",
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
                        title: response.message
                    });
                }
            },
            error: function () {
                Swal.fire({
                    icon: "error",
                    title: "Something went wrong."
                });
            },
            complete: function () {
                $btn.prop("disabled", false).text("Update Changes");
            }
        });
    });
});
