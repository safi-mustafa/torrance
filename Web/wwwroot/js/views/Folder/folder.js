$.getScript("/js/crud/serialize-file.js", function () {
});

$(function () {
    $(document).off('click', '#add-folder');
    $(document).on('click', '#add-folder', function (e) {
        var name = $("#folder-name").val();
        if (name != null && name != undefined && name != "") {

            AddFolder();
        }
        else {
            Swal.fire("Folder name is empty.")
        }
    });

    $(document).off('click', '#edit-folder');
    $(document).on('click', '#edit-folder', function (e) {
        var id = $(this).attr("attr-id");
        var contentUrl = "/Folder/Update/" + id;
        $.ajax({
            type: "GET",
            url: contentUrl,
            success: function (htmlContent) {
                $("#folder-modal-div").html(htmlContent);
                $("#folder-modal").modal("toggle");
            }
        });
    });

    $(document).off('click', '.folder');
    $(document).on("click", ".folder", function (e) {
        e.preventDefault();
        var Id = $(this).attr("attr-id");
        var id = parseInt(Id);
        var url = "/Folder/GetAttachments/" + id;
        window.location.href = url;
    });
});

function ReInitializeDataTables() {
    var searchValue = $("#search-value").val();
    $.ajax({
        url: "/Folder/_GetFolders",
        type: "post",
        data: { 'Search.value': searchValue },
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
