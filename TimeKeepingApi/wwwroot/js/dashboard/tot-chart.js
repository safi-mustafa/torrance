function GetTOTChartData(data) {
    TOTDepartmentChart(data.department);
    TOTUnitChart(data.unit);
    TOTShiftChart(data.shift);
    TOTRequestReasonChart(data.requestReason);
    TOTShiftDelayChart(data.shiftDelay);
    TOTReworkDelayChart(data.reworkDelay);
    TOTStartOfWorkDelayChart(data.startOfWorkDelay);
    TOTOngoingWorkDelayChart(data.ongoingWorkDelay);
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