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
    GenerateBarChart("override-department", seriesData)
}
function OverrideUnitChart(seriesData) {
    GenerateBarChart("override-unit", seriesData)
}
function OverrideShiftChart(seriesData) {

    GenerateBarChart("override-shift", seriesData)
}
function OverrrideRequestReasonChart(seriesData) {
    GenerateBarChart("override-request-reason", seriesData)
}