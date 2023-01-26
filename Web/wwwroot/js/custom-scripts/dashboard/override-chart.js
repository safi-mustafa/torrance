function GetOverrideChartData(queryString = "") {
  $.ajax({
    url: "/Home/GetOverrideChartsData",
    type: "GET",
    data: queryString,
    success: function (response) {
      if (response) {
        // Total Count by Unit
        GenerateBarChart("override-unit-count", response.UnitCount);
        // Total Hours by Unit
        GenerateBarChart("override-unit-hours", response.UnitHours);
        // Total Cost by Unit
        GenerateBarChartWithCurrency("override-unit-cost", response.UnitCost);
      }
    },
    error: function () {
      console.log("Error occurred while fetching Override chart data");
    },
  });
}

function OverrideDepartmentChart(seriesData) {
  GenerateBarChart("override-department", seriesData);
}
function OverrideUnitChart(seriesData) {
  GenerateBarChart("override-unit", seriesData);
}
function OverrideShiftChart(seriesData) {
  GenerateBarChart("override-shift", seriesData);
}
function OverrrideRequestReasonChart(seriesData) {
  GenerateBarChart("override-request-reason", seriesData);
}
