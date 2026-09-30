var allFeedbackRecords = [];

// Page load: setup and event handlers
$(document).ready(function () {
    loadData();
});

// Load data
function loadData() {

    // AJAX call to /Admin/Feedback/GetAll
    $.ajax({
        url: "/Admin/Feedback/GetAll",
        type: "GET", success:
            function (r) {
                if (r.success)
                    bindTable(r.data);
            }
    });
}

// Bind table
function bindTable(data) {
    allFeedbackRecords = data;
    var html = "";
    $.each(data, function (i, item) {
        html += "<tr>";
        html += "<td><img src='/" + (item.imagePath || "images/User/User1.jfif") + "' width='50'/></td>";
        html += "<td>" + item.name + "<br><small>" + (item.designation || "") + "</small></td>";
        html += "<td>" + "★".repeat(item.rating) + "</td>";
        html += "<td class='cellMessage'>" + item.message + "</td>";
        html += "<td>" + new Date(item.createdDate).toLocaleDateString() + "</td>";
        html += "<td>" + (item.isApproved ? "<span>Approved</span>" : "<span>Pending</span>") + "</td>";
        html += "<td>";
        html += "<button onclick='viewDetails(" + item.feedbackId + ")'>Details</button> ";
        html += "<button onclick='toggleApproval(" + item.feedbackId + ")'>" + (item.isApproved ? "Unapprove" : "Approve") + "</button> ";
        html += "<button onclick='deleteRecord(" + item.feedbackId + ")'>Delete</button>";
        html += "</td>";
        html += "</tr>";
    });
    $("#tblData tbody").html(html);
}

// View details
function viewDetails(id) {
    var item = allFeedbackRecords.find(function (x) { return x.feedbackId === id; });
    if (!item) return;
    showDetails("Feedback Details", [
        { label: "Name", value: item.name },
        { label: "Designation", value: item.designation },
        { label: "Rating", value: "★".repeat(item.rating) },
        { label: "Message", value: item.message },
        { label: "Submitted On", value: new Date(item.createdDate).toLocaleString() },
        { label: "Status", value: item.isApproved ? "Approved" : "Pending" },
        { label: "Photo", value: item.imagePath, isImage: true }
    ]);
}

// Toggle approval
function toggleApproval(id) {

    // AJAX call to /Admin/Feedback/ToggleApproval
    $.ajax({
        url: "/Admin/Feedback/ToggleApproval",
        type: "POST", data: { id: id },
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

// Delete record
function deleteRecord(id) {
    Swal.fire({
        title: "Delete Feedback?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes"
    }).then((result) => {
        if (result.isConfirmed) {

            // AJAX call to /Admin/Feedback/Delete
            $.ajax({
                url: "/Admin/Feedback/Delete",
                type: "POST",
                data: { id: id },
                success: function (r) {
                    if (r.success) {
                        Swal.fire({
                            icon: 'success',
                            title: r.message,
                            timer: 1500,
                            showConfirmButton: false
                        }); loadData();
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
