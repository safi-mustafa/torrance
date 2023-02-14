function GetStatusChartData(type, containerId, formData = "") {
    let url = "/Home/Get" + type + "StatusChartData";
    $.ajax({
        type: "GET",
        url: url,
        data: formData,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            GenerateBarChart(containerId, data.ChartData);
        },
        error: function () {
            console.log("Error occured!!")
        }
    });
}
