$(document).ready(function () {
    // Common Part
    var $moduleSelect = $("#moduleSelect");
    var $btnModuleSelect = $("#btnModuleSelect");
    var $moduleDropdown = $("#moduleDropdown");
    var $search = $("#txtModuleSearch");
    window.currentRestoreFilter = "All";
    $btnModuleSelect.on("click", function (e) {
        e.stopPropagation();
        $moduleDropdown.toggleClass("show");
        $btnModuleSelect.toggleClass("active", $moduleDropdown.hasClass("show"));
        if ($moduleDropdown.hasClass("show")) {
            $search.val("");
            $(".module-option").show();
            setTimeout(function () {
                $search.focus();
            }, 100);
        }
    });
    $(".module-option").on("click", function (e) {
        e.stopPropagation();
        var value = $(this).data("value");
        var name = $(this).data("name");
        var icon = $(this).find("i").attr("class");
        $("#selectedModule").html("<i class='" + icon + "'></i> " + name);
        $(".module-option").removeClass("selected");
        $(this).addClass("selected");
        $moduleDropdown.removeClass("show");
        $btnModuleSelect.removeClass("active");
        window.currentRestoreFilter = "All";
        loadModule(value);
    });
    $search.on("keyup", function () {
        var searchText = $(this).val().toLowerCase().trim();
        $(".module-option").each(function () {
            var name = $(this).data("name").toLowerCase();
            if (name.indexOf(searchText) !== -1) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    });
    $(document).on("click", function (e) {
        if (!$(e.target).closest("#moduleSelect").length) {
            $moduleDropdown.removeClass("show");
            $btnModuleSelect.removeClass("active");
        }
    });
    function loadModule(module) {
        currentModule = module;
        loadRecords();
    }
    $("#recordContainer").on("input", "#txtRecordSearch", function () {
        var value = $(this).val().toLowerCase();
        $("#tblRestoreRecords tbody tr").each(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) !== -1);
        });
    });
    $("#recordContainer").on("change", "#ddlRecordFilter", function () {
        window.currentRestoreFilter = $(this).val();
        loadRecords();
    });
    $("#recordContainer").on("click", ".action-edit", function () {
        var controller = restoreModules[currentModule].controller;
        window.location.href = "/Admin/" + controller + "/" + controller + "?editId=" + $(this).data("id");
    });
    $("#recordContainer").on("click", ".action-delete", function () {
        confirmAction("Delete Record?", "This record will be moved to deleted records.", "Yes, Delete", "/Admin/" + restoreModules[currentModule].controller + "/Delete", { id: $(this).data("id") });
    });
    $("#recordContainer").on("click", ".action-restore", function () {
        var remaining = Math.ceil(($(this).data("ready") - Date.now()) / 1000);
        if (remaining > 0) {
            showRestoreTimer(remaining);
            return;
        }
        confirmAction("Restore Record?", "This record will be restored.", "Yes, Restore", "/Admin/RestoreRecords/Restore", { module: currentModule, id: $(this).data("id") });
    });
});
// Module list: controller name is used for Edit and Delete, cols = [column, heading, type]
var currentModule = "";
var restoreModules = {
    // Module: Counter Box
    CounterBox: { controller: "AboutCounter", cols: [["iconClass", "Icon", "icon"], ["number", "Number"], ["suffix", "Suffix"], ["label", "Label"], ["displayOrder", "Display Order"]] },
    // Module: Rooms
    Rooms: { controller: "Room", cols: [["roomNumber", "Room No"], ["title", "Title"], ["pricePerNight", "Price"], ["maxGuests", "Guests"]] },
    // Module: Why Choose Us
    WhyChooseUs: { controller: "HomeWhyChooseUs", cols: [["iconClass", "Icon", "icon"], ["title", "Title"], ["description", "Description"], ["displayOrder", "Display Order"]] },
    // Module: Room Category
    RoomCategory: { controller: "RoomCategory", cols: [["name", "Name"], ["description", "Description"], ["displayOrder", "Display Order"]] },
    // Module: Amenity
    Amenity: { controller: "Amenity", cols: [["iconClass", "Icon", "icon"], ["name", "Name"]] },
    // Module: Gallery
    Gallery: { controller: "Gallery", cols: [["imagePath", "Image", "img"], ["title", "Title"], ["category", "Category"], ["displayOrder", "Display Order"]] },
    // Module: Facility
    Facility: { controller: "Facility", cols: [["imagePath", "Image", "img"], ["title", "Title"], ["description", "Description"], ["displayOrder", "Display Order"]] },
    // Module: Slider
    Slider: { controller: "Slider", cols: [["imagePath", "Image", "img"], ["pageKey", "Page"], ["title", "Title"], ["displayOrder", "Display Order"]] },
    // Module: Navbar
    Navbar: { controller: "Navbar", cols: [["title", "Title"], ["url", "Url"], ["displayOrder", "Display Order"]] },
    // Module: Social Media
    SocialMedia: { controller: "SocialMedia", cols: [["iconClass", "Icon", "icon"], ["platformName", "Platform"], ["url", "Url"], ["displayOrder", "Display Order"]] }
};
function esc(value) {
    return $("<div>").text(value == null ? "" : value).html();
}
function loadRecords() {
    $.get("/Admin/RestoreRecords/GetRecords", { module: currentModule, filter: window.currentRestoreFilter }, function (response) {
        if (!response.success) {
            $("#recordContainer").html("<div class='table-container'><p style='padding:20px;'>Unable to load records.</p></div>");
            return;
        }
        var cols = restoreModules[currentModule].cols;
        var html = "<div class='restore-toolbar'><div class='restore-search'><i class='fa-solid fa-magnifying-glass'></i><input type='text' id='txtRecordSearch' placeholder='Search Here...' /></div>";
        html += "<select id='ddlRecordFilter' class='record-filter'><option value='All'>All</option><option value='Inactive'>Inactive</option><option value='Deleted'>Deleted</option></select></div>";
        html += "<div class='table-container'><table id='tblRestoreRecords'><thead><tr>";
        $.each(cols, function (i, c) { html += "<th>" + c[1] + "</th>"; });
        html += "<th>Status</th><th>Action</th></tr></thead><tbody>";
        if (response.data.length === 0) {
            html += "<tr><td colspan='" + (cols.length + 2) + "' style='text-align:center;'>No records found.</td></tr>";
        }
        $.each(response.data, function (i, item) {
            html += "<tr>";
            $.each(cols, function (j, c) {
                var value = item[c[0]];
                if (c[2] === "icon") html += "<td class='tableIcon'><i class='" + esc(value) + "'></i></td>";
                else if (c[2] === "img") html += "<td><img src='/" + esc(value) + "' width='60' /></td>";
                else html += "<td>" + esc(value) + "</td>";
            });
            html += item.isDeleted ? "<td><span class='status-badge status-deleted'>Deleted</span></td>" : "<td><span class='status-badge status-inactive'>Inactive</span></td>";
            html += "<td>";
            if (item.isDeleted) {
                var readyAt = Date.now() + Math.max(item.remainingSeconds || 0, 0) * 1000;
                html += "<button type='button' class='action-btn action-restore' title='Restore' data-id='" + item.id + "' data-ready='" + readyAt + "'><i class='fa-solid fa-rotate-left'></i></button>";
            } else {
                html += "<button type='button' class='action-btn action-edit' title='Edit' data-id='" + item.id + "'><i class='fa-solid fa-pen'></i></button>";
                html += "<button type='button' class='action-btn action-delete' title='Delete' data-id='" + item.id + "'><i class='fa-solid fa-trash'></i></button>";
            }
            html += "</td></tr>";
        });
        $("#recordContainer").html(html + "</tbody></table></div>");
        $("#ddlRecordFilter").val(window.currentRestoreFilter);
    }).fail(function () {
        $("#recordContainer").html("<div class='table-container'><p style='padding:20px;'>Unable to load records.</p></div>");
    });
}
function confirmAction(title, text, buttonText, url, data) {
    Swal.fire({ title: title, text: text, icon: "question", showCancelButton: true, confirmButtonText: buttonText }).then(function (result) {
        if (!result.isConfirmed) return;
        $.post(url, data, function (response) {
            Swal.fire({ icon: response.success ? "success" : "error", title: response.success ? "Done" : "Error", text: response.message }).then(function () {
                if (response.success) loadRecords();
            });
        }).fail(function () {
            Swal.fire({ icon: "error", title: "Error", text: "Something went wrong." });
        });
    });
}
function showRestoreTimer(remaining) {
    var endTime = Date.now() + remaining * 1000;
    var timerInterval;
    function left() {
        var s = Math.max(Math.ceil((endTime - Date.now()) / 1000), 0);
        return [Math.floor(s / 3600), Math.floor(s % 3600 / 60), s % 60].map(function (n) { return String(n).padStart(2, "0"); }).join(":");
    }
    Swal.fire({
        icon: "error",
        title: "Cannot Restore",
        html: "<div style='font-size:16px;'>This record can be restored only after 1 hour.</div><div style='margin-top:18px;font-size:15px;font-weight:600;'>Restore available in</div><div id='restoreTimer' style='margin-top:8px;font-size:30px;font-weight:700;color:#b8860b;'>" + left() + "</div>",
        confirmButtonText: "OK",
        didOpen: function () {
            timerInterval = setInterval(function () {
                $("#restoreTimer").text(left());
                if (Date.now() >= endTime) {
                    clearInterval(timerInterval);
                    $("#restoreTimer").css("color", "green");
                }
            }, 1000);
        },
        willClose: function () { clearInterval(timerInterval); }
    });
}
