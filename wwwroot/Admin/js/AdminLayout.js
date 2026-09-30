// Page load: setup and event handlers
$(document).ready(function () {
    $("#btnMenu").click(function () {
        $(".sidebar").toggleClass("collapse");
    });

    // Sidebar submenu toggle click
    $(".has-sub > a.sub-toggle").on("click", function (e) {
        e.preventDefault();
        var $parent = $(this).closest(".has-sub");
        var wasOpen = $parent.hasClass("open");
        $(".has-sub").not($parent).removeClass("open");
        $parent.toggleClass("open", !wasOpen);
    });
    var currentPath = window.location.pathname.toLowerCase();
    $(".sidebar ul.menu a").not(".sub-toggle").each(function () {
        var href = ($(this).attr("href") || "").toLowerCase();
        if (href && currentPath.indexOf(href) === 0) {
            $(this).addClass("active-link");
            $(this).closest(".has-sub").addClass("open");
        }
    });
    var backdrop = $("#drawerBackdrop");

    // Sync backdrop
    function syncBackdrop() {
        if ($(".drawer.active").length > 0) {
            backdrop.addClass("active");
        } else {
            backdrop.removeClass("active");
        }
    }
    if (window.MutationObserver) {
        document.querySelectorAll(".drawer").forEach(function (drawerEl) {
            var observer = new MutationObserver(syncBackdrop);
            observer.observe(drawerEl, { attributes: true, attributeFilter: ["class"] });
        });
    }
    backdrop.on("click", function () {
        $(".drawer.active").removeClass("active");
        backdrop.removeClass("active");
    });
});

// Page load: setup and event handlers
$(function () {
    var editId = new URLSearchParams(window.location.search).get("editId");
    if (editId && typeof edit === "function") setTimeout(function () { edit(editId); }, 400);
});
