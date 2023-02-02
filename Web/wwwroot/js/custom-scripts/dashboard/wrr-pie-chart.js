function GetWrrChartData() {
    $.ajax({
        type: "GET",
        url: "/Home/GetWrrChartsData",
        data: "",
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            WeldMethodChart(data.WeldMethods);
            RodTypeChart(data.RodTypes)
        },
        error: function () {
            console.log("Error occured!!")
        }
    });
}
function WeldMethodChart(seriesData) {
    GeneratePieChart("wrr-weld-method", seriesData)
}
function RodTypeChart(seriesData) {
    GeneratePieChart("wrr-rod-type", seriesData)
}