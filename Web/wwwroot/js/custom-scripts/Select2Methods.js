function Select2AutoCompleteAjax(id, url, dataArray, pageSize, placeholder = "---Select---", minimumInputLength = 0, appendCompleteObject = false, isMultiSelect = false, afterSelect = () => { }, afterUnSelect = () => { }, delay = 150) {
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
                minimumInputLength: minimumInputLength,
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
        $(document).on("select2:select", id, function (e) {
            var recordId = e.params.data.id;
            var modelPropertyName = GetModelPropertyName(id, isMultiSelect);
            let model = $(id).select2('data').filter(function (e) {
                return e.id == recordId;
            })[0].additionalAttributesModel;

            RemoveInput(modelPropertyName, recordId, isMultiSelect);
            if (appendCompleteObject) {
                AppendCompleteObject(id, recordId, model, modelPropertyName, isMultiSelect);
            }
            else {
                AppendInput(id, modelPropertyName, (modelPropertyName + ".Name"), model["Name"], recordId, isMultiSelect);
            }
            afterSelect(recordId, modelPropertyName);
        });
        $(document).on("select2:unselect", id, function (e) {
            var recordId = e.params.data.id;
            var modelPropertyName = GetModelPropertyName(id, isMultiSelect);
            RemoveInput(modelPropertyName, recordId, isMultiSelect);
            afterUnSelect(recordId, modelPropertyName);
        });

        function RemoveInput(modelPropertyName, recordId, isMultiSelect) {
            var inputIdentifier = GetElementInputIdentifier(modelPropertyName, recordId, isMultiSelect);
            if ($('input[data-identifier="' + inputIdentifier + '"]').length > 0)
                $('input[data-identifier="' + inputIdentifier + '"]').remove();
        }

        function AppendInput(elementId, modelPropertyName, appendedElementName, appendElementValue, recordId, isMultiSelect) {
            var inputIdentitfier = GetElementInputIdentifier(modelPropertyName, recordId, isMultiSelect);
            $('<input type="hidden" data-identifier=' + inputIdentitfier + ' name="' + appendedElementName + '" value="' + appendElementValue + '" />').insertAfter($(elementId));
        }
        function GetModelPropertyName(elementId, isMultiSelect) {
            var elementName = $(elementId).attr("name");
            var parentName = elementName.substring(0, elementName.lastIndexOf('.'));
            var name = parentName == "" ? elementName : parentName;
            if (isMultiSelect)
                name += "[]";
            return name;
        }
        function GetElementInputIdentifier(modelPropertyName, recordId, isMultiSelect) {
            if (isMultiSelect)
                return modelPropertyName + "-" + recordId;
            else
                return modelPropertyName;
        }
        function AppendCompleteObject(elementId, recordId, model, modelPropertyName, isMultiSelect) {
            Object.keys(model).forEach(key => {
                //if (key != "Id" && key != "name") {
                if (Array.isArray(model[key])) {
                    model[key].forEach((value, index) => {
                        var inputName = modelPropertyName + "." + key + "[]";
                        if (index == 0) {
                            AppendInput(elementId, modelPropertyName, inputName, value, recordId, isMultiSelect);
                        } else {
                            AppendInput(elementId, modelPropertyName, inputName, value, recordId, isMultiSelect);
                        }
                    });
                }
                else {
                    AppendInput(elementId, modelPropertyName, (modelPropertyName + "." + key), model[key], recordId, isMultiSelect);
                }
                //}


            });
        }
    }
    catch (err) {
        console.log(err);
    }
}
