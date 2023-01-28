$.getScript("/js/crud/serialize-file.js", function () {
});

$(function () {

    $(document).off('click', '.folder');
    $(document).on("click", ".folder", function (e) {
        e.preventDefault();
        var Id = $(this).attr("attr-id");
        var id = parseInt(Id);
        var url = "/Attachment/Index/" + id;
        window.location.href = url;
    });

    $(document).off('keyup', '#search-value');
    $(document).on('keyup', '#search-value', function (e) {
        e.preventDefault();
        ReInitializeDataTables();
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
        method: "POST",
        url: "/Folder/_GetFolders",
        data: { Name: searchValue },
        dataType: "html",
        success: function (response) {
            $('#folder-list').html(response);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(textStatus, errorThrown);
        }
    });
}

function loadCreateModalPanel(contentUrl, action = "Create Folder") {
    $("#modal-title").val(action);
    loadModalPanel(contentUrl, "crudModalPanel", "crudModalPanelBody")
}
