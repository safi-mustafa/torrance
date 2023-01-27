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

    $.fn.serializeFiles = function () {
        var formData = new FormData();
        var obj = $(this);
        /* ADD FILE TO PARAM AJAX */
        //var formData = new FormData();
        $.each($(obj).find("input[type='file']"), function (i, tag) {
            $.each($(tag)[0].files, function (i, file) {
                formData.append(tag.name, file);
            });
        });
        var params = $(obj).serializeArray();
        $.each(params, function (i, val) {
            formData.append(val.name, val.value);
        });
        return formData;
    };

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