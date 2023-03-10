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
                        console.log(params.page, pageSize, (params.page * pageSize), result, result.Total);
                        params.page = params.page || 1;
                        var check = {
                            results: result.Results,
                            pagination: {
                              //  more: false
                                more: (params.page * pageSize) < result.Total
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
        // Add on scroll pagination
        $(id).on('select2:open', function () {
            var dropdown = $(this).data('select2').$dropdown;
            dropdown.off('scroll.select2');
            dropdown.on('scroll.select2', function () {
                var scrollHeight = dropdown.get(0).scrollHeight;
                var scrollTop = dropdown.scrollTop();
                var height = dropdown.height();
                if (scrollTop + height >= scrollHeight) {
                    var select2 = $(id).data('select2');
                    var params = select2.dropdown._params;
                    if (params.pagination.more) {
                        params.page++;
                        select2.trigger('query', params);
                    }
                }
            });
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
