function GetFCOChartData(formData = "") {
    $.ajax({
        type: "GET",
        url: "/FCODashboard/GetFCOChartsData",
        data: formData,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            FCOStatusChart(data.Status);
            FCORequestorChart(data.Requestor);
            FCOCompanyChart(data.Company);
            FCOAreaExecutionLeadChart(data.AreaExecutionLead);
            FCOBusinessTeamLeaderChart(data.BusinessTeamLeader);
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
function FCOCompanyChart(seriesData) {
    GeneratePieChart("fco-company", seriesData)
}
function FCOAreaExecutionLeadChart(seriesData) {
    GeneratePieChart("fco-area-execution-lead", seriesData)
}
function FCOBusinessTeamLeaderChart(seriesData) {
    GeneratePieChart("fco-business-team-leader", seriesData)
}