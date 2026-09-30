$(document).ready(function () {

    loadRecords("All");

    $("#ddlFilter").change(function () {

        var filter = $(this).val();

        loadRecords(filter);
    });

});

// Load records
function loadRecords(filter) {

    // AJAX call to /Admin/HomeWhyChooseUs/GetAllRecords
    $.ajax({

        url: "/Admin/HomeWhyChooseUs/GetAllRecords",

        type: "GET",

        data: {
            filter: filter
        },

        success: function (response) {

            if (response.success) {

                bindRecords(response.data);

            }
            else {

                Swal.fire({
                    icon: "error",
                    title: response.message,
                    confirmButtonText: "OK"
                });

            }

        },

        error: function () {

            Swal.fire({
                icon: "error",
                title: "Unable to load records.",
                text: "Unable to load records.",
                confirmButtonText: "OK"
            });

        }

    });
}

// Bind records
function bindRecords(data) {

    var html = "";

    if (!data || data.length === 0) {

        html =
            "<tr>" +
            "<td colspan='7' style='text-align:center;'>No records found.</td>" +
            "</tr>";

        $("#tblAllRecords tbody").html(html);

        return;
    }

    $.each(data, function (index, item) {

        var isDeleted =
            item.isDeleted === true ||
            item.IsDeleted === true ||
            item.isDeleted === 1 ||
            item.IsDeleted === 1 ||
            item.isDeleted === "1" ||
            item.IsDeleted === "1";

        var isActive =
            item.isActive === true ||
            item.IsActive === true ||
            item.isActive === 1 ||
            item.IsActive === 1 ||
            item.isActive === "1" ||
            item.IsActive === "1";

        var status = "";

        if (item.isDeleted) {

            status =
                "<span style='display:inline-flex;align-items:center;justify-content:center;" +
                "min-width:70px;padding:6px 12px;border-radius:20px;" +
                "font-size:12px;font-weight:600;" +
                "background:#fde7e9 !important;color:#dc3545 !important;'>" +
                "Deleted</span>";

        }
        else if (item.isActive) {

            status =
                "<span style='display:inline-flex;align-items:center;justify-content:center;" +
                "min-width:70px;padding:6px 12px;border-radius:20px;" +
                "font-size:12px;font-weight:600;" +
                "background:#eaf7ed !important;color:#2e7d32 !important;'>" +
                "Active</span>";

        }
        else {

            status =
                "<span style='display:inline-flex;align-items:center;justify-content:center;" +
                "min-width:70px;padding:6px 12px;border-radius:20px;" +
                "font-size:12px;font-weight:600;" +
                "background:#fff4d6 !important;color:#a87900 !important;'>" +
                "Inactive</span>";

        }

        html += "<tr>";

        html +=
            "<td class='tableIcon'>" +
            "<i class='" +
            (item.iconClass || item.IconClass || "") +
            "'></i>" +
            "</td>";

        html +=
            "<td>" +
            (item.title || item.Title || "") +
            "</td>";

        html +=
            "<td>" +
            (item.description || item.Description || "") +
            "</td>";

        html +=
            "<td>" +
            (item.displayOrder || item.DisplayOrder || 0) +
            "</td>";

        html +=
            "<td>" +
            status +
            "</td>";

        html +=
            "<td>" +
            (isDeleted ? "Yes" : "No") +
            "</td>";

        html += "<td>";

        if (isDeleted) {

            html +=
                "<button type='button' " +
                "class='action-btn action-restore' " +
                "title='Restore' " +
                "onclick='restoreRecord(" +
                (item.homeWhyChooseUsId || item.HomeWhyChooseUsId) +
                ")'>" +
                "<i class='fa-solid fa-rotate-left'></i>" +
                "</button>";

        }
        else if (!isActive) {

            html +=
                "<button type='button' " +
                "class='action-btn action-edit' " +
                "title='Edit' " +
                "onclick='editRecord(" +
                (item.homeWhyChooseUsId || item.HomeWhyChooseUsId) +
                ")'>" +
                "<i class='fa-solid fa-pen'></i>" +
                "</button> ";

            html +=
                "<button type='button' " +
                "class='action-btn action-delete' " +
                "title='Delete' " +
                "onclick='deleteRecord(" +
                (item.homeWhyChooseUsId || item.HomeWhyChooseUsId) +
                ")'>" +
                "<i class='fa-solid fa-trash'></i>" +
                "</button>";

        }
        else {

            html +=
                "<button type='button' " +
                "class='action-btn action-disabled' " +
                "title='Action disabled' " +
                "disabled>" +
                "<i class='fa-solid fa-lock'></i>" +
                "</button>";

        }

        html += "</td>";

        

    });

    $("#tblAllRecords tbody").html(html);
}
$("#btnBack").click(function () {
    window.location.href =
        "/Admin/HomeWhyChooseUs/HomeWhyChooseUs";
});

// Restore record
function restoreRecord(id) {

    Swal.fire({
        title: "Restore Record?",
        text: "This record will be restored as inactive.",
        icon: "question",
        showCancelButton: true,
        confirmButtonText: "Yes, Restore",
        cancelButtonText: "Cancel"
    }).then(function (result) {

        if (!result.isConfirmed) {
            return;
        }

        // AJAX call to /Admin/HomeWhyChooseUs/Restore
        $.ajax({

            url: "/Admin/HomeWhyChooseUs/Restore",

            type: "POST",

            cache: false,

            data: {
                id: id
            },

            success: function (response) {

                if (response.success) {

                    Swal.fire({
                        icon: "success",
                        title: "Restored",
                        text: response.message,
                        confirmButtonText: "OK"
                    }).then(function () {

                        var filter = $("#ddlFilter").val();

                        loadRecords(filter);

                    });

                }
                else {

                    Swal.fire({
                        icon: "error",
                        title: "Restore Failed",
                        text: response.message,
                        confirmButtonText: "OK"
                    });

                }

            },

            error: function () {
                Swal.fire({
                    icon: "error",
                    title: "Error",
                    text: "You are able to restore After Some Time",
                    confirmButtonText: "OK"
                });
            }
        });
    });
}

// Edit record
function editRecord(id) {
    window.location.href =
        "/Admin/HomeWhyChooseUs/HomeWhyChooseUs?editId=" + id;
}

// Delete record
function deleteRecord(id) {
    if (!id || id <= 0) {
        Swal.fire({
            icon: "error",
            title: "Invalid Record.",
            confirmButtonText: "OK"
        });
        return;
    }
    Swal.fire({
        title: "Delete Record?",
        text: "You won't be able to recover this record.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes, Delete",
        cancelButtonText: "Cancel"
    }).then(function (result) {
        if (!result.isConfirmed) {
            return;
        }

        // AJAX call to /Admin/HomeWhyChooseUs/Delete
        $.ajax({
            url: "/Admin/HomeWhyChooseUs/Delete",
            type: "POST",
            cache: false,
            data: {
                id: id
            },
            success: function (response) {
                if (response.success) {
                    Swal.fire({
                        icon: "success",
                        title: response.message,
                        timer: 1500,
                        showConfirmButton: false
                    }).then(function () {
                        var filter = $("#ddlFilter").val();
                        loadRecords(filter);
                    });
                }
                else {
                    Swal.fire({
                        icon: "error",
                        title: response.message,
                        confirmButtonText: "OK"
                    });
                }
            },
            error: function () {
                Swal.fire({
                    icon: "error",
                    title: "Unable to delete record.",
                    text: "Something went wrong while deleting the record.",
                    confirmButtonText: "OK"
                });
            }
        });
    });
}