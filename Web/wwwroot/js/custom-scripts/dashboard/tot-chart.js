function GetTOTChartData(isAdmin, isApprover, formData = "") {
    $.ajax({
        type: "GET",
        url: "/Home/GetTotChartsData",
        data: formData,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (isAdmin == "True") {
                TOTDepartmentChart(data.Department);
                TOTUnitChart(data.Unit);
                TOTShiftChart(data.Shift);
                TOTRequestReasonChart(data.RequestReason);
            }
            if (isAdmin == "True" || isApprover == "True") {
                TOTShiftDelayChart(data.ShiftDelay);
                TOTReworkDelayChart(data.ReworkDelay);
                TOTStartOfWorkDelayChart(data.StartOfWorkDelay);
                TOTOngoingWorkDelayChart(data.OngoingWorkDelay);
            }
            
        },
        error: function () {
            console.log("Error occured!!")
        }
    });
}

function TOTDepartmentChart(seriesData) {
    GenerateBarChart("tot-department", seriesData)
}
function TOTUnitChart(seriesData) {
    GenerateBarChart("tot-unit", seriesData)
}
function TOTShiftChart(seriesData) {

    GenerateBarChart("tot-shift", seriesData)
}
function TOTRequestReasonChart(seriesData) {
    GenerateBarChart("tot-request-reason", seriesData)
}

function TOTShiftDelayChart(seriesData) {
    GenerateBarChart("tot-shift-delay", seriesData)
}
function TOTReworkDelayChart(seriesData) {
    GenerateBarChart("tot-rework-delay", seriesData)
}
function TOTStartOfWorkDelayChart(seriesData) {
    GenerateBarChart("tot-start-of-work-delay", seriesData)
}
function TOTOngoingWorkDelayChart(seriesData) {
    GenerateBarChart("tot-ongoing-work-delay", seriesData)
}