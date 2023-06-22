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
            TOTShiftDelayChart(data.ShiftDelay);
            TOTReworkDelayChart(data.ReworkDelay);
            TOTStartOfWorkDelayChart(data.StartOfWorkDelay);
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

function TOTShiftDelayChart(seriesData) {
    GeneratePieChart("tot-shift-delay", seriesData)
}
function TOTReworkDelayChart(seriesData) {
    console.log("HERE");
    console.log(seriesData);
    GeneratePieChart("tot-rework-delay", seriesData)
}
function TOTStartOfWorkDelayChart(seriesData) {
    GeneratePieChart("tot-start-of-work-delay", seriesData)
}