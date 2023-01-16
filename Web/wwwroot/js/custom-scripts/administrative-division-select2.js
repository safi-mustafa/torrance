var administrativeDivisionTypes = ["country", "province", "division", "district", "sub-division", "tehsil", "union-council", "village", "sub-village"];
function onAdministrativeDivisionTypeChange(id, name) {
   // var divisionTypeIndex = administrativeDivisionTypes.findIndex(e => id.includes(e));
    //var arrayItem = "";
    //if (id.replace("#", "").split("-").length > 2) {
    //    arrayItem = id.replace("#", "").split("-")[0] + "-" + id.replace("#", "").split("-")[1];
    //}
    //else {
    //    arrayItem = id.replace("#", "").split("-")[0];
    //}  
    var divisionTypeIndex = administrativeDivisionTypes.findIndex(x => x === name);
    if (divisionTypeIndex > -1) {
        var divisionType = administrativeDivisionTypes[divisionTypeIndex];
        var childDivisionType = administrativeDivisionTypes[divisionTypeIndex + 1];
        var childDivisionTypeId = id.replace(divisionType, childDivisionType);
        $(id).on("change", function (e) {
            if ($(childDivisionTypeId).length > 0) {
                $(childDivisionTypeId).val(null).trigger('change');
            }
        });
    }
}
