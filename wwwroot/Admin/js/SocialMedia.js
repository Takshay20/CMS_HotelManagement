var allSocialMediaRecords = [];
$(document).ready(function () {
    loadData();
});
$("#btnAdd").click(function () {
    clearForm();
    $("#drawer").addClass("active");
});
$("#closeDrawer,#btnCancel").click(function () {
    $("#drawer").removeClass("active");
});

function loadData() {
    $.ajax({
        url: "/Admin/SocialMedia/GetAll",
        type: "GET",
        success: function (r) {
            if (r.success) bindTable(r.data);
        }
    });
}

function bindTable(data) {
    allSocialMediaRecords = data;
    var html = "";
    $.each(data, function (i, item) {
        html += "<tr>";
        html += "<td><i class='" + (item.iconClass || "") + "'></i></td>";
        html += "<td>" + item.platformName + "</td>";
        html += "<td>" + item.url + "</td>";
        html += "<td>" + item.displayOrder + "</td>";
        html += "<td>" + (item.isActive ? "<span>Active</span>" : "<span>Inactive</span>") + "</td>";
        html += "<td><button onclick='viewDetails(" + item.socialMediaId + ")'>Details</button> <button onclick='edit(" + item.socialMediaId + ")'>Edit</button> <button onclick='deleteRecord(" + item.socialMediaId + ")'>Delete</button></td>";
        html += "</tr>";
    });
    $("#tblData tbody").html(html);
}

function viewDetails(id) {
    var item = allSocialMediaRecords.find(function (x) { return x.socialMediaId === id; });
    if (!item) return;
    showDetails("Social Media Details", [
        { label: "Platform", value: item.platformName },
        { label: "Icon Class", value: item.iconClass },
        { label: "Url", value: item.url },
        { label: "Display Order", value: item.displayOrder },
        { label: "Status", value: item.isActive ? "Active" : "Inactive" }
    ]);
}

function edit(id) {
    $.ajax({
        url: "/Admin/SocialMedia/GetById?id=" + id,
        type: "GET",
        success: function (r) {
            if (r.success) {
                var item = r.data;
                $("#Id").val(item.socialMediaId);
                $("#PlatformName").val(item.platformName);
                $("#IconClass").val(item.iconClass);
                $("#Url").val(item.url);
                $("#IsActive").prop("checked", item.isActive);
                $("#drawer").addClass("active");
            }
        }
    });
}

$("#btnSave").click(function () {
    var ok = validateForm([
        { id: "PlatformName", label: "Platform Name", required: true },
        { id: "IconClass", label: "Icon Class", required: true },
        { id: "Url", label: "Url", required: true }
    ]);
    if (!ok) return;
    var data = {
        "SocialMedia.SocialMediaId": $("#Id").val(),
        "SocialMedia.PlatformName": $("#PlatformName").val(),
        "SocialMedia.IconClass": $("#IconClass").val(),
        "SocialMedia.Url": $("#Url").val(),
        "SocialMedia.IsActive": $("#IsActive").is(":checked")
    };
    $.ajax({
        url: "/Admin/SocialMedia/Save",
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
                clearForm();
                $("#drawer").removeClass("active");
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

function deleteRecord(id) {
    Swal.fire({
        title: "Delete Record?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: "/Admin/SocialMedia/Delete",
                type: "POST",
                data: { id: id },
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
                }
            });
        }
    });
}

$("#txtSearch").keyup(function () {
    var value = $(this).val().toLowerCase();
    $("#tblData tbody tr").filter(function () {
        $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
    });
});

function clearForm() {
    $("#Id").val(0);
    $("#PlatformName").val("");
    $("#IconClass").val("");
    $("#Url").val("");
    $("#IsActive").prop("checked", true);
    $("#frmData .invalid").removeClass("invalid");
    $("#frmData .field-error").removeClass("show").text("");
}

// Restore Edit
$(function () {
    var editId = new URLSearchParams(window.location.search).get("editId");
    if (editId) {
        setTimeout(function () {
            edit(editId);
        }, 300);
    }
});
