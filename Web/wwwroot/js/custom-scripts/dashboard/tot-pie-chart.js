function GetTOTChartData(formData = "") {
    $.ajax({
        type: "GET",
        url: "/Home/GetTotChartsData",
        data: formData,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            TOTDepartmentChart(data.Department);
            TOTUnitChart(data.Unit);
            TOTShiftChart(data.Shift);
            TOTRequestReasonChart(data.RequestReason);
        },
        error: function () {
            console.log("Error occured!!")
        }
    });
}

function TOTDepartmentChart(seriesData) {
    GeneratePieChart("tot-department", seriesData)
}
function TOTUnitChart(seriesData) {
    GeneratePieChart("tot-unit", seriesData)
}
function TOTShiftChart(seriesData) {

    GeneratePieChart("tot-shift", seriesData)
}
function TOTRequestReasonChart(seriesData) {
    GeneratePieChart("tot-request-reason", seriesData)
}