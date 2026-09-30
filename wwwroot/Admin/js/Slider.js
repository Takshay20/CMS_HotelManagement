var allSliderRecords = [];

$(document).ready(function () {

    $("#ddlPageFilter").select2({
        placeholder: "All Pages",
        allowClear: false,
        width: "180px"
    });

    $("#PageKey").select2({
        placeholder: "-- Select Page --",
        allowClear: false,
        width: "100%",
        dropdownParent: $("#drawer")
    });

    loadData();

    $("#btnAdd").on("click", function () {
        clearForm();
        $("#drawer").addClass("active");
    });

    $("#closeDrawer, #btnCancel").on("click", function () {
        clearForm();
        $("#drawer").removeClass("active");
    });

    $("#ddlPageFilter").on("change", function () {
        loadData();
    });

    $("#ImageFile").on("change", function () {

        var file = this.files && this.files[0];

        if (!file) {
            return;
        }

        var allowedExtensions = [
            ".jpg",
            ".jpeg",
            ".png",
            ".gif",
            ".jfif"
        ];

        var extension =
            "." +
            file.name
                .split(".")
                .pop()
                .toLowerCase();

        if (!allowedExtensions.includes(extension)) {

            $(this).val("");

            $("#imgPreview")
                .attr("src", "")
                .hide();

            $("#err-ImageFile")
                .text("Only JPG, JPEG, PNG, GIF and JFIF images are allowed.")
                .addClass("show");

            return;
        }

        $("#err-ImageFile")
            .text("")
            .removeClass("show");

        var reader = new FileReader();

        reader.onload = function (e) {

            $("#imgPreview")
                .attr("src", e.target.result)
                .show();
        };

        reader.readAsDataURL(file);
    });

    $("#PageKey").on("change", function () {

        var value = $(this).val();

        if (value) {

            $("#err-PageKey")
                .text("")
                .removeClass("show");

            $("#PageKey")
                .next(".select2")
                .find(".select2-selection")
                .removeClass("invalid");
        }
    });

    $("#Title").on("input", function () {

        if ($(this).val().trim() !== "") {

            $("#err-Title")
                .text("")
                .removeClass("show");

            $(this).removeClass("invalid");
        }
    });

    $("#SubTitle").on("input", function () {

        if ($(this).val().trim() !== "") {

            $("#err-SubTitle")
                .text("")
                .removeClass("show");

            $(this).removeClass("invalid");
        }
    });

    $("#ButtonText").on("input", function () {

        if ($(this).val().trim() !== "") {

            $("#err-ButtonText")
                .text("")
                .removeClass("show");

            $(this).removeClass("invalid");
        }
    });

    $("#ButtonUrl").on("input", function () {

        if ($(this).val().trim() !== "") {

            $("#err-ButtonUrl")
                .text("")
                .removeClass("show");

            $(this).removeClass("invalid");
        }
    });

    $("#btnSave").on("click", function () {

        saveSlider();
    });

    $("#txtSearch").on("keyup", function () {

        var value =
            $(this)
                .val()
                .toLowerCase()
                .trim();

        $("#tblData tbody tr").each(function () {

            var text =
                $(this)
                    .text()
                    .toLowerCase();

            $(this).toggle(
                text.indexOf(value) > -1
            );
        });
    });
});


function loadData() {

    var pageKey =
        $("#ddlPageFilter").val() || "";

    $.ajax({
        url: "/Admin/Slider/GetAll",
        type: "GET",
        data: {
            pageKey: pageKey
        },

        success: function (r) {

            if (r.success) {

                bindTable(
                    r.data || []
                );

            } else {

                bindTable([]);
            }
        },

        error: function () {

            Swal.fire({
                icon: "error",
                title: "Unable to load slider records."
            });
        }
    });
}


function bindTable(data) {

    allSliderRecords = data || [];

    var html = "";

    if (allSliderRecords.length === 0) {

        html = `
            <tr>
                <td colspan="7" style="text-align:center;">
                    No slider records found.
                </td>
            </tr>
        `;

        $("#tblData tbody").html(html);

        return;
    }

    $.each(
        allSliderRecords,
        function (i, item) {

            html += "<tr>";

            html += "<td>" +
                (i + 1) +
                "</td>";

            html += "<td>";

            if (item.imagePath) {

                html +=
                    "<img src='/" +
                    item.imagePath +
                    "' width='70' height='45' " +
                    "style='object-fit:cover;border-radius:6px;' />";
            }
            else {

                html +=
                    "<span>No Image</span>";
            }

            html += "</td>";

            html += "<td>" +
                (item.pageKey || "") +
                "</td>";

            html += "<td>" +
                (item.title || "") +
                "</td>";

            html += "<td>" +
                (item.displayOrder || 0) +
                "</td>";

            html += "<td>" +
                (
                    item.isActive
                        ? "<span>Active</span>"
                        : "<span>Inactive</span>"
                ) +
                "</td>";

            html += "<td>";

            html +=
                "<button type='button' " +
                "onclick='viewDetails(" +
                item.sliderId +
                ")'>Details</button> ";

            html +=
                "<button type='button' " +
                "onclick='edit(" +
                item.sliderId +
                ")'>Edit</button> ";

            html +=
                "<button type='button' " +
                "onclick='deleteRecord(" +
                item.sliderId +
                ")'>Delete</button>";

            html += "</td>";

            html += "</tr>";
        }
    );

    $("#tblData tbody").html(html);
}


function viewDetails(id) {

    var item =
        allSliderRecords.find(function (x) {
            return x.sliderId === id;
        });

    if (!item) {
        return;
    }

    showDetails(
        "Slider Details",
        [
            {
                label: "Page",
                value: item.pageKey
            },
            {
                label: "Title",
                value: item.title
            },
            {
                label: "Sub Title",
                value: item.subTitle
            },
            {
                label: "Button Text",
                value: item.buttonText
            },
            {
                label: "Button Url",
                value: item.buttonUrl
            },
            {
                label: "Display Order",
                value: item.displayOrder
            },
            {
                label: "Status",
                value:
                    item.isActive
                        ? "Active"
                        : "Inactive"
            },
            {
                label: "Image",
                value: item.imagePath,
                isImage: true
            }
        ]
    );
}


function edit(id) {

    $.ajax({

        url:
            "/Admin/Slider/GetById?id=" +
            encodeURIComponent(id),

        type: "GET",

        success: function (r) {

            if (!r.success) {

                Swal.fire({
                    icon: "error",
                    title: r.message
                });

                return;
            }

            var item = r.data;

            $("#Id")
                .val(item.sliderId);

            $("#PageKey")
                .val(item.pageKey || "")
                .trigger("change");

            $("#Title")
                .val(item.title || "");

            $("#SubTitle")
                .val(item.subTitle || "");

            $("#ButtonText")
                .val(item.buttonText || "");

            $("#ButtonUrl")
                .val(item.buttonUrl || "");

            $("#DisplayOrder")
                .val(item.displayOrder || 1);

            $("#IsActive")
                .prop(
                    "checked",
                    item.isActive === true
                );

            $("#ImageFile")
                .val("");

            if (item.imagePath) {

                $("#imgPreview")
                    .attr(
                        "src",
                        "/" + item.imagePath
                    )
                    .show();

            } else {

                $("#imgPreview")
                    .attr("src", "")
                    .hide();
            }

            clearValidation();

            $("#btnSave")
                .text("Update");

            $("#btnCancel")
                .show();

            $("#drawer")
                .addClass("active");
        },

        error: function () {

            Swal.fire({
                icon: "error",
                title: "Unable to load slider details."
            });
        }
    });
}


function saveSlider() {

    var id =
        parseInt(
            $("#Id").val()
        ) || 0;

    var isNew =
        id === 0;

    var ok = validateSliderForm(
        isNew
    );

    if (!ok) {
        return;
    }

    var formData =
        new FormData();

    formData.append(
        "Slider.SliderId",
        id
    );

    formData.append(
        "Slider.PageKey",
        $("#PageKey").val() || ""
    );

    formData.append(
        "Slider.Title",
        $("#Title").val().trim()
    );

    formData.append(
        "Slider.SubTitle",
        $("#SubTitle").val().trim()
    );

    formData.append(
        "Slider.ButtonText",
        $("#ButtonText").val().trim()
    );

    formData.append(
        "Slider.ButtonUrl",
        $("#ButtonUrl").val().trim()
    );

    formData.append(
        "Slider.DisplayOrder",
        $("#DisplayOrder").val() || 1
    );

    formData.append(
        "Slider.IsActive",
        $("#IsActive").is(":checked")
    );

    var image =
        $("#ImageFile")[0].files[0];

    if (image) {

        formData.append(
            "ImageFile",
            image
        );
    }

    var button =
        $("#btnSave");

    button
        .prop("disabled", true)
        .text(
            isNew
                ? "Saving..."
                : "Updating..."
        );

    $.ajax({

        url: "/Admin/Slider/Save",

        type: "POST",

        data: formData,

        processData: false,

        contentType: false,

        success: function (r) {

            if (r.success) {

                Swal.fire({
                    icon: "success",
                    title: r.message,
                    timer: 1500,
                    showConfirmButton: false
                }).then(function () {

                    clearForm();

                    $("#drawer")
                        .removeClass("active");

                    loadData();
                });

            } else {

                Swal.fire({
                    icon: "error",
                    title: r.message
                });
            }
        },

        error: function (xhr) {

            var message =
                "Something went wrong.";

            if (
                xhr.responseJSON &&
                xhr.responseJSON.message
            ) {
                message =
                    xhr.responseJSON.message;
            }

            Swal.fire({
                icon: "error",
                title: message
            });
        },

        complete: function () {

            button
                .prop("disabled", false);

            if (
                parseInt(
                    $("#Id").val()
                ) > 0
            ) {
                button.text("Update");
            } else {
                button.text("Save");
            }
        }
    });
}


function validateSliderForm(isNew) {

    var valid = true;

    clearValidation();

    var pageKey =
        $("#PageKey").val();

    if (!pageKey) {

        showFieldError(
            "PageKey",
            "Page is required."
        );

        valid = false;
    }

    var title =
        $("#Title").val().trim();

    if (!title) {

        showFieldError(
            "Title",
            "Title is required."
        );

        valid = false;
    }

    var image =
        $("#ImageFile")[0].files[0];

    if (isNew && !image) {

        showFieldError(
            "ImageFile",
            "Slider Image is required."
        );

        valid = false;
    }

    if (image) {

        var allowedExtensions = [
            ".jpg",
            ".jpeg",
            ".png",
            ".gif",
            ".jfif"
        ];

        var extension =
            "." +
            image.name
                .split(".")
                .pop()
                .toLowerCase();

        if (
            !allowedExtensions.includes(
                extension
            )
        ) {

            showFieldError(
                "ImageFile",
                "Only JPG, JPEG, PNG, GIF and JFIF images are allowed."
            );

            valid = false;
        }
    }

    return valid;
}


function showFieldError(
    id,
    message
) {

    var field =
        $("#" + id);

    field.addClass("invalid");

    $("#err-" + id)
        .text(message)
        .addClass("show");

    if (
        id === "PageKey"
    ) {

        field
            .next(".select2")
            .find(".select2-selection")
            .addClass("invalid");
    }
}


function clearValidation() {

    $("#frmData .invalid")
        .removeClass("invalid");

    $("#frmData .field-error")
        .removeClass("show")
        .text("");

    $("#PageKey")
        .next(".select2")
        .find(".select2-selection")
        .removeClass("invalid");
}


function deleteRecord(id) {

    Swal.fire({

        title: "Delete Record?",

        text:
            "You won't be able to recover this record.",

        icon: "warning",

        showCancelButton: true,

        confirmButtonText: "Yes",

        cancelButtonText: "Cancel"

    }).then(function (result) {

        if (!result.isConfirmed) {
            return;
        }

        $.ajax({

            url: "/Admin/Slider/Delete",

            type: "POST",

            data: {
                id: id
            },

            success: function (r) {

                if (r.success) {

                    Swal.fire({
                        icon: "success",
                        title: r.message,
                        timer: 1500,
                        showConfirmButton: false
                    }).then(function () {

                        clearForm();

                        $("#drawer")
                            .removeClass("active");

                        loadData();
                    });

                } else {

                    Swal.fire({
                        icon: "error",
                        title: r.message
                    });
                }
            },

            error: function () {

                Swal.fire({
                    icon: "error",
                    title: "Unable to delete record."
                });
            }
        });
    });
}


function clearForm() {

    $("#Id")
        .val(0);

    $("#PageKey")
        .val("")
        .trigger("change");

    $("#Title")
        .val("");

    $("#SubTitle")
        .val("");

    $("#ButtonText")
        .val("");

    $("#ButtonUrl")
        .val("");

    $("#DisplayOrder")
        .val(1);

    $("#IsActive")
        .prop("checked", true);

    $("#ImageFile")
        .val("");

    $("#imgPreview")
        .attr("src", "")
        .hide();

    clearValidation();

    $("#btnSave")
        .prop("disabled", false)
        .text("Save");

    $("#btnCancel")
        .hide();
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
