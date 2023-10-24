function GetWrrChartData(formData = "") {
    $.ajax({
        type: "GET",
        url: "/Home/GetWrrChartsData",
        data: formData,
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
    GenerateBarChart("wrr-weld-method", seriesData)
}
function RodTypeChart(seriesData) {
    GenerateBarChart("wrr-rod-type", seriesData)
}