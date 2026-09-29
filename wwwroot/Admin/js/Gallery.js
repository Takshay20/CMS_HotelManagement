var allGalleryRecords = [];
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
$("#ImageFile").change(function () {
    if (this.files && this.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $("#imgPreview").attr("src", e.target.result).show();
        };
        reader.readAsDataURL(this.files[0]);
    }
});

function loadData() {
    $.ajax({
        url: "/Admin/Gallery/GetAll",
        type: "GET",
        success: function (r) {
            if (r.success)
                bindTable(r.data);
        }
    });
}

function bindTable(data) {
    allGalleryRecords = data;
    var html = "";
    $.each(data, function (i, item) {
        html += "<tr>";
        html += "<td><img src='/" + item.imagePath + "' width='70'/></td>";
        html += "<td>" + (item.title || "") + "</td>";
        html += "<td>" + (item.category || "") + "</td>";
        html += "<td>" + item.displayOrder + "</td>";
        html += "<td>" + (item.isActive ? "<span>Active</span>" : "<span>Inactive</span>") + "</td>";
        html += "<td><button onclick='viewDetails(" + item.galleryId + ")'>Details</button> <button onclick='edit(" + item.galleryId + ")'>Edit</button> <button onclick='deleteRecord(" + item.galleryId + ")'>Delete</button></td>";
        html += "</tr>";
    });
    $("#tblData tbody").html(html);
}

function viewDetails(id) {
    var item = allGalleryRecords.find(function (x) {
        return x.galleryId === id;
    });
    if (!item) return;
    showDetails("Gallery Image Details", [
        { label: "Title", value: item.title },
        { label: "Category", value: item.category },
        { label: "Display Order", value: item.displayOrder },
        { label: "Status", value: item.isActive ? "Active" : "Inactive" },
        { label: "Image", value: item.imagePath, isImage: true }
    ]);
}

function edit(id) {
    $.ajax({
        url: "/Admin/Gallery/GetById?id=" + id,
        type: "GET",
        success: function (r) {
            if (r.success) {
                var item = r.data;
                $("#Id").val(item.galleryId);
                $("#Title").val(item.title);
                $("#Category").val(item.category);
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
        { id: "Category", label: "Category", required: true, type: "select" },
        { id: "ImageFile", label: "Image", required: true, type: "file", onlyForNew: true, isNew: isNew }
    ]);
    if (!ok) return;

    var image = $("#ImageFile")[0].files[0];
    if (image) {
        var allowedTypes = [
            "image/jpeg",
            "image/png",
            "image/webp",
            "image/jpg"
        ];
        if (allowedTypes.indexOf(image.type) === -1) {
            Swal.fire({
                icon: "warning",
                title: "Only JPG, PNG or WEBP images are allowed"
            });
            return;
        }
        if (image.size > 2 * 1024 * 1024) {
            Swal.fire({
                icon: "warning",
                title: "Image size must be under 2 MB"
            });
            return;
        }
    }

    var formData = new FormData();
    formData.append("Gallery.GalleryId", $("#Id").val());
    formData.append("Gallery.Title", $("#Title").val());
    formData.append("Gallery.Category", $("#Category").val());
    formData.append("Gallery.IsActive", $("#IsActive").is(":checked"));
    var image = $("#ImageFile")[0].files[0];
    if (image) formData.append("ImageFile", image);

    $.ajax({
        url: "/Admin/Gallery/Save",
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

function deleteRecord(id) {

    Swal.fire({
        title: "Are you sure?",
        text: "You want to delete this record?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes, Delete",
        cancelButtonText: "Cancel"
    }).then(function (result) {

        if (!result.isConfirmed) {
            return;
        }

        $.ajax({
            url: "/Admin/Gallery/Delete",
            type: "POST",
            data: {
                id: id
            },
            success: function (response) {

                if (response.success) {

                    Swal.fire({
                        icon: "success",
                        title: "Deleted Successfully",
                        text: response.message,
                        timer: 1500,
                        showConfirmButton: false
                    }).then(function () {
                        loadData();
                    });

                } else {

                    Swal.fire({
                        icon: "error",
                        title: "Unable To Delete",
                        text: response.message
                    });
                }
            },
            error: function (xhr) {

                Swal.fire({
                    icon: "error",
                    title: "Something went wrong.",
                    text: "Unable to delete the record."
                });
            }
        });
    });
}

$("#txtSearch").keyup(function () {
    var value = $(this).val().toLowerCase();
    $("#tblData tbody tr").filter(function () {
        $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
    });
});

function clearForm() {
    $("#Id").val(0); $("#Title").val("");
    $("#Category").val("");
    $("#IsActive").prop("checked", true);
    $("#ImageFile").val("");
    $("#imgPreview").hide();
}
