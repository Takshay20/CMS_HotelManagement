$(function () {
    if ($(".drawer-backdrop").length === 0) {
        $("body").append('<div class="drawer-backdrop"></div>');
    }
    var $backdrop = $(".drawer-backdrop");
    var $drawer = $("#drawer");

    if ($drawer.length === 0) return;

    var observer = new MutationObserver(function () {
        if ($drawer.hasClass("active")) {
            $backdrop.addClass("active");
        } else {
            $backdrop.removeClass("active");
            $drawer.find(".invalid").removeClass("invalid");
            $drawer.find(".field-error").removeClass("show").text("");
        }
    });
    observer.observe($drawer.get(0), { attributes: true, attributeFilter: ["class"] });

    $backdrop.on("click", function () {
        $drawer.removeClass("active");
    });

    $(document).on("keydown", function (e) {
        if (e.key === "Escape" && $drawer.hasClass("active")) {
            $drawer.removeClass("active");
        }
    });
});

function validateForm(fields) {
    var firstInvalid = null;
    var anyInvalid = false;

    fields.forEach(function (f) {
        var $el = $("#" + f.id);
        var $err = $("#err-" + f.id);
        var value = f.type === "file" ? null : $.trim($el.val() || "");
        var isInvalid = false;

        if (f.onlyForNew && !f.isNew) {
            $el.removeClass("invalid");
            if ($err.length) $err.removeClass("show").text("");
            return;
        }

        if (f.type === "file") {
            var hasFile = $el.length && $el[0].files && $el[0].files.length > 0;
            if (f.required && !hasFile) isInvalid = true;
        } else if (f.type === "email") {
            var emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            if (f.required && value === "") isInvalid = true;
            else if (value !== "" && !emailPattern.test(value)) isInvalid = true;
        } else if (f.type === "select") {
            if (f.required && (value === "" || value === null)) isInvalid = true;
        } else {
            if (f.required && value === "") isInvalid = true;
        }

        if (isInvalid) {
            anyInvalid = true;
            $el.addClass("invalid");
            if ($err.length) $err.addClass("show").text((f.label || "This field") + " is required.");
            if (!firstInvalid) firstInvalid = $el;
        } else {
            $el.removeClass("invalid");
            if ($err.length) $err.removeClass("show").text("");
        }
    });

    if (anyInvalid) {
        if (firstInvalid) {
            firstInvalid.focus();
            var container = firstInvalid.closest(".drawer-body, .table-container");
            if (container && container.length) {
                container.animate({ scrollTop: firstInvalid.position().top - 20 }, 300);
            }
        }
        Swal.fire({
            icon: "warning",
            title: "Please fill in all required fields",
            text: "The highlighted field(s) below need your attention.",
            confirmButtonColor: "#D4AF37"
        });
        return false;
    }

    return true;
}

$(document).on("input change", "#frmData input, #frmData select, #frmData textarea", function () {
    $(this).removeClass("invalid");
    $("#err-" + this.id).removeClass("show").text("");
});

function showDetails(title, rows) {
    var html = "<div class='text-left'>";
    rows.forEach(function (r) {
        if (r.value === null || r.value === undefined || r.value === "") return;
        if (r.isImage) {
            html += "<div class='details-row-img'><strong class='details-label'>" + r.label + "</strong>";
            html += "<img src='/" + r.value + "' class='details-img'/></div>";
        } else {
            html += "<div class='details-row'><strong class='details-label'>" + r.label + "</strong>";
            html += "<span class='details-value'>" + r.value + "</span></div>";
        }
    });
    html += "</div>";

    Swal.fire({
        title: title,
        html: html,
        width: 500,
        confirmButtonText: "Close",
        confirmButtonColor: "#D4AF37"
    });
}
