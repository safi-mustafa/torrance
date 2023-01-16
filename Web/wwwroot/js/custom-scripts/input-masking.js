Inputmask.extendAliases({
    dollars: {
        prefix: "$",
        groupSeparator: ".",
        alias: "decimal",
        numericInput: false,
        placeholder: "0",
        autoGroup: true,
        digits: 2,
        digitsOptional: false,
        clearMaskOnLostFocus: false
    }
});

$(document).ready(function () {
    $(".masking").inputmask({ alias: "dollars" });
});

