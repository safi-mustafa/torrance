function maskCurrency(identifier) {
    $(identifier).inputmask({ alias: "currency", prefix: '$', removeMaskOnSubmit: true, rightAlign: false });
}
function maskDatatableCurrency(identifier, tableIdentifier) {
    $(tableIdentifier).find(identifier).inputmask({ alias: "currency", prefix: '$', removeMaskOnSubmit: true, rightAlign: false });
}


