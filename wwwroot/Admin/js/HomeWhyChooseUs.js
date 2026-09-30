var allHomeWhyChooseUsRecords = [];

var iconList = [
    {
        value: "fa-solid fa-hotel",
        text: "Hotel"
    },
    {
        value: "fa-solid fa-square-parking",
        text: "Free Parking"
    },
    {
        value: "fa-solid fa-person-swimming",
        text: "Swimming Pool"
    },
    {
        value: "fa-solid fa-award",
        text: "Award / Quality"
    },
    {
        value: "fa-solid fa-shield-halved",
        text: "Safety & Security"
    },
    {
        value: "fa-solid fa-clock",
        text: "24/7 Service"
    },
    {
        value: "fa-solid fa-heart",
        text: "Comfort / Care"
    },
    {
        value: "fa-solid fa-handshake",
        text: "Trust / Service"
    },
    {
        value: "fa-solid fa-medal",
        text: "Best Rated"
    },
    {
        value: "fa-solid fa-headset",
        text: "Customer Support"
    },
    {
        value: "fa-solid fa-tags",
        text: "Best Price"
    },
    {
        value: "fa-solid fa-star",
        text: "Top Quality"
    },
    {
        value: "fa-solid fa-location-dot",
        text: "Prime Location"
    },
    {
        value: "fa-solid fa-bed",
        text: "Comfortable Rooms"
    },
    {
        value: "fa-solid fa-crown",
        text: "Luxury"
    },
    {
        value: "fa-solid fa-gem",
        text: "Premium"
    },
    {
        value: "fa-solid fa-thumbs-up",
        text: "Customer Satisfaction"
    },
    {
        value: "fa-solid fa-user-shield",
        text: "Guest Safety"
    },
    {
        value: "fa-solid fa-bell-concierge",
        text: "Guest Service"
    },
    {
        value: "fa-solid fa-wifi",
        text: "Free Wi-Fi"
    },
    {
        value: "fa-solid fa-utensils",
        text: "Fine Dining"
    },
    {
        value: "fa-solid fa-spa",
        text: "Wellness & Spa"
    }
];

// Page load: setup and event handlers
$(document).ready(function () {

    loadIcons();
    loadData();

    $("#btnAdd").click(function () {

        clearForm();

        $("#btnSave").text("Save");

        $("#drawer").addClass("active");
    });

    $("#closeDrawer, #btnCancel").click(function () {

        $("#drawer").removeClass("active");

        clearForm();
    });

    $("#btnSave").click(function () {

        if (!validateWhyChooseUsForm()) {
            return;
        }

        var id = parseInt($("#Id").val()) || 0;

        var data = {
            "HomeWhyChooseUs.HomeWhyChooseUsId": id,
            "HomeWhyChooseUs.IconClass": $("#IconClass").val(),
            "HomeWhyChooseUs.Title": $("#Title").val().trim(),
            "HomeWhyChooseUs.Description": $("#Description").val().trim(),
            "HomeWhyChooseUs.IsActive": $("#IsActive").is(":checked")
        };

        var $btn = $("#btnSave");

        $btn.prop("disabled", true);

        if (id === 0) {
            $btn.text("Saving...");
        }
        else {
            $btn.text("Updating...");
        }

        // AJAX call to /Admin/HomeWhyChooseUs/Save
        $.ajax({
            url: "/Admin/HomeWhyChooseUs/Save",
            type: "POST",
            data: data,

            success: function (response) {

                if (response.success) {

                    Swal.fire({
                        icon: "success",
                        title: response.message,
                        timer: 1500,
                        showConfirmButton: false
                    }).then(function () {

                        clearForm();

                        $("#drawer").removeClass("active");

                        loadData();
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
                    title: "Something went wrong.",
                    text: "Unable to process your request.",
                    confirmButtonText: "OK"
                });
            },

            complete: function () {

                $btn.prop("disabled", false);

                if (parseInt($("#Id").val()) === 0) {
                    $btn.text("Save");
                }
                else {
                    $btn.text("Update");
                }
            }
        });
    });

    // Search box typing
    $("#txtSearch").on("keyup", function () {

        var value = $(this).val().toLowerCase();

        $("#tblData tbody tr").filter(function () {

            $(this).toggle(
                $(this)
                    .text()
                    .toLowerCase()
                    .indexOf(value) > -1
            );
        });
    });

    // Icon class field change
    $("#IconClass").on("change", function () {

        if ($(this).val()) {
            $("#err-IconClass").text("");
        }
    });

    // Title field typing
    $("#Title").on("input", function () {

        if ($(this).val().trim() !== "") {
            $("#err-Title").text("");
        }
    });
});

// Validate why choose us form
function validateWhyChooseUsForm() {

    var isValid = true;

    $(".field-error").text("");

    var iconClass = $("#IconClass").val();
    var title = $("#Title").val().trim();

    if (!iconClass) {

        $("#err-IconClass").text(
            "Icon is required."
        );

        isValid = false;
    }

    if (title === "") {

        $("#err-Title").text(
            "Title is required."
        );

        isValid = false;
    }

    return isValid;
}

// Load icons
function loadIcons() {

    var html = "<option value=''></option>";

    $.each(iconList, function (index, icon) {

        html +=
            "<option value='" +
            icon.value +
            "'>" +
            icon.text +
            "</option>";
    });

    $("#IconClass").html(html);

    $("#IconClass").select2({

        placeholder: "-- Select Icon --",

        allowClear: false,

        width: "100%",

        dropdownParent: $("#drawer"),

        templateResult: function (icon) {

            if (!icon.id) {

                return $(
                    "<span style='color:#888;'>-- Select Icon --</span>"
                );
            }

            return $(
                "<span class='icon-option'>" +
                "<i class='" + icon.id + "'></i>" +
                "<span>" + icon.text + "</span>" +
                "</span>"
            );
        },

        templateSelection: function (icon) {

            if (!icon.id) {

                return $(
                    "<span style='color:#888;'>-- Select Icon --</span>"
                );
            }

            return $(
                "<span class='icon-selected'>" +
                "<i class='" + icon.id + "'></i>" +
                "<span>" + icon.text + "</span>" +
                "</span>"
            );
        }
    });

    $("#IconClass")
        .next(".select2")
        .find(".select2-search__field")
        .attr(
            "placeholder",
            "Search Icon..."
        );
}

// Load data
function loadData() {

    // AJAX call to /Admin/HomeWhyChooseUs/GetAll
    $.ajax({

        url: "/Admin/HomeWhyChooseUs/GetAll",

        type: "GET",

        success: function (response) {

            if (response.success) {

                bindTable(response.data);
            }
        },

        error: function () {

            Swal.fire({
                icon: "error",
                title: "Unable to load records."
            });
        }
    });
}

// Bind table
function bindTable(data) {

    allHomeWhyChooseUsRecords = data || [];

    var html = "";

    $.each(
        allHomeWhyChooseUsRecords,
        function (index, item) {

            html += "<tr>";

            html +=
                "<td class='tableIcon'>" +
                "<i class='" +
                (item.iconClass || "") +
                "'></i>" +
                "</td>";

            html +=
                "<td>" +
                (item.title || "") +
                "</td>";

            html +=
                "<td>" +
                (item.description || "") +
                "</td>";

            html +=
                "<td>" +
                item.displayOrder +
                "</td>";

            html +=
                "<td>" +
                (
                    item.isActive
                        ? "<span>Active</span>"
                        : "<span>Inactive</span>"
                ) +
                "</td>";

            html += "<td>";

            html +=
                "<button type='button' class='action-btn action-details' " +
                "onclick='viewDetails(" +
                item.homeWhyChooseUsId +
                ")' title='Details'>" +
                "<i class='fa-solid fa-eye'></i>" +
                "</button> ";

            html +=
                "<button type='button' class='action-btn action-edit' " +
                "onclick='edit(" +
                item.homeWhyChooseUsId +
                ")' title='Edit'>" +
                "<i class='fa-solid fa-pen'></i>" +
                "</button> ";

            html +=
                "<button type='button' class='action-btn action-delete' " +
                "onclick='deleteRecord(" +
                item.homeWhyChooseUsId +
                ")' title='Delete'>" +
                "<i class='fa-solid fa-trash'></i>" +
                "</button>";

            html += "</td>";
        }
    );

    $("#tblData tbody").html(html);
}

// View details
function viewDetails(id) {

    var item = allHomeWhyChooseUsRecords.find(
        function (x) {
            return x.homeWhyChooseUsId === id;
        }
    );

    if (!item) {
        return;
    }

    showDetails(
        "Why Choose Us - Details",
        [
            {
                label: "Icon",
                value: item.iconClass
            },
            {
                label: "Title",
                value: item.title
            },
            {
                label: "Description",
                value: item.description
            },
            {
                label: "Display Order",
                value: item.displayOrder
            },
            {
                label: "Status",
                value: item.isActive
                    ? "Active"
                    : "Inactive"
            }
        ]
    );
}

// Edit
function edit(id) {

    // AJAX call to server
    $.ajax({

        url:
            "/Admin/HomeWhyChooseUs/GetById?id=" +
            encodeURIComponent(id),

        type: "GET",

        success: function (response) {

            if (!response.success) {

                Swal.fire({
                    icon: "error",
                    title: response.message,
                    confirmButtonText: "OK"
                });

                return;
            }

            var item = response.data;

            $("#Id").val(
                item.homeWhyChooseUsId
            );

            $("#Title").val(
                item.title || ""
            );

            $("#Description").val(
                item.description || ""
            );

            $("#IsActive").prop(
                "checked",
                item.isActive
            );

            $(".field-error").text("");

            $("#btnSave").text("Update");

            $("#drawer").addClass("active");

            setTimeout(function () {

                var iconValue = item.iconClass || "";

                $("#IconClass")
                    .val(iconValue)
                    .trigger("change");

            }, 100);
        },

        error: function () {

            Swal.fire({
                icon: "error",
                title: "Unable to load record.",
                confirmButtonText: "OK"
            });
        }
    });
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

        text:
            "You won't be able to recover this record.",

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

                        loadData();
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
                    text:
                        "Something went wrong while deleting the record.",
                    confirmButtonText: "OK"
                });
            }
        });
    });
}

// Clear form
function clearForm() {

    $("#Id").val(0);

    $("#IconClass")
        .val("")
        .trigger("change");

    $("#Title").val("");

    $("#Description").val("");

    $("#IsActive")
        .prop("checked", true);

    $(".field-error").text("");

    $("#btnSave").text("Save");
}
$("#btnAllRecords").click(function () {
    window.location.href =
        "/Admin/HomeWhyChooseUs/AllRecords";
});