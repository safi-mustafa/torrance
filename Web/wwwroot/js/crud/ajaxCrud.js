$.getScript("/js/crud/serialize-file.js", function () {
});
$.getScript("/js/crud/validation-summary.js", function () {
});

$(function () {
    $(document).on('keydown', "#crudModalPanelBody form", function (event) {
        if (event.key === "Enter") {
            event.preventDefault();
            //var childElement = $(this).children().first();
            //updateRecord(childElement);
        }
    })
})

function loadModalPanel(contentUrl, modalPanelId, modalPanelBody) {
    $.ajax({
        type: "GET",
        url: contentUrl,
        success: function (htmlContent) {
            $("#" + modalPanelBody).html(htmlContent);
            $("#" + modalPanelId).find(".modal-title").html($("#modal-title").val());
            $("#" + modalPanelId).modal("toggle");
        }
    });
}

function approveRecord(element, modalPanelId = "crudModalPanel") {
    $("#status-id").val(1);
    updateRecord(element, modalPanelId)
}
function rejectRecord(element, modalPanelId = "crudModalPanel") {
    $("#status-id").val(2);
    updateRecord(element, modalPanelId)
}

function approveDetail(element) {
    sendApproveAjax(1);
}

function rejectDetail(element) {
    sendApproveAjax(2);
}

function sendApproveAjax(status) {
    var controller = $("#controller-name").val();
    var id = $("#log-id").val();
    var isUnauthenticatedApproval = $("#is-unauthenticated-approval").val().toLowerCase();
    var approverId = $("#approver-id").val();
    var notificationId = $("#notification-id").val();
    //var reqEmail = $("#log-requestor").val();
    debugger;
    var url = "/" + controller + "/ApproveStatus";
    var data = { status: status, id: id, isUnauthenticatedApproval: isUnauthenticatedApproval, approverId: approverId, notificationId: notificationId };

    $.ajax({
        type: "Get",
        url: url,
        data: data,
        success: function (result) {
            $("#crudModalPanel").modal("toggle");
            if (isUnauthenticatedApproval == "false")
                ReInitializeDataTables();
            else {
                let statusMessage = "";
                if (status == "1") {
                    statusMessage = "Log Approved Successfully";
                }
                else {
                    statusMessage = "Log Rejected Successfully";
                }
                window.location.href = window.location.href + "&message=" + statusMessage;
            }

        }
    });
}

async function updateRecord(element, modalPanelId = "crudModalPanel") {
    var form = element.closest("form");
    var updateUrl = form.action;
    removeCurrencyMasking();
    var formData = $(form).serializeFiles();
    $(form).removeData("validator").removeData("unobtrusiveValidation");
    $.validator.unobtrusive.parse($(form));
    let isFormValid = $(form).valid();
    let areCustomConditionsValid = validateCustomConditions();
    if (isFormValid && areCustomConditionsValid) {// its set in variables so that both validation messages are shown at once
        clearValidationSummary(form);
        disableControls(form);
        try {
            const compressedImages = await compressAllImages(formData);

            // Loop through compressed images and update formData
            for (var i = 0; i < compressedImages.length; i++) {
                // Create a new input element with the compressed image data
                const { key, compressedImage } = compressedImages[i];
                formData.set(key, compressedImage);
            }
            $.ajax({
                type: "POST",
                url: updateUrl,
                data: formData,
                processData: false,
                contentType: false,
                beforeSend: function () {

                },
                success: function (result) {
                    if (!result.Success) {
                        createValidationSummary(form, result.Errors)
                    }
                    else {
                        ReInitializeDataTables();
                        enableControls(form);
                        $("#" + modalPanelId).modal("toggle");
                    }
                },
                complete: function () {
                    addCurrencyMasking();
                    enableControls(form);
                },
            });
        }
        catch {
            enableControls(form);
        }

    }
    addCurrencyMasking();

}


async function compressImageAsync(file) {
    return new Promise((resolve, reject) => {
        new Compressor(file, {
            quality: 0.5,
            maxWidth: 1200,
            success(result) {
                const compressedFile = new File([result], file.name, {
                    type: file.type,
                });
                resolve(compressedFile);
            },
            error(error) {
                reject(error);
            },
        });
    });
}

async function compressAllImages(formData) {
    const compressedImages = [];

    for (var pair of formData.entries()) {
        var [key, value] = pair;

        if (value instanceof File && value.type.startsWith("image/")) {
            try {
                const compressedImage = await compressImageAsync(value);
                console.log(compressedImage);
                formData.set(pair[0], compressedImage);
                compressedImages.push({ key, compressedImage });
            } catch (error) {
                console.error("Error during image compression:", error);
            }
        }
    }

    return compressedImages;
}
function disableControls(form) {
    DisableProperty(form, "#approve-btn", true);
    DisableProperty(form, "#reject-btn", true);
    DisableProperty(form, "#submit-btn", true, `<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Submitting...`);
    DisableProperty(form, ".cancel-btn", true);
    let modalPanel = $(form).closest("#crudModalPanel");

    $(modalPanel).off('hide.bs.modal');
    $(modalPanel).on('hide.bs.modal', function () {
        return false;
    });
    $(modalPanel).find("fieldset").block({
        centerY: false,
        centerX: false,
        css: {
            margin: 'auto',
            border: 'none',
            padding: '15px',
            backgroundColor: 'transparent',
            '-webkit-border-radius': '10px',
            '-moz-border-radius': '10px',
            color: 'transparent'
        }

    });
    $(modalPanel).find(".blockOverlay").css({ "background-color": "#fff", "opacity": "0.5" })
}
function enableControls(form) {
    if ($(form).find(".form-btn").is(":disabled")) {
        DisableProperty(form, ".cancel-btn", false);
        DisableProperty(form, "#submit-btn", false, "Submit");
        let modalPanel = $(form).closest("#crudModalPanel");
        $(modalPanel).off('hide.bs.modal');
        $(modalPanel).on('hide.bs.modal', function () {
            return true;
        });
        $(modalPanel).find("fieldset").unblock();
    }
}

function DisableProperty(form, target, status, html = "") {
    let btn = $(form).find(target);
    if (html != "")
        $(btn).html(`Submit`);
    $(btn).prop('disabled', status);
}

function removeCurrencyMasking() {
    $(".input-currency").each(function (index, element) {
        $(element).inputmask("remove");
    });
}
function addCurrencyMasking() {
    maskCurrency(".input-currency");
}

function validateCustomConditions() {
    return true;
}