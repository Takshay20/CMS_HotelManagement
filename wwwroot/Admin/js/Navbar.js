var allNavbarRecords = [];

// Page load: setup and event handlers
$(document).ready(function () { loadData(); });
$("#btnAdd").click(function () { clearForm(); $("#drawer").addClass("active"); });
$("#closeDrawer,#btnCancel").click(function () { $("#drawer").removeClass("active"); });

// Load data
function loadData() {

    // AJAX call to server
    $.ajax({ url: "/Admin/Navbar/GetAll", type: "GET", success: function (r) { if (r.success) bindTable(r.data); } });
}

// Bind table
function bindTable(data) {
    allNavbarRecords = data;
    var html = "";
    $.each(data, function (i, item) {
        html += "<tr>";
        html += "<td>" + item.title + "</td>";
        html += "<td>" + item.url + "</td>";
        html += "<td>" + item.displayOrder + "</td>";
        html += "<td>" + (item.isActive ? "<span>Active</span>" : "<span>Inactive</span>") + "</td>";
        html += "<td><button onclick='viewDetails(" + item.menuMasterId + ")'>Details</button> <button onclick='edit(" + item.menuMasterId + ")'>Edit</button> <button onclick='deleteRecord(" + item.menuMasterId + ")'>Delete</button></td>";
        html += "</tr>";
    });
    $("#tblData tbody").html(html);
}

// View details
function viewDetails(id) {
    var item = allNavbarRecords.find(function (x) {
        return x.menuMasterId === id;
    });
    if (!item) return;
    showDetails("Navbar Item Details", [
        { label: "Title", value: item.title },
        { label: "Url", value: item.url },
        { label: "Display Order", value: item.displayOrder },
        { label: "Status", value: item.isActive ? "Active" : "Inactive" }
    ]);
}

// Edit
function edit(id) {

    // AJAX call to server
    $.ajax({
        url: "/Admin/Navbar/GetById?id=" + id,
        type: "GET",
        success: function (r) {
            if (r.success) {
                var item = r.data;
                $("#Id").val(item.menuMasterId);
                $("#Title").val(item.title);
                $("#Url").val(item.url);
                $("#IsActive").prop("checked", item.isActive);
                $("#drawer").addClass("active");
            }
        }
    });
}

$("#btnSave").click(function () {
    var ok = validateForm([
        { id: "Title", label: "Title", required: true },
        { id: "Url", label: "Url", required: true }
    ]);
    if (!ok) return;

    var data = {
        "MenuMaster.MenuMasterId": $("#Id").val(),
        "MenuMaster.Title": $("#Title").val(),
        "MenuMaster.Url": $("#Url").val(),
        "MenuMaster.IsActive": $("#IsActive").is(":checked")
    };

    // AJAX call to /Admin/Navbar/Save
    $.ajax({
        url: "/Admin/Navbar/Save",
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

// Delete record
function deleteRecord(id) {
    Swal.fire({
        title: "Delete Menu Item?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes"
    }).then((result) => {
        if (result.isConfirmed) {

            // AJAX call to /Admin/Navbar/Delete
            $.ajax({
                url: "/Admin/Navbar/Delete",
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
    $("#Url").val("");
    $("#IsActive").prop("checked", true);
    $("#frmData .invalid").removeClass("invalid");
    $("#frmData .field-error").removeClass("show").text("");
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
