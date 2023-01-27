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

function Getfolders() {
    var searchValue = $("#search-value").val();
    $.ajax({
        url: "/Folder/_GetFolderView",
        type: "post",
        data: { 'Search.value': regexValue },
        dataType: "html",
        success: function (response) {
            $('#folder-list').html(response);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(textStatus, errorThrown);
        }
    });
}

function AddFolder() {

    var form = $("#folder-form");
    var updateUrl = "/Folder/Create";
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
    //$.ajax({
    //    url: "/Folder/Create",
    //    type: "post",
    //    data: { 'model': model },
    //    dataType: "html",
    //    success: function (response) {
    //        Getfolders();
    //    },
    //    error: function (jqXHR, textStatus, errorThrown) {
    //        console.log(textStatus, errorThrown);
    //    }
    //});
}