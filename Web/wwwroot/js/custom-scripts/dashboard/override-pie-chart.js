function GetOverrideChartData(formData = "") {
    $.ajax({
        type: "GET",
        url: "/Home/GetOverrideChartsData",
        data: formData,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            OverrideDepartmentChart(data.Department);
            OverrideUnitChart(data.Unit);
            OverrideShiftChart(data.Shift);
            OverrrideRequestReasonChart(data.RequestReason);
        },
        error: function () {
            console.log("Error occured!!")
        }
    });
}

function OverrideDepartmentChart(seriesData) {
    GeneratePieChart("override-department", seriesData)
}
function OverrideUnitChart(seriesData) {
    GeneratePieChart("override-unit", seriesData)
}
function OverrideShiftChart(seriesData) {

    GeneratePieChart("override-shift", seriesData)
}
function OverrrideRequestReasonChart(seriesData) {
    GeneratePieChart("override-request-reason", seriesData)
}