var allAmenityRecords = [];

var iconList = [
    {
        value: "fa-solid fa-wifi",
        text: "Wi-Fi"
    },
    {
        value: "fa-solid fa-square-parking",
        text: "Parking"
    },
    {
        value: "fa-solid fa-person-swimming",
        text: "Swimming Pool"
    },
    {
        value: "fa-solid fa-dumbbell",
        text: "Gym / Fitness"
    },
    {
        value: "fa-solid fa-spa",
        text: "Spa"
    },
    {
        value: "fa-solid fa-utensils",
        text: "Restaurant"
    },
    {
        value: "fa-solid fa-mug-saucer",
        text: "Breakfast"
    },
    {
        value: "fa-solid fa-martini-glass",
        text: "Bar / Lounge"
    },
    {
        value: "fa-solid fa-snowflake",
        text: "Air Conditioning"
    },
    {
        value: "fa-solid fa-tv",
        text: "Television"
    },
    {
        value: "fa-solid fa-shirt",
        text: "Laundry Service"
    },
    {
        value: "fa-solid fa-elevator",
        text: "Elevator / Lift"
    },
    {
        value: "fa-solid fa-shuttle-van",
        text: "Airport Shuttle"
    },
    {
        value: "fa-solid fa-paw",
        text: "Pet Friendly"
    },
    {
        value: "fa-solid fa-wheelchair",
        text: "Wheelchair Accessible"
    },
    {
        value: "fa-solid fa-smoking",
        text: "Smoking Area"
    },
    {
        value: "fa-solid fa-ban-smoking",
        text: "Non-Smoking"
    },
    {
        value: "fa-solid fa-bell-concierge",
        text: "Room Service"
    },
    {
        value: "fa-solid fa-briefcase",
        text: "Business Center"
    },
    {
        value: "fa-solid fa-mug-hot",
        text: "Tea / Coffee Maker"
    },
    {
        value: "fa-solid fa-lock",
        text: "Safe / Locker"
    },
    {
        value: "fa-solid fa-fire-extinguisher",
        text: "Fire Safety"
    },
    {
        value: "fa-solid fa-camera",
        text: "CCTV Security"
    },
    {
        value: "fa-solid fa-child",
        text: "Kids Play Area"
    },
    {
        value: "fa-solid fa-water",
        text: "Hot Water"
    }
];

$(document).ready(function () {

    loadIcons();
    loadData();

    $("#btnAdd").click(function () {
        clearForm();
        $("#drawer").addClass("active");
    });

    $("#closeDrawer, #btnCancel").click(function () {
        $("#drawer").removeClass("active");
    });

    $("#btnSave").click(function () {

        var ok = validateForm([
            {
                id: "IconClass",
                label: "Icon",
                required: true,
                type: "select"
            },
            {
                id: "Name",
                label: "Amenity Name",
                required: true
            }
        ]);

        if (!ok) {
            return;
        }

        var data = {
            "Amenity.AmenityId": $("#Id").val(),
            "Amenity.IconClass": $("#IconClass").val(),
            "Amenity.Name": $("#Name").val(),
            "Amenity.IsActive": $("#IsActive").is(":checked")
        };

        $.ajax({
            url: "/Admin/Amenity/Save",
            type: "POST",
            data: data,

            success: function (r) {

                if (r.success) {

                    Swal.fire({
                        icon: "success",
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
                        icon: "error",
                        title: r.message
                    });
                }
            },

            error: function () {

                Swal.fire({
                    icon: "error",
                    title: "Something went wrong."
                });
            }
        });
    });

    $("#txtSearch").on("keyup", function () {

        var value = $(this).val().toLowerCase();

        $("#tblData tbody tr").filter(function () {

            $(this).toggle(
                $(this).text().toLowerCase().indexOf(value) > -1
            );

        });
    });

    $("#Name").on("input", function () {
        $("#err-Name").text("");
    });

    $("#IconClass").on("change", function () {
        $("#err-IconClass").text("");
    });
});


function loadIcons() {

    var html = "<option value=''></option>";

    $.each(iconList, function (i, icon) {

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
        .attr("placeholder", "Search Icon...");
}


function loadData() {

    $.ajax({
        url: "/Admin/Amenity/GetAll",
        type: "GET",

        success: function (r) {

            if (r.success) {

                bindTable(r.data);
            }
        },

        error: function () {

            Swal.fire({
                icon: "error",
                title: "Unable to load amenities."
            });
        }
    });
}


function bindTable(data) {

    allAmenityRecords = data || [];

    var html = "";

    $.each(allAmenityRecords, function (i, item) {

        html += "<tr>";

        html +=
            "<td>" +
            "<i class='" +
            (item.iconClass || "") +
            "'></i>" +
            "</td>";

        html +=
            "<td>" +
            (item.name || "") +
            "</td>";

        html +=
            "<td>" +
            (
                item.isActive
                    ? "<span>Active</span>"
                    : "<span>Inactive</span>"
            ) +
            "</td>";

        html +=
            "<td>" +

            "<button type='button' onclick='viewDetails(" +
            item.amenityId +
            ")'>Details</button> " +

            "<button type='button' onclick='edit(" +
            item.amenityId +
            ")'>Edit</button> " +

            "<button type='button' onclick='deleteRecord(" +
            item.amenityId +
            ")'>Delete</button>" +

            "</td>";

        html += "</tr>";
    });

    $("#tblData tbody").html(html);
}


function viewDetails(id) {

    var item = allAmenityRecords.find(function (x) {

        return x.amenityId === id;
    });

    if (!item) {
        return;
    }

    showDetails("Amenity Details", [

        {
            label: "Name",
            value: item.name
        },

        {
            label: "Icon Class",
            value: item.iconClass
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

        url: "/Admin/Amenity/GetById?id=" + id,

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

            $("#Id").val(item.amenityId);

            $("#IconClass")
                .val(item.iconClass)
                .trigger("change");

            $("#Name").val(item.name);

            $("#IsActive")
                .prop("checked", item.isActive);

            $(".field-error").text("");

            $("#drawer").addClass("active");
        },

        error: function () {

            Swal.fire({
                icon: "error",
                title: "Unable to load amenity."
            });
        }
    });
}


function deleteRecord(id) {

    Swal.fire({

        title: "Delete Record?",

        text: "Are you sure you want to delete this amenity?",

        icon: "warning",

        showCancelButton: true,

        confirmButtonText: "Yes, Delete",

        cancelButtonText: "Cancel"

    }).then(function (result) {

        if (!result.isConfirmed) {
            return;
        }

        $.ajax({

            url: "/Admin/Amenity/Delete",

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

                    });

                    loadData();
                }
                else {

                    Swal.fire({

                        icon: "error",

                        title: r.message

                    });
                }
            },

            error: function () {

                Swal.fire({

                    icon: "error",

                    title: "Unable to delete amenity."

                });
            }
        });
    });
}


function clearForm() {

    $("#Id").val(0);

    $("#IconClass")
        .val("")
        .trigger("change");

    $("#Name").val("");

    $("#IsActive")
        .prop("checked", true);

    $(".field-error").text("");
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
