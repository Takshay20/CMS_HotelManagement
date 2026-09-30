// Page load: setup and event handlers
$(document).ready(function () {
    loadData();
});

$("#LogoFile").change(function () {
    if (this.files && this.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $("#imgPreview").attr("src", e.target.result).show();
        };
        reader.readAsDataURL(this.files[0]);
    }
});

// Load data
function loadData() {

    // AJAX call to /Admin/SiteSetting/Get
    $.ajax({
        url: "/Admin/SiteSetting/Get",
        type: "GET",
        success: function (r) {
            if (r.success && r.data) {
                var item = r.data;
                $("#Id").val(item.siteSettingId);
                $("#SiteName").val(item.siteName);
                $("#Tagline").val(item.tagline);
                $("#Phone").val(item.phone);
                $("#Email").val(item.email);
                $("#Address").val(item.address);
                if (item.logoPath) $("#imgPreview").attr("src", "/" + item.logoPath).show();
            }
        }
    });
}

$("#btnSave").click(function () {
    var ok = validateForm([
        { id: "SiteName", label: "Site Name", required: true },
        { id: "Phone", label: "Phone", required: true },
        { id: "Email", label: "Email", required: true, type: "email" }
    ]);
    if (!ok) return;

    // Prepare form data for upload
    var formData = new FormData();
    formData.append("SiteSetting.SiteSettingId", $("#Id").val() || 0);
    formData.append("SiteSetting.SiteName", $("#SiteName").val());
    formData.append("SiteSetting.Tagline", $("#Tagline").val());
    formData.append("SiteSetting.Phone", $("#Phone").val());
    formData.append("SiteSetting.Email", $("#Email").val());
    formData.append("SiteSetting.Address", $("#Address").val());
    var logo = $("#LogoFile")[0].files[0];
    if (logo) formData.append("LogoFile", logo);

    // AJAX call to /Admin/SiteSetting/Save
    $.ajax({
        url: "/Admin/SiteSetting/Save",
        type: "POST",
        data: formData,
        processData: false,
        contentType: false,
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
