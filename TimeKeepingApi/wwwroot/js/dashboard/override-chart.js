function GetOverrideChartData(data) {
    OverrideDepartmentChart(data.department);
    OverrideUnitChart(data.unit);
    OverrideShiftChart(data.shift);
    OverrrideRequestReasonChart(data.requestReason);

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