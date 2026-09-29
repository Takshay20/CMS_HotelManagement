var bcYear, bcMonth;
var bcData = { rooms: [], bookings: [] };

var bcDayNames = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];

var bcMonthNames = [
    "January",
    "February",
    "March",
    "April",
    "May",
    "June",
    "July",
    "August",
    "September",
    "October",
    "November",
    "December"
];

$(document).ready(function () {

    var today = new Date();

    bcYear = today.getFullYear();
    bcMonth = today.getMonth() + 1;

    $("#bcCategory").select2({
        placeholder: "All Categories",
        width: "220px"
    });

    loadCategories();
    loadMonth();

    $("#bcPrev").click(function () {

        bcMonth--;

        if (bcMonth < 1) {
            bcMonth = 12;
            bcYear--;
        }

        loadMonth();
    });

    $("#bcNext").click(function () {

        bcMonth++;

        if (bcMonth > 12) {
            bcMonth = 1;
            bcYear++;
        }

        loadMonth();
    });

    $("#bcToday").click(function () {

        var t = new Date();

        bcYear = t.getFullYear();
        bcMonth = t.getMonth() + 1;

        loadMonth();
    });

    $("#bcCategory").change(function () {
        loadMonth();
    });
});


function loadCategories() {

    $.ajax({
        url: "/Admin/BookingCalendar/GetCategories",
        type: "GET",

        success: function (r) {

            if (!r.success) {
                console.error(
                    "BookingCalendar: Categories could not be loaded.",
                    r.message
                );
                return;
            }

            var $category = $("#bcCategory");

            $category.empty();

            $category.append(
                $("<option>", {
                    value: "0",
                    text: "All Categories"
                })
            );

            $.each(r.data, function (i, c) {

                $category.append(
                    $("<option>", {
                        value: c.roomCategoryId,
                        text: c.name
                    })
                );

            });

            $category.val("0").trigger("change");
        },

        error: function (xhr) {

            console.error(
                "BookingCalendar: GetCategories failed",
                xhr.status,
                xhr.responseText
            );
        }
    });
}


function loadMonth() {

    $("#bcMonthLabel").text(
        bcMonthNames[bcMonth - 1] + " " + bcYear
    );

    var categoryId = $("#bcCategory").val() || 0;

    $(".bc-scroll").html(
        "<div class='bc-empty-msg'>Loading calendar...</div>"
    );

    $.ajax({
        url: "/Admin/BookingCalendar/GetMonthData",
        type: "GET",

        data: {
            year: bcYear,
            month: bcMonth,
            categoryId: categoryId
        },

        success: function (r) {

            if (r.success) {

                $(".bc-scroll").html(
                    "<table class='bc-grid' id='bcGrid'></table>"
                );

                bcData = r.data;

                renderGrid();

            } else {

                $(".bc-scroll").html(
                    "<div class='bc-empty-msg'>" +
                    (r.message || "Unable to load calendar data.") +
                    "</div>"
                );
            }
        },

        error: function (xhr) {

            console.error(
                "BookingCalendar: GetMonthData failed",
                xhr.status,
                xhr.responseText
            );

            var detail =
                xhr.status === 401 || xhr.status === 403
                    ? "Your session may have expired. Please log in to the Admin panel again."
                    : "Server error (" +
                      xhr.status +
                      "). Check the browser console (F12) for details.";

            $(".bc-scroll").html(
                "<div class='bc-empty-msg text-danger'>" +
                "Could not load the booking calendar.<br>" +
                detail +
                "</div>"
            );
        }
    });
}


function renderGrid() {

    var daysInMonth =
        new Date(bcYear, bcMonth, 0).getDate();

    var today = new Date();

    var isCurrentMonth =
        today.getFullYear() === bcYear &&
        today.getMonth() + 1 === bcMonth;

    var todayDate = today.getDate();

    if (bcData.rooms.length === 0) {

        $(".bc-scroll").html(
            "<div class='bc-empty-msg'>" +
            "No rooms found for this category. Add rooms from Admin &gt; Room &gt; Rooms first." +
            "</div>"
        );

        return;
    }

    var headHtml =
        "<thead><tr>" +
        "<th class='bc-room-col'>Room</th>";

    for (var d = 1; d <= daysInMonth; d++) {

        var dow =
            new Date(
                bcYear,
                bcMonth - 1,
                d
            ).getDay();

        var isWeekend =
            dow === 0 || dow === 6;

        headHtml +=
            "<th class='" +
            (isWeekend ? "bc-weekend" : "") +
            "'>" +
            bcDayNames[dow][0] +
            "<br>" +
            d +
            "</th>";
    }

    headHtml += "</tr></thead>";

    var bodyHtml = "<tbody>";

    $.each(bcData.rooms, function (i, room) {

        bodyHtml +=
            "<tr>" +
            "<td class='bc-room-col'>" +
            room.title +
            "<br>" +
            "<small class='text-muted'>#" +
            room.roomNumber +
            "</small>" +
            "</td>";

        for (var d = 1; d <= daysInMonth; d++) {

            var cellDate =
                new Date(
                    bcYear,
                    bcMonth - 1,
                    d
                );

            var booking =
                findBookingForRoomDay(
                    room.roomId,
                    cellDate
                );

            var isToday =
                isCurrentMonth &&
                d === todayDate;

            var cls =
                "bc-day-cell" +
                (isToday ? " bc-today-col" : "");

            if (booking) {

                cls +=
                    " bc-booked " +
                    (
                        booking.status === "Approved"
                            ? "bc-approved"
                            : "bc-pending"
                    );

                bodyHtml +=
                    "<td class='" +
                    cls +
                    "' onclick='showBookingInfo(" +
                    booking.bookingId +
                    ")' title='" +
                    booking.fullName +
                    " (" +
                    booking.status +
                    ")'>" +
                    "</td>";

            } else {

                bodyHtml +=
                    "<td class='" +
                    cls +
                    "'></td>";
            }
        }

        bodyHtml += "</tr>";
    });

    bodyHtml += "</tbody>";

    $("#bcGrid").html(
        headHtml + bodyHtml
    );
}


function findBookingForRoomDay(roomId, dateObj) {

    var dateStr =
        formatDateYMD(dateObj);

    for (var i = 0; i < bcData.bookings.length; i++) {

        var b =
            bcData.bookings[i];

        if (
            b.roomId === roomId &&
            dateStr >= b.checkInDate &&
            dateStr < b.checkOutDate
        ) {
            return b;
        }
    }

    return null;
}


function formatDateYMD(d) {

    var m =
        (d.getMonth() + 1)
        .toString()
        .padStart(2, "0");

    var day =
        d.getDate()
        .toString()
        .padStart(2, "0");

    return (
        d.getFullYear() +
        "-" +
        m +
        "-" +
        day
    );
}


function showBookingInfo(bookingId) {

    var booking = null;

    for (
        var i = 0;
        i < bcData.bookings.length;
        i++
    ) {

        if (
            bcData.bookings[i].bookingId ===
            bookingId
        ) {

            booking =
                bcData.bookings[i];

            break;
        }
    }

    if (!booking) {
        return;
    }

    Swal.fire({

        title: booking.fullName,

        html:
            "<div class='text-left'>" +
            "<p><strong>Status:</strong> " +
            booking.status +
            "</p>" +

            "<p><strong>Check-in:</strong> " +
            booking.checkInDate +
            "</p>" +

            "<p><strong>Check-out:</strong> " +
            booking.checkOutDate +
            "</p>" +

            "<p><strong>Guests:</strong> " +
            booking.guests +
            "</p>" +

            "</div>",

        icon: "info",

        showCancelButton: true,

        confirmButtonText:
            "Manage In Bookings",

        cancelButtonText:
            "Close"

    }).then(function (result) {

        if (result.isConfirmed) {

            window.location.href =
                "/Admin/Booking/Booking";
        }
    });
}