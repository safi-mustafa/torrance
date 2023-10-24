function GenerateTOTBarChart(seriesData) {
    GenerateBarChart("tot-status", seriesData, setStatusChartColors)
}
function GenerateORBarChart(seriesData) {
    GenerateBarChart("override-status", seriesData, setStatusChartColors)
}
function GenerateWRRBarChart(seriesData) {
    GenerateBarChart("wrr-status", seriesData, setStatusChartColors)
}
function setStatusChartColors(series) {
    series.columns.template.adapters.add("fill", function (fill, target) {
        if (target.dataItem.dataContext.category == "Pending") {
            return am5.color("#ffa500");
        }
        else if (target.dataItem.dataContext.category == "Approved") {
            return am5.color("#25b372");
        }
        else if (target.dataItem.dataContext.category == "Rejected") {
            return am5.color("#ff0000");
        }
        else {
            return am5.color("#800080");
        }
    });

    series.columns.template.adapters.add("stroke", function (stroke, target) {
        if (target.dataItem.dataContext.category == "Pending") {
            return am5.color("#ffa500");
        }
        else if (target.dataItem.dataContext.category == "Approved") {
            return am5.color("#25b372");
        }
        else if (target.dataItem.dataContext.category == "Rejected") {
            return am5.color("#ff0000");
        }
        else {
            return am5.color("#800080");
        }
    });
}
