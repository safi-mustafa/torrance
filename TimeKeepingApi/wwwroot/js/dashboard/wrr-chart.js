function GetWrrChartData(data) {
    WeldMethodChart(data.weldMethods);
    RodTypeChart(data.rodTypes)
}
function WeldMethodChart(seriesData) {
    GenerateBarChart("wrr-weld-method", seriesData)
}
function RodTypeChart(seriesData) {
    GenerateBarChart("wrr-rod-type", seriesData)
}