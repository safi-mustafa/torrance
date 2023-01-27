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
}));
