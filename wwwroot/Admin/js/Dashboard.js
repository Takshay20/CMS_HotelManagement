$(document).ready(function () {
    loadCounts();
    loadReminders();
});
function loadCounts() {
    $.ajax({
        url: "/Admin/Dashboard/GetCounts",
        type: "GET",
        success: function (r) {
            if (r.success) {
                bindCards(r.data);
                bindCharts(r.data);
            }
        }
    });
}
function card(icon, label, value, cls, url) {
    return "<a href='" + url + "' class='dashCard " + cls + "'>" +
        "<div class='icon'>" + icon + "</div>" +
        "<div class='value'>" + value + "</div>" +
        "<div class='label'>" + label + "</div>" +
        "</a>";
}
function bindCards(data) {
    var html = "";
    html += card("🛏", "Total Rooms", data.totalRooms, "card-rooms", "/Admin/Room/Room");
    html += card("🏷", "Room Categories", data.totalCategories, "card-categories", "/Admin/RoomCategory/RoomCategory");
    html += card("👤", "Registered Users", data.totalUsers, "card-users", "/Admin/User/User");
    html += card("📅", "Total Bookings", data.totalBookings, "card-bookings", "/Admin/Booking/Booking");
    html += card("⏳", "Pending Bookings", data.pendingBookings, "card-pending", "/Admin/Booking/Booking?status=Pending");
    html += card("✅", "Approved Bookings", data.approvedBookings, "card-approved", "/Admin/Booking/Booking?status=Approved");
    html += card("✉", "Unread Messages", data.unreadMessages, "card-messages", "/Admin/ContactMessage/ContactMessage");
    html += card("⭐", "Feedback Awaiting Approval", data.pendingFeedback, "card-feedback", "/Admin/Feedback/Feedback");
    html += card("💰", "Total Revenue (Approved)", "₹" + Number(data.totalRevenue).toLocaleString(), "card-revenue", "/Admin/Booking/Booking?status=Approved");
    html += card("🛎️", "Check-ins Today", data.todayCheckIns, "card-checkin", "/Admin/Booking/Booking?status=Approved");
    html += card("🧳", "Check-outs Today", data.todayCheckOuts, "card-checkout", "/Admin/Booking/Booking?status=Approved");
    $("#dashGrid").html(html);
}
function loadReminders() {
    $.ajax({
        url: "/Admin/Dashboard/GetReminders",
        type: "GET",
        success: function (r) {
            if (r.success) {
                bindReminderList("#arrivalsList", r.data.arrivals, "checkInDate", "Check-in");
                bindReminderList("#departuresList", r.data.departures, "checkOutDate", "Check-out");
            }
        }
    });
}
function bindReminderList(containerId, list, dateField, label) {
    if (!list || list.length === 0) {
        $(containerId).html(
            "<p class='text-muted'>No " +
            label.toLowerCase() +
            "s in the next 2 days.</p>"
        );
        return;
    }
    var today = new Date();
    today.setHours(0, 0, 0, 0);
    var html = "";
    $.each(list, function (i, b) {
        var d = new Date(b[dateField]);
        d.setHours(0, 0, 0, 0);
        var isToday = d.getTime() == today.getTime();
        var badge = isToday
            ? "<span class='badge badge-danger'>TODAY</span>"
            : "<span class='badge badge-warning'>TOMORROW</span>";
        html += "<div class='reminderRow'>";
        html += "<div>";
        html += "<strong>" + b.fullName + "</strong><br>";
        html += "<small class='text-muted'>" +
            b.roomTitle +
            " (#" +
            b.roomNumber +
            ")</small>";
        html += "</div>";
        html += "<div class='reminderRight'>" +
            badge +
            "<br><small class='text-muted'>" +
            new Date(b[dateField]).toLocaleDateString() +
            "</small></div>";
        html += "</div>";
    });
    $(containerId).html(html);
}