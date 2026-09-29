function bindCharts(data) {
    bindMetricsChart(data);
    bindBookingChart(data);
}
function bindMetricsChart(data) {
    var metricsCanvas = document.getElementById("metricsChart");
    if (!metricsCanvas) {
        return;
    }
    var metricsCtx = metricsCanvas.getContext("2d");
    new Chart(metricsCtx, {
        type: "bar",
        data: {
            labels: [
                "Rooms",
                "Categories",
                "Users",
                "Bookings",
                "Messages",
                "Feedback"
            ],
            datasets: [{
                label: "Count",
                data: [
                    data.totalRooms,
                    data.totalCategories,
                    data.totalUsers,
                    data.totalBookings,
                    data.unreadMessages,
                    data.pendingFeedback
                ],
                backgroundColor: [
                    "#D4AF37",
                    "#0F172A",
                    "#2E86AB",
                    "#8E44AD",
                    "#C0392B",
                    "#16A085"
                ],

                borderRadius: 6
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    display: false
                }
            },
            scales: {
                y: {
                    beginAtZero: true,

                    ticks: {
                        precision: 0
                    }
                }
            },
            onClick: function (event, elements) {
                if (!elements.length) {
                    return;
                }
                var index = elements[0].index;
                var label = this.data.labels[index];
                if (label === "Rooms") {
                    window.location.href = "/Admin/Room/Room";
                }
                else if (label === "Categories") {
                    window.location.href = "/Admin/RoomCategory/RoomCategory";
                }
                else if (label === "Users") {
                    window.location.href = "/Admin/User/User";
                }
                else if (label === "Bookings") {
                    window.location.href = "/Admin/Booking/Booking";
                }
                else if (label === "Messages") {
                    window.location.href = "/Admin/ContactMessage/ContactMessage";
                }
                else if (label === "Feedback") {
                    window.location.href = "/Admin/Feedback/Feedback";
                }
            },
            onHover: function (event, elements) {
                event.native.target.style.cursor =
                    elements.length ? "pointer" : "default";
            }
        }
    });
}
function bindBookingChart(data) {
    var bookingCanvas = document.getElementById("bookingChart");
    if (!bookingCanvas) {
        return;
    }
    var bookingCtx = bookingCanvas.getContext("2d");
    new Chart(bookingCtx, {
        type: "doughnut",
        data: {
            labels: [
                "Pending",
                "Approved",
                "Rejected",
                "Cancelled"
            ],
            datasets: [{
                data: [
                    data.pendingBookings,
                    data.approvedBookings,
                    data.rejectedBookings,
                    data.cancelledBookings
                ],
                backgroundColor: [
                    "#E67E22",
                    "#27AE60",
                    "#C0392B",
                    "#7F8C8D"
                ],
                borderWidth: 2,
                borderColor: "#FFFFFF"
            }]
        },
        options: {
            responsive: true,

            plugins: {
                legend: {
                    position: "bottom"
                }
            },
            onClick: function (event, elements) {
                if (!elements.length) {
                    return;
                }
                var index = elements[0].index;
                var label = this.data.labels[index];

                if (label === "Pending") {
                    window.location.href =
                        "/Admin/Booking/Booking?status=Pending";
                }
                else if (label === "Approved") {
                    window.location.href =
                        "/Admin/Booking/Booking?status=Approved";
                }
                else if (label === "Rejected") {
                    window.location.href =
                        "/Admin/Booking/Booking?status=Rejected";
                }
                else if (label === "Cancelled") {
                    window.location.href =
                        "/Admin/Booking/Booking?status=Cancelled";
                }
            },
            onHover: function (event, elements) {
                event.native.target.style.cursor =
                    elements.length ? "pointer" : "default";
            }
        }
    });
}