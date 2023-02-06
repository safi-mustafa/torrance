﻿function GetTOTStatusChartData(formData = "") {
    $.ajax({
        type: "GET",
        url: "/Home/GetTotStatusChartData",
        data: formData,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            GenerateBarChart("tot-status", data.ChartData);
        },
        error: function () {
            console.log("Error occured!!")
        }
    });
}