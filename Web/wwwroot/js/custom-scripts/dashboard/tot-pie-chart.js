function GetTOTChartData(formData = "") {
    $.ajax({
        type: "GET",
        url: "/Home/GetTotChartsData",
        data: formData,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            ShiftDelayChart(data.ShiftDelays);
            ReworkDelayChart(data.ReworkDelays)
        },
        error: function () {
            console.log("Error occured!!")
        }
    });
}
function ShiftDelayChart(seriesData) {

    GeneratePieChart("tot-shift-delay", seriesData)
}
function ReworkDelayChart(seriesData) {
    GeneratePieChart("tot-rework-delay", seriesData)
}