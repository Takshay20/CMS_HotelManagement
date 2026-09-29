var allUserRecords = [];
$(document).ready(function () {
    loadData();
});

function loadData() {
    $.ajax({
        url: "/Admin/User/GetAll",
        type: "GET",
        success: function (r) {
            if (r.success) bindTable(r.data);
        }
    });
}

function bindTable(data) {
    allUserRecords = data;
    var html = "";
    $.each(data, function (i, item) {
        html += "<tr>";
        html += "<td>" + item.fullName + "</td>";
        html += "<td>" + item.email + "</td>";
        html += "<td>" + (item.phone || "") + "</td>";
        html += "<td>" + new Date(item.createdDate).toLocaleDateString() + "</td>";
        html += "<td>" + (item.isActive ? "<span>Active</span>" : "<span>Blocked</span>") + "</td>";
        html += "<td>";
        html += "<button onclick='viewDetails(" + item.userId + ")'>Details</button> ";
        html += "<button onclick='toggleActive(" + item.userId + ")'>" + (item.isActive ? "Block" : "Unblock") + "</button> ";
        html += "<button onclick='deleteRecord(" + item.userId + ")'>Delete</button>";
        html += "</td>";
        html += "</tr>";
    });
    $("#tblData tbody").html(html);
}

function viewDetails(id) {
    var item = allUserRecords.find(function (x) {
        return x.userId === id;
    });
    if (!item) return;
    showDetails("User Details", [
        { label: "Full Name", value: item.fullName },
        { label: "Email", value: item.email },
        { label: "Phone", value: item.phone },
        { label: "Registered On", value: new Date(item.createdDate).toLocaleString() },
        { label: "Status", value: item.isActive ? "Active" : "Blocked" }
    ]);
}

function toggleActive(id) {
    $.ajax({
        url: "/Admin/User/ToggleActive",
        type: "POST",
        data: { id: id },
        success: function (r) {
            if (r.success) {
                Swal.fire({
                    icon: 'success',
                    title: r.message,
                    timer: 1200,
                    showConfirmButton: false
                });
                loadData();
            }
        }
    });
}

function deleteRecord(id) {
    Swal.fire({
        title: "Delete User?",
        text: "This will remove the user account permanently.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: "/Admin/User/Delete",
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
