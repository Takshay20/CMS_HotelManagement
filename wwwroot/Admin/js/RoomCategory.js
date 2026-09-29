var allRoomCategories = [];
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

function loadData() {
    $.ajax({
        url: "/Admin/RoomCategory/GetAll",
        type: "GET",
        success: function (response) {
            if (response.success) bindTable(response.data);
        }
    });
}

function bindTable(data) {
    allRoomCategories = data;
    var html = "";
    $.each(data, function (index, item) {
        html += "<tr>";
        html += "<td>" + item.name + "</td>";
        html += "<td>" + (item.description || "") + "</td>";
        html += "<td>" + item.displayOrder + "</td>";
        html += "<td>" + (item.isActive ? "<span>Active</span>" : "<span>Inactive</span>") + "</td>";
        html += "<td><button onclick='viewDetails(" + item.roomCategoryId + ")'>Details</button> <button onclick='edit(" + item.roomCategoryId + ")'>Edit</button> <button onclick='deleteRecord(" + item.roomCategoryId + ")'>Delete</button></td>";
        html += "</tr>";
    });
    $("#tblData tbody").html(html);
}

function viewDetails(id) {
    var item = allRoomCategories.find(function (x) {
        return x.roomCategoryId === id;
    });
    if (!item) return;
    showDetails("Room Category Details", [
        { label: "Name", value: item.name },
        { label: "Description", value: item.description },
        { label: "Display Order", value: item.displayOrder },
        { label: "Status", value: item.isActive ? "Active" : "Inactive" }
    ]);
}

function edit(id) {
    $.ajax({
        url: "/Admin/RoomCategory/GetById?id=" + id,
        type: "GET",
        success: function (response) {
            if (response.success) {
                var item = response.data;
                $("#Id").val(item.roomCategoryId);
                $("#Name").val(item.name);
                $("#Description").val(item.description);
                $("#IsActive").prop("checked", item.isActive);
                $("#drawer").addClass("active");
            }
        }
    });
}

$("#btnSave").click(function () {
    var ok = validateForm([
        {
            id: "Name",
            label: "Category Name",
            required: true
        }
    ]);
    if (!ok) return;
    var data = {
        "RoomCategory.RoomCategoryId": $("#Id").val(),
        "RoomCategory.Name": $("#Name").val(),
        "RoomCategory.Description": $("#Description").val(),
        "RoomCategory.IsActive": $("#IsActive").is(":checked")
    };
    $.ajax({
        url: "/Admin/RoomCategory/Save",
        type: "POST",
        data: data,
        success: function (response) {
            if (response.success) {
                Swal.fire({
                    icon: 'success',
                    title: response.message,
                    timer: 1500,
                    showConfirmButton: false
                });
                clearForm();
                $("#drawer").removeClass("active");
                loadData();
            } else {
                Swal.fire({
                    icon: 'error',
                    title: response.message
                });
            }
        }
    });
});

function deleteRecord(id) {
    Swal.fire({
        title: "Delete Record?",
        text: "This category cannot be deleted if rooms are assigned to it.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes"
    })
        .then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: "/Admin/RoomCategory/Delete",
                    type: "POST",
                    data: { id: id },
                    success: function (response) {
                        if (response.success) {
                            Swal.fire({
                                icon: 'success',
                                title: response.message,
                                timer: 1500,
                                showConfirmButton: false
                            });
                            loadData();
                        }
                        else {
                            Swal.fire({
                                icon: 'error',
                                title: response.message
                            });
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

function clearForm() {
    $("#Id").val(0);
    $("#Name").val("");
    $("#Description").val("");
    $("#IsActive").prop("checked", true);
}
