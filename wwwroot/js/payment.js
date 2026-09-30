document.addEventListener("DOMContentLoaded", function () {

    var payButton = document.getElementById("payButton");

    if (!payButton) {
        return;
    }

    payButton.addEventListener("click", function () {

        var key = payButton.dataset.key;
        var orderId = payButton.dataset.orderId;
        var amount = payButton.dataset.amount;

        console.log("Razorpay Key:", key);
        console.log("Razorpay Order ID:", orderId);
        console.log("Razorpay Amount:", amount);

        if (!key) {
            alert("Razorpay Key is missing.");
            return;
        }

        if (!orderId) {
            alert("Razorpay Order ID is missing.");
            return;
        }

        if (!amount || Number(amount) <= 0) {
            alert("Invalid payment amount.");
            return;
        }

        payButton.disabled = true;
        payButton.innerText = "Opening Payment...";

        var options = {

            key: key,

            amount: Number(amount),

            currency: "INR",

            name: "Royal Paradise Hotel",

            description: "Hotel Booking Payment",

            order_id: orderId,

            handler: function (response) {

                console.log(
                    "Payment Success:",
                    response
                );

                // AJAX call to /Payment/VerifyPayment
                $.ajax({

                    url: "/Payment/VerifyPayment",

                    type: "POST",

                    data: {

                        razorpay_payment_id:
                            response.razorpay_payment_id,

                        razorpay_order_id:
                            response.razorpay_order_id,

                        razorpay_signature:
                            response.razorpay_signature
                    },

                    success: function (res) {

                        console.log(
                            "Server Verification:",
                            res
                        );

                        if (res.success) {

                            document.getElementById(
                                "paymentMessage"
                            ).innerHTML =
                                "<span class='text-success'>" +
                                res.message +
                                "</span>";

                            setTimeout(function () {

                                if (res.redirectUrl) {

                                    window.location.href =
                                        res.redirectUrl;

                                } else {

                                    window.location.href =
                                        "/Booking/MyBookings";
                                }

                            }, 1000);

                        } else {

                            document.getElementById(
                                "paymentMessage"
                            ).innerHTML =
                                "<span class='text-danger'>" +
                                res.message +
                                "</span>";

                            payButton.disabled = false;

                            payButton.innerText =
                                "Pay ₹" +
                                (Number(amount) / 100)
                                    .toFixed(2);
                        }
                    },

                    error: function (xhr) {

                        console.error(
                            "Server Error:",
                            xhr.responseText
                        );

                        document.getElementById(
                            "paymentMessage"
                        ).innerHTML =
                            "<span class='text-danger'>" +
                            "Payment verification request failed." +
                            "</span>";

                        payButton.disabled = false;

                        payButton.innerText =
                            "Pay ₹" +
                            (Number(amount) / 100)
                                .toFixed(2);
                    }
                });
            },

            modal: {

                ondismiss: function () {

                    console.log(
                        "Payment window closed."
                    );

                    payButton.disabled = false;

                    payButton.innerText =
                        "Pay ₹" +
                        (Number(amount) / 100)
                            .toFixed(2);
                }
            },

            prefill: {
                name: "",
                email: "",
                contact: ""
            },

            theme: {
                color: "#111827"
            }
        };

        var razorpay =
            new Razorpay(options);

        razorpay.on(
            "payment.failed",
            function (response) {

                console.error(
                    "========== RAZORPAY ERROR =========="
                );

                console.error(
                    "Code:",
                    response.error.code
                );

                console.error(
                    "Description:",
                    response.error.description
                );

                console.error(
                    "Source:",
                    response.error.source
                );

                console.error(
                    "Step:",
                    response.error.step
                );

                console.error(
                    "Reason:",
                    response.error.reason
                );

                console.error(
                    "Metadata:",
                    response.error.metadata
                );

                console.error(
                    "===================================="
                );

                document.getElementById(
                    "paymentMessage"
                ).innerHTML =
                    "<span class='text-danger'>" +
                    response.error.description +
                    "</span>";

                payButton.disabled = false;

                payButton.innerText =
                    "Pay ₹" +
                    (Number(amount) / 100)
                        .toFixed(2);
            }
        );

        razorpay.open();
    });
});