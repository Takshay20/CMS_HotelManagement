$(document).ready(function () {
    // Common Part
    var $moduleSelect = $("#moduleSelect");
    var $btnModuleSelect = $("#btnModuleSelect");
    var $moduleDropdown = $("#moduleDropdown");
    var $search = $("#txtModuleSearch");
    window.currentRestoreFilter = "All";
    $btnModuleSelect.on("click", function (e) {
        e.stopPropagation();
        $moduleDropdown.toggleClass("show");
        $btnModuleSelect.toggleClass("active", $moduleDropdown.hasClass("show"));
        if ($moduleDropdown.hasClass("show")) {
            $search.val("");
            $(".module-option").show();
            setTimeout(function () {
                $search.focus();
            }, 100);
        }
    });
    $(".module-option").on("click", function (e) {
        e.stopPropagation();
        var value = $(this).data("value");
        var name = $(this).data("name");
        var icon = $(this).find("i").attr("class");
        $("#selectedModule").html("<i class='" + icon + "'></i> " + name);
        $(".module-option").removeClass("selected");
        $(this).addClass("selected");
        $moduleDropdown.removeClass("show");
        $btnModuleSelect.removeClass("active");
        window.currentRestoreFilter = "All";
        loadModule(value);
    });
    $search.on("keyup", function () {
        var searchText = $(this).val().toLowerCase().trim();
        $(".module-option").each(function () {
            var name = $(this).data("name").toLowerCase();
            if (name.indexOf(searchText) !== -1) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    });
    $(document).on("click", function (e) {
        if (!$(e.target).closest("#moduleSelect").length) {
            $moduleDropdown.removeClass("show");
            $btnModuleSelect.removeClass("active");
        }
    });
    function loadModule(module) {
        if (module === "WhyChooseUs") {
            window.currentRestoreFilter = "All";
            loadWhyChooseUs("All");
            return;
        }
        if (module === "CounterBox") {
            window.currentRestoreFilter = "All";
            loadCounterBox("All");
            return;
        }
        $("#recordContainer").html("<div class='table-container'><p style='padding:20px;'>Selected Module: " + module + "</p></div>");
    }
    function showRestoreRestrictionTimer(deletedDate) {
        var deletedTime = new Date(deletedDate).getTime();
        var restoreTime = deletedTime + (60 * 60 * 1000);
        var timerInterval;
        function getRemainingTime() {
            var remaining = restoreTime - new Date().getTime();
            if (remaining <= 0) {
                return "00:00:00";
            }
            var totalSeconds = Math.floor(remaining / 1000);
            var hours = Math.floor(totalSeconds / 3600);
            var minutes = Math.floor((totalSeconds % 3600) / 60);
            var seconds = totalSeconds % 60;
            return String(hours).padStart(2, "0") + ":" + String(minutes).padStart(2, "0") + ":" + String(seconds).padStart(2, "0");
        }
        Swal.fire({
            icon: "error",
            title: "Cannot Restore",
            html: "<div style='font-size:16px;'>This record can be restored only after 1 hour.</div><div style='margin-top:18px;font-size:15px;font-weight:600;'>Restore available in</div><div id='restoreTimer' style='margin-top:8px;font-size:30px;font-weight:700;color:#b8860b;'>" + getRemainingTime() + "</div>",
            confirmButtonText: "OK",
            allowOutsideClick: false,
            didOpen: function () {
                var timerElement = document.getElementById("restoreTimer");
                timerInterval = setInterval(function () {
                    var remaining = restoreTime - new Date().getTime();
                    if (remaining <= 0) {
                        clearInterval(timerInterval);
                        timerElement.innerHTML = "<span style='color:green;'>00:00:00</span>";
                        return;
                    }
                    timerElement.textContent = getRemainingTime();
                }, 1000);
            },
            willClose: function () {
                if (timerInterval) {
                    clearInterval(timerInterval);
                }
            }
        });
    }
    // Why Choose Us
    function loadWhyChooseUs(filter) {
        window.currentRestoreFilter = filter;
        $.ajax({
            url: "/Admin/RestoreRecords/GetWhyChooseUs",
            type: "GET",
            cache: false,
            data: { filter: filter },
            success: function (response) {
                if (response.success) {
                    bindWhyChooseUs(response.data);
                } else {
                    $("#recordContainer").html("<div class='table-container'><p style='padding:20px;'>Unable to load records.</p></div>");
                }
            },
            error: function () {
                $("#recordContainer").html("<div class='table-container'><p style='padding:20px;'>Unable to load records.</p></div>");
            }
        });
    }
    function bindWhyChooseUs(data) {
        var html = "";
        html += "<div class='restore-toolbar'>";
        html += "<div class='restore-search'><i class='fa-solid fa-magnifying-glass'></i><input type='text' id='txtRecordSearch' placeholder='Search Here...' /></div>";
        html += "<select id='ddlRecordFilter' class='record-filter'><option value='All'>All</option><option value='Active'>Active</option><option value='Inactive'>Inactive</option><option value='Deleted'>Deleted</option></select>";
        html += "</div>";
        html += "<div class='table-container'>";
        html += "<table id='tblRestoreRecords'><thead><tr><th>Icon</th><th>Title</th><th>Description</th><th>Display Order</th><th>Status</th><th>Deleted</th><th>Action</th></tr></thead>";
        html += "<tbody>";
        if (!data || data.length === 0) {
            html += "<tr><td colspan='7' style='text-align:center;'>No records found.</td></tr>";
        } else {
            $.each(data, function (index, item) {
                var status = "";
                if (item.isDeleted) {
                    status = "<span class='status-badge status-deleted'>Deleted</span>";
                } else if (item.isActive) {
                    status = "<span class='status-badge status-active'>Active</span>";
                } else {
                    status = "<span class='status-badge status-inactive'>Inactive</span>";
                }
                html += "<tr>";
                html += "<td class='tableIcon'><i class='" + (item.iconClass || "") + "'></i></td>";
                html += "<td>" + (item.title || "") + "</td>";
                html += "<td>" + (item.description || "") + "</td>";
                html += "<td>" + item.displayOrder + "</td>";
                html += "<td>" + status + "</td>";
                html += "<td>" + (item.isDeleted ? "Yes" : "No") + "</td>";
                html += "<td>";
                if (item.isDeleted) {
                    html += "<button type='button' class='action-btn action-restore' title='Restore' onclick='restoreRecord(" + item.homeWhyChooseUsId + ")'><i class='fa-solid fa-rotate-left'></i></button>";
                } else if (!item.isActive) {
                    html += "<button type='button' class='action-btn action-edit' title='Edit' onclick='editRecord(" + item.homeWhyChooseUsId + ")'><i class='fa-solid fa-pen'></i></button> ";
                    html += "<button type='button' class='action-btn action-delete' title='Delete' onclick='deleteRecord(" + item.homeWhyChooseUsId + ")'><i class='fa-solid fa-trash'></i></button>";
                } else {
                    html += "<button type='button' class='action-btn action-disabled' title='Active record' disabled><i class='fa-solid fa-lock'></i></button>";
                }
                html += "</td>";
                html += "</tr>";
            });
        }
        html += "</tbody></table></div>";
        $("#recordContainer").html(html);
        $("#ddlRecordFilter").val(window.currentRestoreFilter || "All");
        $("#ddlRecordFilter").on("change", function () {
            window.currentRestoreFilter = $(this).val();
            loadWhyChooseUs(window.currentRestoreFilter);
        });
        $("#txtRecordSearch").on("keyup", function () {
            var value = $(this).val().toLowerCase();
            $("#tblRestoreRecords tbody tr").each(function () {
                $(this).toggle($(this).text().toLowerCase().indexOf(value) !== -1);
            });
        });
    }
    // About Counter
    function loadCounterBox(filter) {
        window.currentRestoreFilter = filter;
        $.ajax({
            url: "/Admin/RestoreRecords/GetCounterBox",
            type: "GET",
            cache: false,
            data: { filter: filter },
            success: function (response) {
                if (response.success) {
                    bindCounterBox(response.data);
                } else {
                    $("#recordContainer").html("<div class='table-container'><p style='padding:20px;'>Unable to load records.</p></div>");
                }
            },
            error: function () {
                $("#recordContainer").html("<div class='table-container'><p style='padding:20px;'>Unable to load records.</p></div>");
            }
        });
    }
    function bindCounterBox(data) {
        var html = "";
        html += "<div class='restore-toolbar'>";
        html += "<div class='restore-search'><i class='fa-solid fa-magnifying-glass'></i><input type='text' id='txtRecordSearch' placeholder='Search Here...' /></div>";
        html += "<select id='ddlRecordFilter' class='record-filter'><option value='All'>All</option><option value='Active'>Active</option><option value='Inactive'>Inactive</option><option value='Deleted'>Deleted</option></select>";
        html += "</div>";
        html += "<div class='table-container'>";
        html += "<table id='tblRestoreRecords'><thead><tr><th>Icon</th><th>Number</th><th>Suffix</th><th>Label</th><th>Display Order</th><th>Status</th><th>Deleted</th><th>Action</th></tr></thead>";
        html += "<tbody>";
        if (!data || data.length === 0) {
            html += "<tr><td colspan='8' style='text-align:center;'>No records found.</td></tr>";
        } else {
            $.each(data, function (index, item) {
                var status = "";
                if (item.isDeleted) {
                    status = "<span class='status-badge status-deleted'>Deleted</span>";
                } else if (item.isActive) {
                    status = "<span class='status-badge status-active'>Active</span>";
                } else {
                    status = "<span class='status-badge status-inactive'>Inactive</span>";
                }
                html += "<tr>";
                html += "<td class='tableIcon'><i class='" + (item.iconClass || "") + "'></i></td>";
                html += "<td>" + (item.number || "") + "</td>";
                html += "<td>" + (item.suffix || "") + "</td>";
                html += "<td>" + (item.label || "") + "</td>";
                html += "<td>" + item.displayOrder + "</td>";
                html += "<td>" + status + "</td>";
                html += "<td>" + (item.isDeleted ? "Yes" : "No") + "</td>";
                html += "<td>";
                if (item.isDeleted) {
                    html += "<button type='button' class='action-btn action-restore' title='Restore' onclick='restoreCounterBox(" + item.aboutCounterId + ")'><i class='fa-solid fa-rotate-left'></i></button>";
                } else if (!item.isActive) {
                    html += "<button type='button' class='action-btn action-edit' title='Edit' onclick='editCounterBox(" + item.aboutCounterId + ")'><i class='fa-solid fa-pen'></i></button> ";
                    html += "<button type='button' class='action-btn action-delete' title='Delete' onclick='deleteCounterBox(" + item.aboutCounterId + ")'><i class='fa-solid fa-trash'></i></button>";
                } else {
                    html += "<button type='button' class='action-btn action-disabled' title='Active record' disabled><i class='fa-solid fa-lock'></i></button>";
                }
                html += "</td>";
                html += "</tr>";
            });
        }
        html += "</tbody></table></div>";
        $("#recordContainer").html(html);
        $("#ddlRecordFilter").val(window.currentRestoreFilter || "All");
        $("#ddlRecordFilter").on("change", function () {
            window.currentRestoreFilter = $(this).val();
            loadCounterBox(window.currentRestoreFilter);
        });
        $("#txtRecordSearch").on("keyup", function () {
            var value = $(this).val().toLowerCase();
            $("#tblRestoreRecords tbody tr").each(function () {
                $(this).toggle($(this).text().toLowerCase().indexOf(value) !== -1);
            });
        });
    }
});
function editRecord(id) {
    window.location.href = "/Admin/HomeWhyChooseUs/HomeWhyChooseUs?editId=" + id;
}
function deleteRecord(id) {
    Swal.fire({
        title: "Delete Record?",
        text: "This record will be moved to deleted records.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes, Delete",
        cancelButtonText: "Cancel"
    }).then(function (result) {
        if (!result.isConfirmed) {
            return;
        }
        $.ajax({
            url: "/Admin/HomeWhyChooseUs/Delete",
            type: "POST",
            cache: false,
            data: { id: id },
            success: function (response) {
                if (response.success) {
                    Swal.fire({
                        icon: "success",
                        title: "Deleted",
                        text: response.message
                    }).then(function () {
                        loadWhyChooseUs(window.currentRestoreFilter || "All");
                    });
                } else {
                    Swal.fire({
                        icon: "error",
                        title: "Error",
                        text: response.message
                    });
                }
            },
            error: function () {
                Swal.fire({
                    icon: "error",
                    title: "Error",
                    text: "Unable to delete record."
                });
            }
        });
    });
}
function restoreRecord(id) {
    var wrapper = $(".restore-timer-wrapper[data-id='" + id + "']");
    var deletedDate = wrapper.attr("data-deleted-date");
    if (!deletedDate) {
        Swal.fire({
            icon: "error",
            title: "Cannot Restore",
            text: "Deleted date not found."
        });
        return;
    }
    var deletedTime = new Date(deletedDate).getTime();
    var restoreTime = deletedTime + (60 * 60 * 1000);
    var currentTime = new Date().getTime();
    if (restoreTime > currentTime) {
        showRestoreRestrictionTimer(deletedDate);
        return;
    }
    Swal.fire({
        title: "Restore Record?",
        text: "This record will be restored.",
        icon: "question",
        showCancelButton: true,
        confirmButtonText: "Yes, Restore",
        cancelButtonText: "Cancel"
    }).then(function (result) {
        if (!result.isConfirmed) {
            return;
        }
        $.ajax({
            url: "/Admin/HomeWhyChooseUs/Restore",
            type: "POST",
            cache: false,
            data: { id: id },
            success: function (response) {
                if (response.success) {
                    Swal.fire({
                        icon: "success",
                        title: "Restored",
                        text: response.message
                    }).then(function () {
                        loadWhyChooseUs(window.currentRestoreFilter || "All");
                    });
                } else {
                    Swal.fire({
                        icon: "error",
                        title: "Cannot Restore",
                        text: response.message
                    });
                }
            },
            error: function () {
                Swal.fire({
                    icon: "error",
                    title: "Error",
                    text: "Unable to restore record."
                });
            }
        });
    });
}
function editCounterBox(id) {
    window.location.href = "/Admin/AboutCounter/AboutCounter?editId=" + id;
}
function deleteCounterBox(id) {
    Swal.fire({
        title: "Delete Record?",
        text: "This record will be moved to deleted records.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes, Delete",
        cancelButtonText: "Cancel"
    }).then(function (result) {
        if (!result.isConfirmed) {
            return;
        }
        $.ajax({
            url: "/Admin/AboutCounter/Delete",
            type: "POST",
            cache: false,
            data: { id: id },
            success: function (response) {
                if (response.success) {
                    Swal.fire({
                        icon: "success",
                        title: "Deleted",
                        text: response.message
                    }).then(function () {
                        loadCounterBox(window.currentRestoreFilter || "All");
                    });
                } else {
                    Swal.fire({
                        icon: "error",
                        title: "Error",
                        text: response.message
                    });
                }
            },
            error: function () {
                Swal.fire({
                    icon: "error",
                    title: "Error",
                    text: "Unable to delete record."
                });
            }
        });
    });
}
function restoreCounterBox(id) {
    var wrapper = $(".restore-timer-wrapper[data-id='" + id + "']");
    var deletedDate = wrapper.attr("data-deleted-date");
    if (!deletedDate) {
        Swal.fire({
            icon: "error",
            title: "Cannot Restore",
            text: "Deleted date not found."
        });
        return;
    }
    var deletedTime = new Date(deletedDate).getTime();
    var restoreTime = deletedTime + (60 * 60 * 1000);
    var currentTime = new Date().getTime();
    if (restoreTime > currentTime) {
        showRestoreRestrictionTimer(deletedDate);
        return;
    }
    Swal.fire({
        title: "Restore Record?",
        text: "This record will be restored.",
        icon: "question",
        showCancelButton: true,
        confirmButtonText: "Yes, Restore",
        cancelButtonText: "Cancel"
    }).then(function (result) {
        if (!result.isConfirmed) {
            return;
        }
        $.ajax({
            url: "/Admin/AboutCounter/Restore",
            type: "POST",
            cache: false,
            data: { id: id },
            success: function (response) {
                if (response.success) {
                    Swal.fire({
                        icon: "success",
                        title: "Restored",
                        text: response.message
                    }).then(function () {
                        loadCounterBox(window.currentRestoreFilter || "All");
                    });
                } else {
                    Swal.fire({
                        icon: "error",
                        title: "Cannot Restore",
                        text: response.message
                    });
                }
            },
            error: function () {
                Swal.fire({
                    icon: "error",
                    title: "Error",
                    text: "Unable to restore record."
                });
            }
        });
    });
}