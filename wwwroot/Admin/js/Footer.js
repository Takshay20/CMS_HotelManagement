// Page load: setup and event handlers
$(document).ready(function () {
    loadData();
});

// Load data
function loadData() {

    // AJAX call to /Admin/Footer/Get
    $.ajax({
        url: "/Admin/Footer/Get",
        type: "GET",
        success: function (r) {
            if (r.success && r.data) {
                var item = r.data;
                $("#Id").val(item.footerId);
                $("#AboutText").val(item.aboutText);
                $("#Phone").val(item.phone);
                $("#Email").val(item.email);
                $("#Address").val(item.address);
                $("#CopyrightText").val(item.copyrightText);
            }
        }
    });
}

$("#btnSave").click(function () {
    var ok = validateForm([
        { id: "AboutText", label: "About Text", required: true },
        { id: "Phone", label: "Phone", required: true },
        { id: "Email", label: "Email", required: true, type: "email" }
    ]);
    if (!ok) return;
    var data = {
        "Footer.FooterId": $("#Id").val() || 0,
        "Footer.AboutText": $("#AboutText").val(),
        "Footer.Phone": $("#Phone").val(),
        "Footer.Email": $("#Email").val(),
        "Footer.Address": $("#Address").val(),
        "Footer.CopyrightText": $("#CopyrightText").val()
    };

    // AJAX call to /Admin/Footer/Save
    $.ajax({
        url: "/Admin/Footer/Save",
        type: "POST",
        data: data,
        success: function (r) {
            if (r.success) {
                Swal.fire({
                    icon: 'success',
                    title: r.message,
                    timer: 1500,
                    showConfirmButton: false
                });
                loadData();
            }
            else {
                Swal.fire({
                    icon: 'error',
                    title: r.message
                });
            }
        }
    });
});
