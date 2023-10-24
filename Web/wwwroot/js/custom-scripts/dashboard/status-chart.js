function GetStatusChartData(type, containerId, formData = "") {
    let url = "/Home/Get" + type + "StatusChartData";
    $.ajax({
        type: "GET",
        url: url,
        data: formData,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            GenerateBarChart(containerId, data.ChartData, setStatusChartColors);
        },
        error: function () {
            console.log("Error occured!!")
        }
    });
}
function setStatusChartColors(series) {
    series.columns.template.adapters.add("fill", function (fill, target) {
        if (target.dataItem.dataContext.Category == "Pending") {
            return am5.color("#ffa500");
        }
        else if (target.dataItem.dataContext.Category == "Approved") {
            return am5.color("#25b372");
        }
        else if (target.dataItem.dataContext.Category == "Rejected") {
            return am5.color("#ff0000");
        }
        else {
            return am5.color("#800080");
        }
    });

    series.columns.template.adapters.add("stroke", function (stroke, target) {
        if (target.dataItem.dataContext.Category == "Pending") {
            return am5.color("#ffa500");
        }
        else if (target.dataItem.dataContext.Category == "Approved") {
            return am5.color("#25b372");
        }
        else if (target.dataItem.dataContext.Category == "Rejected") {
            return am5.color("#ff0000");
        }
        else {
            return am5.color("#800080");
        }
    });
}
