$.getScript("/js/crud/serialize-file.js", function () {
});

$(function () {

    $(document).off('click', '#add-attachment');
    $(document).on('click', '#add-attachment', function (e) {
        var name = $("#file-name").val();
        var file = $("#file").val();
        if (name && file) {

            var form = $("#attachment-form");
            var updateUrl = "/Folder/CreateAttachment";
            var formData = $(form).serializeFiles();
            $.ajax({
                type: "POST",
                url: updateUrl,
                data: formData,
                enctype: 'multipart/form-data',
                processData: false,
                contentType: false,
                cache: false,
                success: function (result) {
                    $("#attachment-modal").modal('hide');
                    $('#attachment-list').html(result);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(textStatus, errorThrown);
                }
            });
        }
        else {
            Swal.fire("Attachment name and file cannot be empty.")
        }
    });

    $(document).off('click', '#back-to-folders');
    $(document).on("click", "#back-to-folders", function (e) {
        window.location.href = "/Folder/Index";
    });

    $(document).off('click', '.delete');
    $(document).on('click', '.delete', function (e) {
        e.preventDefault();
        DeleteDataItem($(this).attr("href"));
    });
});

function DeleteDataItem(deleteUrl) {
    var confirmBtnText = "Yes, delete it!";
    var cancelBtnText = "No, cancel!";
    swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        type: 'error',
        //icon: 'warning',
        showCancelButton: true,
        confirmButtonText: confirmBtnText,
        cancelButtonText: cancelBtnText,
        confirmButtonClass: 'btn btn-success',
        cancelButtonClass: 'btn btn-danger',
        buttonsStyling: false
    }).then(function (result) {
        if (result.value) {
            DeleteItem(deleteUrl).then(function (ajaxResult) {
                if (ajaxResult.Success) {
                    ReInitializeDataTables();
                }
                else {
                    Swal.fire("Couldn't delete. Try again later.")
                }
            });

        }
        else if (result.dismiss === swal.DismissReason.cancel) {
        }
    });
}

function DeleteItem(url) {
    return $.ajax({
        url: url,
        type: 'POST',
        success: function (res) {
        }
    });
}

function loadUpdateAndDetailModalPanel(contentUrl) {
    loadModalPanel(contentUrl, "crudModalPanel", "crudModalPanelBody")
}

function ReInitializeDataTables() {
    var searchValue = $("#search-value").val();
    $.ajax({
        url: "/Attachment/_GetAttachments",
        type: "post",
        data: { 'Search.value': searchValue },
        dataType: "html",
        success: function (response) {
            $('#attachment-list').html(response);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(textStatus, errorThrown);
        }
    });
}

function loadCreateModalPanel(contentUrl, action = "Create Attachment") {
    $("#modal-title").val(action);
    loadModalPanel(contentUrl, "crudModalPanel", "crudModalPanelBody")
}
