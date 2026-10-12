var allAmenities = [];
var allCategories = [];

// Page load: setup and event handlers
$(document).ready(function () {

    loadFormData();
    loadData();

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
                $("#imgPreview")
                    .attr("src", e.target.result)
                    .show();
            };

            reader.readAsDataURL(this.files[0]);
        }
    });

    $("#btnSave").click(function () {

        var isNew = $("#Id").val() == 0;

        var ok = validateForm([
            {
                id: "RoomCategoryId",
                label: "Room Category",
                required: true,
                type: "select"
            },
            {
                id: "RoomNumber",
                label: "Room Number",
                required: true
            },
            {
                id: "Title",
                label: "Title",
                required: true
            },
            {
                id: "PricePerNight",
                label: "Price Per Night",
                required: true
            },
            {
                id: "ImageFile",
                label: "Cover Image",
                required: true,
                type: "file",
                onlyForNew: true,
                isNew: isNew
            }
        ]);

        if (!ok) {
            return;
        }

        // Size format check (e.g. 350 sqft)
        var size = $.trim($("#SizeSqft").val());

        if (size !== "" && !/^\d{2,5}(\.\d+)?\s*(sq\.?\s?ft|sqft|sq\.?\s?feet|square\s?feet)?$/i.test(size)) {
            showFieldError("SizeSqft", "Enter size like 350 sqft.");
            return;
        }

        var checkedAmenities = [];

        $(".amenityCheck:checked").each(function () {
            checkedAmenities.push($(this).val());
        });

        // Prepare form data for upload
        var formData = new FormData();

        formData.append("Room.RoomId", $("#Id").val());
        formData.append("Room.RoomCategoryId", $("#RoomCategoryId").val());
        formData.append("Room.RoomNumber", $("#RoomNumber").val());
        formData.append("Room.Title", $("#Title").val());
        formData.append("Room.Description", $("#Description").val());
        formData.append("Room.PricePerNight", $("#PricePerNight").val());
        formData.append("Room.MaxGuests", $("#MaxGuests").val());
        formData.append("Room.SizeSqft", $("#SizeSqft").val());
        formData.append("Room.AmenityIds", checkedAmenities.join(","));
        formData.append("Room.IsFeatured", $("#IsFeatured").is(":checked"));
        formData.append("Room.IsAvailable", $("#IsAvailable").is(":checked"));

        var image = $("#ImageFile")[0].files[0];

        if (image) {
            formData.append("ImageFile", image);
        }

        var galleryFiles = $("#GalleryFiles")[0].files;

        for (var i = 0; i < galleryFiles.length; i++) {
            formData.append("GalleryFiles", galleryFiles[i]);
        }

        // AJAX call to /Admin/Room/Save
        $.ajax({
            url: "/Admin/Room/Save",
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
                    });

                    clearForm();

                    $("#drawer").removeClass("active");

                    loadData();
                }
                else {

                    // field specific error shows inside the form, others in a pop-up
                    if (r.data && r.data.field) {
                        showFieldError(r.data.field, r.message);
                    }
                    else {
                        Swal.fire({
                            icon: "error",
                            title: r.message
                        });
                    }
                }
            },

            error: function () {
                Swal.fire({
                    icon: "error",
                    title: "Server error",
                    text: "Something went wrong while saving. Please try again."
                });
            }
        });
    });

    $("#txtSearch").keyup(function () {

        var value = $(this).val().toLowerCase();

        $("#tblData tbody tr").filter(function () {

            $(this).toggle(
                $(this).text().toLowerCase().indexOf(value) > -1
            );

        });
    });
});


// Load form data
function loadFormData() {
    $.ajax({
        url: "/Admin/Room/GetFormData",
        type: "GET",

        success: function (r) {

            if (!r.success) {
                return;
            }

            allCategories = r.data.categories;
            allAmenities = r.data.amenities;

            var catHtml = "<option value=''></option>";

            $.each(allCategories, function (i, c) {

                catHtml +=
                    "<option value='" +
                    c.roomCategoryId +
                    "'>" +
                    c.name +
                    "</option>";
            });

            $("#RoomCategoryId").html(catHtml);

            $("#RoomCategoryId").select2({
                placeholder: "-- Select Room Category --",
                allowClear: true,
                width: "100%",
                dropdownParent: $("#drawer")
            });


            var amHtml = "";

            $.each(allAmenities, function (i, a) {

                amHtml +=
                    "<label class='amenityLabel'>" +
                    "<input type='checkbox' class='amenityCheck amenityCheckbox' value='" +
                    a.amenityId +
                    "'> " +
                    a.name +
                    "</label>";
            });

            $("#amenityChecklist").html(amHtml);
        }
    });
}


// Load data
function loadData() {

    // AJAX call to /Admin/Room/GetAll
    $.ajax({
        url: "/Admin/Room/GetAll",
        type: "GET",

        success: function (r) {

            if (r.success) {
                bindTable(r.data);
            }
        }
    });
}


// Bind table
function bindTable(data) {

    var html = "";

    $.each(data, function (i, item) {

        html += "<tr>";

        html +=
            "<td><img src='/" +
            item.imagePath +
            "' width='70'/></td>";

        html +=
            "<td>" +
            item.roomNumber +
            "</td>";

        html +=
            "<td>" +
            item.title +
            "</td>";

        html +=
            "<td>" +
            (item.categoryName || "") +
            "</td>";

        html +=
            "<td>₹" +
            item.pricePerNight +
            "</td>";

        html +=
            "<td>" +
            (item.isFeatured ? "Yes" : "No") +
            "</td>";

        html +=
            "<td>" +
            (
                item.isAvailable
                    ? "<span>Available</span>"
                    : "<span>Unavailable</span>"
            ) +
            "</td>";

        html +=
            "<td>" +
            "<button onclick='viewDetails(" +
            item.roomId +
            ")'>Details</button> " +

            "<button onclick='edit(" +
            item.roomId +
            ")'>Edit</button> " +

            "<button onclick='deleteRecord(" +
            item.roomId +
            ")'>Delete</button>" +

            "</td>";

        html += "</tr>";
    });

    $("#tblData tbody").html(html);
}


// View details
function viewDetails(id) {

    // AJAX call to server
    $.ajax({
        url: "/Admin/Room/GetById?id=" + id,
        type: "GET",

        success: function (r) {

            if (!r.success) {
                return;
            }

            var item = r.data;

            var amenityNames = "—";

            if (item.amenityIds) {

                var ids = item.amenityIds
                    .split(",")
                    .map(function (x) {
                        return x.trim();
                    });

                var names = allAmenities
                    .filter(function (a) {
                        return ids.indexOf(
                            String(a.amenityId)
                        ) > -1;
                    })
                    .map(function (a) {
                        return a.name;
                    });

                if (names.length) {
                    amenityNames = names.join(", ");
                }
            }

            showDetails("Room Details", [

                {
                    label: "Room Number",
                    value: item.roomNumber
                },

                {
                    label: "Title",
                    value: item.title
                },

                {
                    label: "Category",
                    value: item.categoryName
                },

                {
                    label: "Description",
                    value: item.description
                },

                {
                    label: "Price / Night",
                    value: "₹" + item.pricePerNight
                },

                {
                    label: "Max Guests",
                    value: item.maxGuests
                },

                {
                    label: "Size",
                    value: item.sizeSqft
                },

                {
                    label: "Amenities",
                    value: amenityNames
                },

                {
                    label: "Featured on Home Page",
                    value: item.isFeatured ? "Yes" : "No"
                },

                {
                    label: "Status",
                    value: item.isAvailable
                        ? "Available"
                        : "Unavailable"
                },

                {
                    label: "Cover Image",
                    value: item.imagePath,
                    isImage: true
                }

            ]);
        }
    });
}


// Edit
function edit(id) {

    // AJAX call to server
    $.ajax({
        url: "/Admin/Room/GetById?id=" + id,
        type: "GET",

        success: function (r) {

            if (!r.success) {
                return;
            }

            var item = r.data;

            $("#Id").val(item.roomId);

            $("#RoomCategoryId")
                .val(item.roomCategoryId)
                .trigger("change");

            $("#RoomNumber").val(item.roomNumber);

            $("#Title").val(item.title);

            $("#Description").val(item.description);

            $("#PricePerNight").val(item.pricePerNight);

            $("#MaxGuests").val(item.maxGuests);

            $("#SizeSqft").val(item.sizeSqft);

            $("#IsFeatured")
                .prop("checked", item.isFeatured);

            $("#IsAvailable")
                .prop("checked", item.isAvailable);


            $(".amenityCheck")
                .prop("checked", false);

            if (item.amenityIds) {

                var ids = item.amenityIds.split(",");

                $.each(ids, function (i, val) {

                    $(".amenityCheck[value='" +
                        val.trim() +
                        "']")
                        .prop("checked", true);
                });
            }


            if (item.imagePath) {

                $("#imgPreview")
                    .attr(
                        "src",
                        "/" + item.imagePath
                    )
                    .show();
            }

            // additional photos of this room (with delete button)
            renderGallery(item.images);

            $("#drawer").addClass("active");
        }
    });
}


// Delete record
function deleteRecord(id) {

    Swal.fire({

        title: "Delete Record?",

        text: "This will remove the room and all its images.",

        icon: "warning",

        showCancelButton: true,

        confirmButtonText: "Yes"

    }).then(function (result) {

        if (!result.isConfirmed) {
            return;
        }

        // AJAX call to /Admin/Room/Delete
        $.ajax({

            url: "/Admin/Room/Delete",

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
            }
        });
    });
}


// Clear form
function clearForm() {

    $("#Id").val(0);

    $("#RoomNumber").val("");

    $("#Title").val("");

    $("#Description").val("");

    $("#PricePerNight").val("");

    $("#MaxGuests").val(2);

    $("#SizeSqft").val("");


    $("#RoomCategoryId")
        .val("")
        .trigger("change");


    $("#IsFeatured")
        .prop("checked", false);

    $("#IsAvailable")
        .prop("checked", true);


    $("#ImageFile").val("");

    $("#GalleryFiles").val("");

    $("#imgPreview").hide();

    $("#existingGallery").empty().hide();
    $("#galleryHint").hide();

    $(".amenityCheck")
        .prop("checked", false);
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

// Show an error under one form field (cleared automatically when the user types)
function showFieldError(id, message) {
    $("#" + id).addClass("invalid").focus();
    $("#err-" + id).addClass("show").text(message);
}

// Show the saved additional photos of a room inside the edit form
function renderGallery(images) {

    var $box = $("#existingGallery").empty();

    if (!images || images.length === 0) {
        $box.hide();
        $("#galleryHint").hide();
        return;
    }

    var html = "";

    $.each(images, function (i, img) {
        html += '<div class="gallery-item">'
            + '<img src="/' + img.imagePath + '" alt="Room photo" />'
            + '<button type="button" class="gallery-del" data-id="' + img.roomImageId + '" title="Delete photo">'
            + '<i class="fa-solid fa-trash"></i></button>'
            + '</div>';
    });

    $box.html(html).show();
    $("#galleryHint").show();
}

// Delete one additional photo (asks first, removes it from the form on success)
$(document).on("click", ".gallery-del", function () {

    var id = $(this).data("id");
    var $item = $(this).closest(".gallery-item");

    Swal.fire({
        title: "Delete this photo?",
        text: "It will be removed from the room photos.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes, delete"
    }).then(function (result) {

        if (!result.isConfirmed) {
            return;
        }

        $.ajax({
            url: "/Admin/Room/DeleteImage",
            type: "POST",
            data: { roomImageId: id },

            success: function (r) {

                if (r.success) {

                    $item.remove();

                    if ($("#existingGallery .gallery-item").length === 0) {
                        $("#existingGallery").hide();
                        $("#galleryHint").hide();
                    }

                    Swal.fire({
                        icon: "success",
                        title: r.message,
                        timer: 1200,
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
                    title: "Server error",
                    text: "Unable to delete the photo. Please try again."
                });
            }
        });
    });
});
