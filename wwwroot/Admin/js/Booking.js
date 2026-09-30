// Page load: setup and event handlers
$(document).ready(function () {

    loadData();

    // Status filter dropdown change
    $("#ddlStatusFilter, #txtSearch").on("change keyup", function () {
        render();
    });

    // Close payment modal button click
    $("#btnClosePaymentModal").on("click", function () {
        closePaymentModal();
    });

    // Payment modal click
    $("#paymentModal").on("click", function (e) {
        if (e.target === this) {
            closePaymentModal();
        }
    });

    // Close drawer click
    $("#closeDrawer, #btnCancel").on("click", function () {
        $("#drawer").removeClass("active");
    });

    // Save button click
    $("#btnSave").on("click", function () {

        var bookingId = $("#crBookingId").val();
        var roomId = $("#crRoomList").val();

        var ok = validateForm([
            {
                id: "crRoomList",
                label: "Room",
                required: true,
                type: "select"
            }
        ]);

        if (!ok) {
            return;
        }

        var $btn = $(this);

        $btn.prop("disabled", true)
            .text("Sending...");

        // AJAX call to /Admin/Booking/ProposeRoomChange
        $.ajax({
            url: "/Admin/Booking/ProposeRoomChange",
            type: "POST",
            data: {
                bookingId: bookingId,
                proposedRoomId: roomId,
                note: $("#crNote").val()
            },

            success: function (r) {

                if (r.success) {

                    Swal.fire({
                        icon: "success",
                        title: r.message,
                        timer: 1800,
                        showConfirmButton: false
                    });

                    $("#drawer").removeClass("active");

                    loadData();

                } else {

                    Swal.fire({
                        icon: "error",
                        title: r.message
                    });
                }
            },

            error: function () {

                Swal.fire({
                    icon: "error",
                    title: "Unable To Change Room"
                });
            },

            complete: function () {

                $btn.prop("disabled", false)
                    .text("Propose Change");
            }
        });
    });

});


var bookingData = [];


// Load data
function loadData() {

    // AJAX call to /Admin/Booking/GetAll
    $.ajax({
        url: "/Admin/Booking/GetAll",
        type: "GET",

        success: function (res) {

            bookingData = res.data || res;

            if (!Array.isArray(bookingData)) {
                bookingData = [];
            }

            render();
        },

        error: function () {

            $("#tblData tbody").html(`
                <tr>
                    <td colspan="9" style="text-align:center;">
                        Unable to load bookings.
                    </td>
                </tr>
            `);
        }
    });
}


// Render
function render() {

    var statusFilter =
        ($("#ddlStatusFilter").val() || "").toLowerCase();

    var search =
        ($("#txtSearch").val() || "").toLowerCase();

    var data = bookingData.filter(function (item) {

        var status =
            (item.status || "").toLowerCase();

        var text =
            (
                (item.fullName || "") +
                " " +
                (item.email || "") +
                " " +
                (item.phone || "") +
                " " +
                (item.roomTitle || "") +
                " " +
                (item.roomNumber || "")
            ).toLowerCase();

        return (
            (!statusFilter || status === statusFilter) &&
            (!search || text.includes(search))
        );
    });

    var html = "";

    if (!data.length) {

        html = `
            <tr>
                <td colspan="9"
                    style="text-align:center;padding:25px;">
                    No bookings found.
                </td>
            </tr>
        `;

        $("#tblData tbody").html(html);

        return;
    }

    $.each(data, function (_, item) {

        var status =
            (item.status || "").toLowerCase();

        var roomChange =
            (item.roomChangeStatus || "").toLowerCase();

        html += `
            <tr>

                <td>
                    <strong>
                        ${escapeHtml(item.fullName || "")}
                    </strong>

                    <br>

                    <small>
                        ${escapeHtml(item.email || "")}
                        |
                        ${escapeHtml(item.phone || "")}
                    </small>
                </td>

                <td>
                    ${escapeHtml(item.roomTitle || "")}

                    <br>

                    <small>
                        (#${escapeHtml(item.roomNumber || "")})
                    </small>
                </td>

                <td>
                    ${formatDate(item.checkInDate)}
                </td>

                <td>
                    ${formatDate(item.checkOutDate)}
                </td>

                <td>
                    ${item.guests || 0}
                </td>

                <td>
                    ₹${Number(
                        item.totalPrice || 0
                    ).toLocaleString(
                        "en-IN",
                        {
                            minimumFractionDigits: 2,
                            maximumFractionDigits: 2
                        }
                    )}
                </td>

                <td>
                    ${statusBadge(
                        item.status,
                        roomChange,
                        item.emailStatus
                    )}
                </td>

                <td>
                    ${paymentStatusBadge(
                        item.paymentStatus,
                        item.status
                    )}
                </td>

                <td>
                    ${actionButtons(
                        item,
                        status,
                        roomChange
                    )}
                </td>

            </tr>
        `;
    });

    $("#tblData tbody").html(html);
}


// Status badge
function statusBadge(
    status,
    roomChange,
    emailStatus
) {

    var value =
        (status || "Pending").toLowerCase();

    var html = "";

    if (value === "approved") {

        html = `
            <span class="status-badge success">
                <i class="fa-solid fa-circle-check"></i>
                Approved
            </span>
        `;

    } else if (value === "rejected") {

        html = `
            <span class="status-badge danger">
                <i class="fa-solid fa-circle-xmark"></i>
                Rejected
            </span>
        `;

    } else if (value === "cancelled") {

        html = `
            <span class="status-badge danger">
                <i class="fa-solid fa-ban"></i>
                Cancelled
            </span>
        `;

    } else {

        html = `
            <span class="status-badge warning">
                <i class="fa-solid fa-clock"></i>
                Pending
            </span>
        `;
    }

    if (roomChange === "pending") {

        html += `
            <br>

            <span
                class="status-badge warning"
                style="margin-top:5px;">

                ROOM CHANGE PENDING GUEST

            </span>
        `;
    }

    if (
        emailStatus &&
        emailStatus.toLowerCase() === "sent"
    ) {

        html += `
            <br>

            <span
                class="status-badge success"
                style="margin-top:5px;">

                <i class="fa-solid fa-envelope"></i>

                Email Sent

            </span>
        `;
    }

    return html;
}


// Payment status badge
function paymentStatusBadge(
    paymentStatus,
    bookingStatus
) {

    var payment =
        (paymentStatus || "Pending").toLowerCase();

    var booking =
        (bookingStatus || "").toLowerCase();

    if (
        booking === "rejected" ||
        booking === "cancelled"
    ) {

        return `
            <span class="status-badge danger">

                <i class="fa-solid fa-circle-xmark"></i>

                Rejected

            </span>
        `;
    }

    if (
        payment === "paid" ||
        payment === "success" ||
        payment === "successful" ||
        payment === "captured" ||
        payment === "completed"
    ) {

        return `
            <span class="status-badge success">

                <i class="fa-solid fa-circle-check"></i>

                Paid

            </span>
        `;
    }

    if (
        payment === "failed" ||
        payment === "failure"
    ) {

        return `
            <span class="status-badge danger">

                <i class="fa-solid fa-circle-xmark"></i>

                Failed

            </span>
        `;
    }

    if (payment === "refunded") {

        return `
            <span class="status-badge info">

                <i class="fa-solid fa-rotate-left"></i>

                Refunded

            </span>
        `;
    }

    return `
        <span class="status-badge warning">

            <i class="fa-solid fa-clock"></i>

            Pending

        </span>
    `;
}


// Action buttons
function actionButtons(
    item,
    status,
    roomChange
) {

    var id = item.bookingId;

    var payment =
        (item.paymentStatus || "").toLowerCase();

    var buttons = `
        <button
            type="button"
            title="View Booking"
            onclick="viewBooking(${id})">

            <i class="fa-solid fa-eye"></i>

        </button>
    `;


    if (status === "pending") {

        if (roomChange === "pending") {

            buttons += `
                <button
                    type="button"
                    disabled
                    title="User has not accepted or rejected the room change request."
                    style="
                        opacity:.45;
                        cursor:not-allowed;
                    ">

                    <i class="fa-solid fa-check"></i>

                </button>
            `;

        } else {

            buttons += `
                <button
                    type="button"
                    title="Approve Booking"
                    onclick="updateStatus(
                        ${id},
                        'Approved'
                    )">

                    <i class="fa-solid fa-check"></i>

                </button>
            `;
        }


        buttons += `
            <button
                type="button"
                title="Reject Booking"
                onclick="updateStatus(
                    ${id},
                    'Rejected'
                )">

                <i class="fa-solid fa-xmark"></i>

            </button>
        `;


        if (roomChange !== "pending") {

            buttons +=
                roomChangeButton(item);
        }


        buttons += `
            <button
                type="button"
                title="Delete Booking"
                onclick="deleteRecord(${id})">

                <i class="fa-solid fa-trash"></i>

            </button>
        `;

        return buttons;
    }


    if (status === "approved") {

        buttons += `
            <button
                type="button"
                title="Payment Details"
                onclick="viewPayment(${id})">

                <i class="fa-solid fa-credit-card"></i>

            </button>
        `;


        if (payment === "paid") {

            buttons += `
                <button
                    type="button"
                    disabled
                    title="Room change is not allowed after payment is completed."
                    style="
                        opacity:.4;
                        cursor:not-allowed;
                    ">

                    <i class="fa-solid fa-bed"></i>

                </button>
            `;

        } else {

            buttons +=
                roomChangeButton(item);
        }

        return buttons;
    }


    buttons += `
        <button
            type="button"
            title="Delete Booking"
            onclick="deleteRecord(${id})">

            <i class="fa-solid fa-trash"></i>

        </button>
    `;

    return buttons;
}


// Room change button
function roomChangeButton(item) {

    var room =
        (item.roomTitle || "") +
        " (#" +
        (item.roomNumber || "") +
        ")";

    return `
        <button
            type="button"
            title="Change Room"
            onclick="openChangeRoom(
                ${item.bookingId},
                '${escapeAttribute(room)}'
            )">

            <i class="fa-solid fa-bed"></i>

        </button>
    `;
}


// View booking
function viewBooking(bookingId) {

    var booking = null;

    for (
        var i = 0;
        i < bookingData.length;
        i++
    ) {

        if (
            Number(
                bookingData[i].bookingId
            ) === Number(bookingId)
        ) {

            booking =
                bookingData[i];

            break;
        }
    }


    if (!booking) {

        Swal.fire({
            icon: "error",
            title: "Booking Not Found",
            text: "Booking details could not be found."
        });

        return;
    }


    var room =
        (booking.roomTitle || "Hotel Room") +
        (
            booking.roomNumber
                ? " (#" +
                  booking.roomNumber +
                  ")"
                : ""
        );


    var paymentStatus =
        booking.paymentStatus ||
        "Pending";


    Swal.fire({

        title: "Booking Details",

        width: "680px",

        html: `

            <div style="
                text-align:left;
                padding:5px 10px;
            ">

                <div style="
                    display:grid;
                    grid-template-columns:
                        1fr 1fr;
                    gap:14px;
                    margin-bottom:15px;
                ">

                    <div>

                        <strong>
                            Booking ID
                        </strong>

                        <br>

                        #${escapeHtml(
                            String(
                                booking.bookingId ||
                                ""
                            )
                        )}

                    </div>


                    <div>

                        <strong>
                            Guest Name
                        </strong>

                        <br>

                        ${escapeHtml(
                            booking.fullName ||
                            ""
                        )}

                    </div>


                    <div>

                        <strong>
                            Email
                        </strong>

                        <br>

                        ${escapeHtml(
                            booking.email ||
                            ""
                        )}

                    </div>


                    <div>

                        <strong>
                            Phone
                        </strong>

                        <br>

                        ${escapeHtml(
                            booking.phone ||
                            ""
                        )}

                    </div>


                    <div>

                        <strong>
                            Room
                        </strong>

                        <br>

                        ${escapeHtml(room)}

                    </div>


                    <div>

                        <strong>
                            Guests
                        </strong>

                        <br>

                        ${escapeHtml(
                            String(
                                booking.guests ||
                                0
                            )
                        )}

                    </div>


                    <div>

                        <strong>
                            Check-in
                        </strong>

                        <br>

                        ${formatDate(
                            booking.checkInDate
                        )}

                    </div>


                    <div>

                        <strong>
                            Check-out
                        </strong>

                        <br>

                        ${formatDate(
                            booking.checkOutDate
                        )}

                    </div>


                    <div>

                        <strong>
                            Total Amount
                        </strong>

                        <br>

                        ₹${Number(
                            booking.totalPrice ||
                            0
                        ).toLocaleString(
                            "en-IN",
                            {
                                minimumFractionDigits:
                                    2,

                                maximumFractionDigits:
                                    2
                            }
                        )}

                    </div>


                    <div>

                        <strong>
                            Payment
                        </strong>

                        <br>

                        ${escapeHtml(
                            paymentStatus
                        )}

                    </div>


                    <div>

                        <strong>
                            Booking Status
                        </strong>

                        <br>

                        ${escapeHtml(
                            booking.status ||
                            "Pending"
                        )}

                    </div>

                </div>


                ${
                    booking.specialRequest
                        ? `

                            <div style="
                                margin-top:15px;
                                padding:12px;
                                background:#f7f7f7;
                                border-radius:8px;
                            ">

                                <strong>
                                    Special Request
                                </strong>

                                <p style="
                                    margin:6px 0 0;
                                    color:#555;
                                ">

                                    ${escapeHtml(
                                        booking.specialRequest
                                    )}

                                </p>

                            </div>

                        `
                        : ""
                }

            </div>

        `,

        icon: "info",

        confirmButtonText: "Close"

    });
}


// View payment
function viewPayment(bookingId) {

    $("#paymentDetailsContent").html(`
        <div style="
            padding:30px;
            text-align:center;
        ">
            Loading payment details...
        </div>
    `);

    $("#paymentModal").addClass("active");

    // AJAX call to /Admin/Booking/GetPaymentDetails
    $.ajax({

        url: "/Admin/Booking/GetPaymentDetails",

        type: "GET",

        data: {
            bookingId: bookingId
        },

        dataType: "json",

        success: function (res) {

            console.log("Payment Response:", res);

            if (
                !res ||
                res.success !== true ||
                !res.data
            ) {

                $("#paymentDetailsContent").html(`
                    <div style="
                        padding:30px;
                        text-align:center;
                        color:#dc2626;
                    ">
                        ${
                            res && res.message
                                ? escapeHtml(res.message)
                                : "Payment details not found."
                        }
                    </div>
                `);

                return;
            }

            var p = res.data;

            var status =
                (p.paymentStatus || "Pending")
                    .toLowerCase();

            var statusColor = "#856404";
            var statusBg = "#fff3cd";

            if (status === "paid") {

                statusColor = "#198754";
                statusBg = "#e8f7ee";

            }
            else if (
                status === "failed" ||
                status === "failure" ||
                status === "rejected"
            ) {

                statusColor = "#dc2626";
                statusBg = "#fde8e8";

            }
            else if (status === "refunded") {

                statusColor = "#2563eb";
                statusBg = "#eaf2ff";
            }

            var paymentDate =
                p.paymentDate
                    ? new Date(
                        p.paymentDate
                    ).toLocaleString("en-IN")
                    : "Not Paid";

            var createdDate =
                p.createdDate
                    ? new Date(
                        p.createdDate
                    ).toLocaleString("en-IN")
                    : "Not Available";

            $("#paymentDetailsContent").html(`

                <div class="payment-detail-row">

                    <span>
                        Payment ID
                    </span>

                    <strong>
                        #${escapeHtml(
                            String(
                                p.paymentId || ""
                            )
                        )}
                    </strong>

                </div>


                <div class="payment-detail-row">

                    <span>
                        Booking ID
                    </span>

                    <strong>
                        #${escapeHtml(
                            String(
                                p.bookingId ||
                                bookingId
                            )
                        )}
                    </strong>

                </div>


                <div class="payment-detail-row">

                    <span>
                        Amount
                    </span>

                    <strong>
                        ₹${Number(
                            p.amount || 0
                        ).toLocaleString(
                            "en-IN",
                            {
                                minimumFractionDigits: 2,
                                maximumFractionDigits: 2
                            }
                        )}
                    </strong>

                </div>


                <div class="payment-detail-row">

                    <span>
                        Status
                    </span>

                    <strong style="
                        color:${statusColor};
                        background:${statusBg};
                        padding:6px 12px;
                        border-radius:15px;
                    ">

                        ${escapeHtml(
                            p.paymentStatus ||
                            "Pending"
                        )}

                    </strong>

                </div>


                <div class="payment-detail-row">

                    <span>
                        Order ID
                    </span>

                    <strong>
                        ${escapeHtml(
                            p.orderId ||
                            "Not Generated"
                        )}
                    </strong>

                </div>


                <div class="payment-detail-row">

                    <span>
                        Gateway Payment ID
                    </span>

                    <strong>
                        ${escapeHtml(
                            p.gatewayPaymentId ||
                            "Not Available"
                        )}
                    </strong>

                </div>


                <div class="payment-detail-row">

                    <span>
                        Payment Date
                    </span>

                    <strong>
                        ${escapeHtml(
                            paymentDate
                        )}
                    </strong>

                </div>


                <div class="payment-detail-row">

                    <span>
                        Created Date
                    </span>

                    <strong>
                        ${escapeHtml(
                            createdDate
                        )}
                    </strong>

                </div>


                ${
                    p.failureReason
                        ? `
                            <div style="
                                margin-top:15px;
                                padding:12px;
                                background:#fde8e8;
                                color:#dc2626;
                                border-radius:8px;
                            ">

                                <strong>
                                    Failure Reason:
                                </strong>

                                <div style="
                                    margin-top:5px;
                                ">

                                    ${escapeHtml(
                                        p.failureReason
                                    )}

                                </div>

                            </div>
                        `
                        : ""
                }

            `);
        },

        error: function (xhr) {

            console.log(
                "GetPaymentDetails Error:",
                xhr.status
            );

            console.log(
                xhr.responseText
            );

            $("#paymentDetailsContent").html(`
                <div style="
                    padding:30px;
                    text-align:center;
                    color:#dc2626;
                ">
                    Unable to load payment details.
                </div>
            `);
        }
    });
}

// Update status
function updateStatus(bookingId, status) {

    var title =
        status === "Approved"
            ? "Approve Booking?"
            : "Reject Booking?";

    var text =
        status === "Approved"
            ? "Are you sure you want to approve this booking?"
            : "Are you sure you want to reject this booking?";

    var confirmText =
        status === "Approved"
            ? "Yes, Approve"
            : "Yes, Reject";

    Swal.fire({

        title: title,

        text: text,

        icon: "warning",

        showCancelButton: true,

        confirmButtonText: confirmText,

        cancelButtonText: "Cancel",

        reverseButtons: true

    }).then(function (result) {

        if (!result.isConfirmed) {
            return;
        }

        // AJAX call to /Admin/Booking/UpdateStatus
        $.ajax({

            url: "/Admin/Booking/UpdateStatus",

            type: "POST",

            data: {
                bookingId: bookingId,
                status: status
            },

            beforeSend: function () {

                Swal.fire({

                    title:
                        status === "Approved"
                            ? "Approving..."
                            : "Rejecting...",

                    text: "Please wait.",

                    allowOutsideClick: false,

                    allowEscapeKey: false,

                    didOpen: function () {
                        Swal.showLoading();
                    }

                });
            },

            success: function (res) {

                if (res && res.success) {

                    Swal.fire({

                        icon: "success",

                        title:
                            status === "Approved"
                                ? "Booking Approved"
                                : "Booking Rejected",

                        text:
                            res.message ||
                            "Booking status updated successfully.",

                        timer: 1800,

                        showConfirmButton: false

                    });

                    loadData();

                }
                else {

                    Swal.fire({

                        icon: "error",

                        title: "Unable To Update Booking",

                        text:
                            res && res.message
                                ? res.message
                                : "Something went wrong."

                    });
                }
            },

            error: function (xhr) {

                console.log(
                    "Update Status Error:",
                    xhr.status
                );

                console.log(
                    xhr.responseText
                );

                Swal.fire({

                    icon: "error",

                    title: "Server Error",

                    text:
                        "Unable to update booking status."

                });
            }
        });
    });
}


// Close payment modal
function closePaymentModal() {

    $("#paymentModal")
        .removeClass("active");

    $("#paymentDetailsContent")
        .html("");
}


// Open change room
function openChangeRoom(
    id,
    currentRoom
) {

    $("#crBookingId")
        .val(id);


    $("#crCurrentRoom")
        .html(
            "<strong>Current Room:</strong> " +
            escapeHtml(currentRoom)
        );


    $("#crRoomList")
        .html(
            "<option value=''>Loading rooms...</option>"
        );


    $("#crNoRooms")
        .hide();


    $("#drawer")
        .addClass("active");


    // AJAX call to /Admin/Booking/GetAlternativeRooms
    $.ajax({

        url:
            "/Admin/Booking/GetAlternativeRooms",

        type:
            "GET",

        data: {
            bookingId:
                id
        },


        success: function (res) {

            var rooms =
                res.data || res;


            var html =
                "<option value=''>Select Room</option>";


            if (
                !rooms ||
                !rooms.length
            ) {

                $("#crRoomList")
                    .html(
                        "<option value=''>No rooms available</option>"
                    );


                $("#crNoRooms")
                    .show();


                return;
            }


            $.each(
                rooms,
                function (_, room) {

                    html += `

                        <option
                            value="${room.roomId}">

                            ${escapeHtml(
                                room.title
                            )}

                            (#${escapeHtml(
                                room.roomNumber
                            )})

                        </option>

                    `;
                }
            );


            $("#crRoomList")
                .html(html);
        },


        error: function () {

            $("#crRoomList")
                .html(
                    "<option value=''>Unable to load rooms</option>"
                );
        }

    });
}


// Format date
function formatDate(value) {

    if (!value) {
        return "";
    }


    var date =
        new Date(value);


    if (
        isNaN(
            date.getTime()
        )
    ) {

        return value;
    }


    return date.toLocaleDateString(
        "en-GB"
    );
}


// Escape html
function escapeHtml(value) {

    return $("<div>")
        .text(value || "")
        .html();
}


// Escape attribute
function escapeAttribute(value) {

    return String(
        value || ""
    )
        .replace(
            /\\/g,
            "\\\\"
        )
        .replace(
            /'/g,
            "\\'"
        );
}