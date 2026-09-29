var allAboutCounterRecords = [];

var counterIcons = [
    { value: "fa-solid fa-bed", text: "Rooms (Bed)" },
    { value: "fa-solid fa-hotel", text: "Hotel" },
    { value: "fa-solid fa-users", text: "Guests / Customers" },
    { value: "fa-solid fa-user-tie", text: "Staff" },
    { value: "fa-solid fa-calendar-days", text: "Years of Experience" },
    { value: "fa-solid fa-star", text: "Rating / Awards" },
    { value: "fa-solid fa-trophy", text: "Awards Won" },
    { value: "fa-solid fa-city", text: "Branches / Locations" },
    { value: "fa-solid fa-face-smile", text: "Happy Clients" },
    { value: "fa-solid fa-thumbs-up", text: "Positive Reviews" },
    { value: "fa-solid fa-globe", text: "Countries Served" },
    { value: "fa-solid fa-utensils", text: "Restaurants" }
];

$(document).ready(function () {

    loadIconDropdown();

    $("#IconClass").select2({
        width: "100%",
        placeholder: "-- Select Icon --",
        allowClear: false,
        dropdownParent: $("#drawer"),
        templateResult: formatIcon,
        templateSelection: formatIconSelection,
        escapeMarkup: function (markup) {
            return markup;
        }
    });

    loadData();
});

function loadIconDropdown() {

    var html = "<option value=''>-- Select Icon --</option>";

    $.each(counterIcons, function (index, item) {

        html += "<option value='" +
            item.value +
            "'>" +
            item.text +
            "</option>";
    });

    $("#IconClass").html(html);
}

function formatIcon(option) {

    if (!option.id) {
        return option.text;
    }

    return "<span class='icon-option'>" +
        "<i class='" + option.id + "'></i>" +
        "<span>" + option.text + "</span>" +
        "</span>";
}

function formatIconSelection(option) {

    if (!option.id) {
        return option.text;
    }

    return "<span class='icon-selected'>" +
        "<i class='" + option.id + "'></i>" +
        "<span>" + option.text + "</span>" +
        "</span>";
}

$("#btnAdd").click(function () {

    clearForm();

    $("#drawer").addClass("active");
});

$("#closeDrawer,#btnCancel").click(function () {

    $("#drawer").removeClass("active");
});

function loadData() {

    $.ajax({
        url: "/Admin/AboutCounter/GetAll",
        type: "GET",
        success: function (response) {

            if (response.success) {
                bindTable(response.data);
            }
        }
    });
}

function bindTable(data) {

    allAboutCounterRecords = data;

    var html = "";

    $.each(data, function (index, item) {

        html += "<tr>";

        html += "<td class='tableIcon'>" +
            "<i class='" +
            (item.iconClass || "") +
            "'></i>" +
            "</td>";

        html += "<td>" +
            (item.number || "") +
            (item.suffix || "") +
            "</td>";

        html += "<td>" +
            (item.label || "") +
            "</td>";

        html += "<td>" +
            item.displayOrder +
            "</td>";

        html += "<td>" +
            (item.isActive
                ? "<span>Active</span>"
                : "<span>Inactive</span>") +
            "</td>";

        html += "<td>";

        html += "<button onclick='viewDetails(" +
            item.aboutCounterId +
            ")'>Details</button> ";

        html += "<button onclick='edit(" +
            item.aboutCounterId +
            ")'>Edit</button> ";

        html += "<button onclick='deleteRecord(" +
            item.aboutCounterId +
            ")'>Delete</button>";

        html += "</td>";

        html += "</tr>";
    });

    $("#tblData tbody").html(html);
}

function viewDetails(id) {

    var item = allAboutCounterRecords.find(function (x) {

        return x.aboutCounterId === id;
    });

    if (!item) {
        return;
    }

    showDetails("Counter Box Details", [
        {
            label: "Icon",
            value: item.iconClass
        },
        {
            label: "Number",
            value: item.number + (item.suffix || "")
        },
        {
            label: "Label",
            value: item.label
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
    ]);
}

function edit(id) {

    $.ajax({
        url: "/Admin/AboutCounter/GetById?id=" + id,
        type: "GET",
        success: function (response) {

            if (response.success) {

                var item = response.data;

                $("#Id").val(item.aboutCounterId);

                $("#IconClass")
                    .val(item.iconClass)
                    .trigger("change");

                $("#Number").val(item.number);

                $("#Suffix").val(item.suffix);

                $("#Label").val(item.label);

                $("#IsActive").prop(
                    "checked",
                    item.isActive
                );

                $("#frmData .invalid")
                    .removeClass("invalid");

                $("#frmData .field-error")
                    .removeClass("show")
                    .text("");

                $("#drawer").addClass("active");
            }
        }
    });
}

$("#btnSave").click(function () {

    var ok = validateForm([
        {
            id: "IconClass",
            label: "Icon",
            required: true,
            type: "select"
        },
        {
            id: "Number",
            label: "Number",
            required: true
        },
        {
            id: "Label",
            label: "Label",
            required: true
        }
    ]);

    if (!ok) {
        return;
    }

    var data = {

        "AboutCounter.AboutCounterId":
            $("#Id").val(),

        "AboutCounter.IconClass":
            $("#IconClass").val(),

        "AboutCounter.Number":
            $("#Number").val(),

        "AboutCounter.Suffix":
            $("#Suffix").val(),

        "AboutCounter.Label":
            $("#Label").val(),

        "AboutCounter.IsActive":
            $("#IsActive").is(":checked")
    };

    $.ajax({
        url: "/Admin/AboutCounter/Save",
        type: "POST",
        data: data,
        success: function (response) {

            if (response.success) {

                Swal.fire({
                    icon: "success",
                    title: response.message,
                    timer: 1500,
                    showConfirmButton: false
                });

                clearForm();

                $("#drawer").removeClass("active");

                loadData();

            } else {

                Swal.fire({
                    icon: "error",
                    title: response.message
                });
            }
        }
    });
});

function deleteRecord(id) {

    Swal.fire({
        title: "Delete Record?",
        text: "You won't be able to recover this record.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes",
        cancelButtonText: "Cancel"
    }).then(function (result) {

        if (result.isConfirmed) {

            $.ajax({
                url: "/Admin/AboutCounter/Delete",
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
                        });

                        loadData();

                    } else {

                        Swal.fire({
                            icon: "error",
                            title: response.message
                        });
                    }
                }
            });
        }
    });
}

$("#txtSearch").on("keyup", function () {

    var value = $(this)
        .val()
        .toLowerCase()
        .trim();

    $("#tblData tbody tr").each(function () {

        var rowText = $(this)
            .text()
            .toLowerCase();

        $(this).toggle(
            rowText.indexOf(value) > -1
        );
    });
});

function clearForm() {

    $("#Id").val(0);

    $("#IconClass")
        .val("")
        .trigger("change");

    $("#Number").val("");

    $("#Suffix").val("+");

    $("#Label").val("");

    $("#IsActive").prop(
        "checked",
        true
    );

    $("#frmData .invalid")
        .removeClass("invalid");

    $("#frmData .field-error")
        .removeClass("show")
        .text("");
}