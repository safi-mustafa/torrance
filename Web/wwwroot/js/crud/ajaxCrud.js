$.getScript("/js/crud/serialize-file.js", function () {
});
$.getScript("/js/crud/validation-summary.js", function () {
});

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
    $("#Status").val(1);
    updateRecord(element, modalPanelId)
}
function rejectRecord(element, modalPanelId = "crudModalPanel") {
    $("#Status").val(2);
    updateRecord(element, modalPanelId)
}

function updateRecord(element, modalPanelId = "crudModalPanel") {
    var form = element.closest("form")
    var updateUrl = form.action;
    removeCurrencyMasking();
    var formData = $(form).serializeFiles();
    $(form).removeData("validator").removeData("unobtrusiveValidation");
    $.validator.unobtrusive.parse($(form));
    if ($(form).valid()) {
        clearValidationSummary(form);
        $.ajax({
            type: "POST",
            url: updateUrl,
            data: formData,
            processData: false,
            contentType: false,
            beforeSend: function () {
                disableControls(form);
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
    addCurrencyMasking();

}
function disableControls(form) {
    let submitBtn = $(form).find("#submit-btn");
    let clearBtn = $(form).find(".cancel-btn");
    let modalPanel = $(form).closest("#crudModalPanel");
    $(submitBtn).html(
        `<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Submitting...`
    );
    $(submitBtn).prop('disabled', true);
    $(clearBtn).prop('disabled', true);
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
    let submitBtn = $(form).find("#submit-btn");
    if ($(submitBtn).is(":disabled")) {
        let clearBtn = $(form).find(".cancel-btn");
        let modalPanel = $(form).closest("#crudModalPanel");
        $(submitBtn).html(`Submit`);
        $(submitBtn).prop('disabled', false);
        $(clearBtn).prop('disabled', false);
        $(modalPanel).off('hide.bs.modal');
        $(modalPanel).on('hide.bs.modal', function () {
            return true;
        });
        $(modalPanel).find("fieldset").unblock();
    }

}

function removeCurrencyMasking() {
    $(".input-currency").each(function (index, element) {
        $(element).inputmask("remove");
    });
}
function addCurrencyMasking() {
    maskCurrency(".input-currency");
}