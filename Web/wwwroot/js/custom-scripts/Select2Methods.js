function Select2AutoCompleteAjax(id, url, dataArray, pageSize, placeholder = "---Select---") {
    try {
        var parent = $(id).parent();
        $(id).select2(
            {
                dropdownParent: $(parent),
                
                ajax: {
                    delay: 150,
                    url: url,
                    dataType: 'json',
                    data: dataArray,
                    processResults: function (result, params) {
                        params.page = params.page || 1;
                        var check = {
                            results: result.Results,
                            pagination: {
                              //  more: false
                                more: (params.page * pageSize) < result.total
                            }
                        };
                        return check;
                    },
                    success: function (response) {
                        //console.log(response);
                    },
                },
                placeholder: placeholder,
                minimumInputLength: 0,
                allowClear: true,
            });
        $(document).on("change", id, function () {
            var propertyValue = $(id + ' :selected').text();
            var propertyName = $(id).attr("name");
            var parentName = propertyName.substring(0, propertyName.lastIndexOf('.'));
            var appendedElementName = parentName + ".Name";
            if ($('input[name="' + appendedElementName + '"]').length > 0)
                $('input[name="' + appendedElementName + '"]').remove();
            $('<input type="hidden" name="' + appendedElementName + '" value="' + propertyValue + '" />').insertAfter($(id));
        });
    }
    catch (err) {
        console.log(err);
    }
}
