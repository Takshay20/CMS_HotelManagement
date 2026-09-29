$(document).ready(function () {
    loadData();
});
$("#closeDrawer,#btnCancel").click(function () {
    $("#drawer").removeClass("active");
});

function loadData() {
    $.ajax({
        url: "/Admin/ContactMessage/GetAll",
        type: "GET", success: function (r) {
            if (r.success) bindTable(r.data);
        }
    });
}

function bindTable(data) {
    var html = "";
    $.each(data, function (i, item) {
        html += "<tr>";
        html += "<td>" + item.name + "</td>";
        html += "<td>" + item.email + "<br><small>" + (item.phone || "") + "</small></td>";
        html += "<td>" + (item.subject || "") + "</td>";
        html += "<td class='cell-wrap'>" + item.message + "</td>";
        html += "<td>" + new Date(item.createdDate).toLocaleDateString() + "</td>";
        html += "<td>" + (item.isRead ? "<span>Read</span>" : "<span class='status-new'>New</span>") + "</td>";
        html += "<td>";
        html += "<button onclick='viewMessage(" + item.contactMessageId + ")'>View / Reply</button> ";
        html += "<button onclick='deleteRecord(" + item.contactMessageId + ")'>Delete</button>";
        html += "</td>";
        html += "</tr>";
    });
    $("#tblData tbody").html(html);
}

function viewMessage(id) {
    $.ajax({
        url: "/Admin/ContactMessage/GetById?id=" + id,
        type: "GET",
        success: function (r) {
            if (r.success && r.data) {
                var item = r.data;
                $("#Id").val(item.contactMessageId);
                $("#viewFrom").text(item.name + "  (" + item.email + (item.phone ? ", " + item.phone : "") + ")");
                $("#viewSubject").text(item.subject || "(No subject)");
                $("#viewMessage").text(item.message);
                $("#viewDate").text(new Date(item.createdDate).toLocaleString());

                if (item.adminReply) {
                    $("#viewReply").text(item.adminReply + " — " + new Date(item.repliedDate).toLocaleString());
                    $("#previousReplyWrap").show();
                    $("#AdminReply").val("");
                } else {
                    $("#previousReplyWrap").hide();
                    $("#AdminReply").val("");
                }

                $("#drawer").addClass("active");
                loadData();
            }
        }
    });
}

$("#btnSave").click(function () {
    var ok = validateForm([
        {
            id: "AdminReply",
            label: "Reply message",
            required: true
        }
    ]);
    if (!ok) return;

    $.ajax({
        url: "/Admin/ContactMessage/Reply",
        type: "POST",
        data: {
            id: $("#Id").val(),
            adminReply: $("#AdminReply").val()
        },
        success: function (r) {
            if (r.success) {
                Swal.fire({
                    icon: 'success',
                    title: r.message,
                    timer: 1500,
                    showConfirmButton: false
                });
                $("#drawer").removeClass("active");
                loadData();
            } else {
                Swal.fire({
                    icon: 'error',
                    title: r.message
                });
            }
        }
    });
});

function deleteRecord(id) {
    Swal.fire({ title: "Delete Message?", icon: "warning", showCancelButton: true, confirmButtonText: "Yes" }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({ url: "/Admin/ContactMessage/Delete", type: "POST", data: { id: id }, success: function (r) { if (r.success) { Swal.fire({ icon: 'success', title: r.message, timer: 1500, showConfirmButton: false }); loadData(); } } });
        }
    });
}

$("#txtSearch").keyup(function () {
    var value = $(this).val().toLowerCase();
    $("#tblData tbody tr").filter(function () { $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1); });
});
