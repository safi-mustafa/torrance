$(function () {
    addAsterikToRequiredFields();
});
function addAsterikToRequiredFields() {
    $('input,select,textarea').each(function () {
        var req = $(this).attr('data-val-required');
        if (undefined != req && req != "" && $(this).attr('type') != "hidden") {
            var label = $(this).siblings(".form-label");
            var text = label.text();
            if (text.length > 0) {
                label.append('<span style="color:red"> *</span>');
            }
        }
    });
}

