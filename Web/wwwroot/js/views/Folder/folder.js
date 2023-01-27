$(function () {
    $(document).off('click', '#add-folder');
    $(document).on('click', '#add-folder', function (e) {
        var name = $("#folder-name").val();
        if (name != null && name != undefined && name != "") {
            var model = {
                'Name': name
            };
            AddFolder(model);
        }
        else {
            Swal.fire("Folder name is empty.")
        }
    });



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
                    $('#folder-list').html(result);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(textStatus, errorThrown);
                }

            });
            //   }


        }
        else {
            Swal.fire("Attachment name and file cannot be empty.")
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
        var UserId = $("#emp-id").val();
        var id = parseInt(Id);
        var userId = parseInt(UserId);
        $.ajax({
            url: "/Folder/_GetAttachmentView",
            type: "post",
            data: { 'id': id, 'userId': userId },
            dataType: "html",
            success: function (response) {
                $('#nav-documents').html(response);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(textStatus, errorThrown);
            }
        });
    });

    $(document).off('click', '#nav-documents-tab');
    $(document).on("click", "#nav-documents-tab", function (e) {
        e.preventDefault();
        Getfolders(empId);
    });

    $(document).off('click', '#back-to-folders');
    $(document).on("click", "#back-to-folders", function (e) {
        Getfolders(empId);
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


function AddFolder(model, empId) {
    $.ajax({
        url: "/Folder/Create",
        type: "post",
        data: { 'model': model },
        dataType: "html",
        success: function (response) {
            Getfolders();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(textStatus, errorThrown);
        }
    });
}