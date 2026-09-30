var allFacilityRecords = [];

// Page load: setup and event handlers
$(document).ready(function () { loadData(); });
$("#btnAdd").click(function () { clearForm(); $("#drawer").addClass("active"); });
$("#closeDrawer,#btnCancel").click(function () { $("#drawer").removeClass("active"); });
$("#ImageFile").change(function () {
    if (this.files && this.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) { $("#imgPreview").attr("src", e.target.result).show(); };
        reader.readAsDataURL(this.files[0]);
    }
});

// Load data
function loadData() {

    // AJAX call to /Admin/Facility/GetAll
    $.ajax({
        url: "/Admin/Facility/GetAll",
        type: "GET",
        success: function (r)
        {
            if (r.success) bindTable(r.data);
        }
    });
}

// Bind table
function bindTable(data) {
    allFacilityRecords = data;
    var html = "";
    $.each(data, function (i, item) {
        html += "<tr>";
        html += "<td><img src='/" + item.imagePath + "' width='70'/></td>";
        html += "<td>" + item.title + "</td>";
        html += "<td>" + (item.description || "") + "</td>";
        html += "<td>" + item.displayOrder + "</td>";
        html += "<td>" + (item.isActive ? "<span>Active</span>" : "<span>Inactive</span>") + "</td>";
        html += "<td><button onclick='viewDetails(" + item.facilityId + ")'>Details</button> <button onclick='edit(" + item.facilityId + ")'>Edit</button> <button onclick='deleteRecord(" + item.facilityId + ")'>Delete</button></td>";
        html += "</tr>";
    });
    $("#tblData tbody").html(html);
}

// View details
function viewDetails(id) {
    var item = allFacilityRecords.find(function (x) {
        return x.facilityId === id;
    });
    if (!item) return;
    showDetails("Facility Details", [
        { label: "Title", value: item.title },
        { label: "Description", value: item.description },
        { label: "Display Order", value: item.displayOrder },
        { label: "Status", value: item.isActive ? "Active" : "Inactive" },
        { label: "Image", value: item.imagePath, isImage: true }
    ]);
}

// Edit
function edit(id) {

    // AJAX call to server
    $.ajax({
        url: "/Admin/Facility/GetById?id=" + id,
        type: "GET",
        success: function (r) {
            if (r.success) {
                var item = r.data;
                $("#Id").val(item.facilityId);
                $("#Title").val(item.title);
                $("#Description").val(item.description);
                $("#IsActive").prop("checked", item.isActive);
                if (item.imagePath) $("#imgPreview").attr("src", "/" + item.imagePath).show();
                $("#drawer").addClass("active");
            }
        }
    });
}

$("#btnSave").click(function () {
    var isNew = $("#Id").val() == 0;
    var ok = validateForm([
        { id: "Title", label: "Title", required: true },
        { id: "ImageFile", label: "Image", required: true, type: "file", onlyForNew: true, isNew: isNew }
    ]);
    if (!ok) return;

    // Prepare form data for upload
    var formData = new FormData();
    formData.append("Facility.FacilityId", $("#Id").val());
    formData.append("Facility.Title", $("#Title").val());
    formData.append("Facility.Description", $("#Description").val());
    formData.append("Facility.IsActive", $("#IsActive").is(":checked"));
    var image = $("#ImageFile")[0].files[0];
    if (image) formData.append("ImageFile", image);

    // AJAX call to /Admin/Facility/Save
    $.ajax({
        url: "/Admin/Facility/Save",
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

// Delete record
function deleteRecord(id) {
    Swal.fire({
        title: "Delete Record?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes"
    }).then((result) => {
        if (result.isConfirmed) {

            // AJAX call to /Admin/Facility/Delete
            $.ajax({
                url: "/Admin/Facility/Delete",
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

// Clear form
function clearForm() {
    $("#Id").val(0);
    $("#Title").val("");
    $("#Description").val("");
    $("#IsActive").prop("checked", true);
    $("#ImageFile").val(""); $("#imgPreview").hide();
}

// Page load: setup and event handlers
$(function () {
    var editId = new URLSearchParams(window.location.search).get("editId");
    if (editId) {
        setTimeout(function () {
            edit(editId);
        }, 300);
    }
});
