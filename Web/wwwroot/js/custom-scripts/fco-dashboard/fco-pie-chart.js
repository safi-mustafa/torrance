function GetFCOChartData(formData = "") {
    $.ajax({
        type: "GET",
        url: "/FCODashboard/Index",
        data: formData,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            FCOStatusChart(data.Status);
            FCORequestorChart(data.Requestor);
            FCOApproverChart(data.Approver);
            FCOCompanyChart(data.Company);
        },
        error: function () {
            console.log("Error occured!!")
        }
    });
}

function FCOStatusChart(seriesData) {
    GeneratePieChart("fco-status", seriesData)
}
function FCORequestorChart(seriesData) {
    GeneratePieChart("fco-requestor", seriesData)
}
function FCOApproverChart(seriesData) {
    GeneratePieChart("fco-approver", seriesData)
}
function FCOCompanyChart(seriesData) {
    GeneratePieChart("fco-company", seriesData)
}