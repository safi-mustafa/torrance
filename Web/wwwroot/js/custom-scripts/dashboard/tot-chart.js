function GetTOTChartData(isAdmin, isApprover, queryString = "") {
  $.ajax({
    url: "/Home/GetTotChartsData",
    type: "GET",
    data: queryString,
    success: function (response) {
      if (response) {
        // Total Count by Delay Type
        GenerateBarChart("tot-delay-type", response.DelayTypeHours);
        // Total Count by Unit
        GenerateBarChart("tot-unit-count", response.UnitCount);
        // Total Hours by Unit
        GenerateBarChart("tot-unit", response.Unit);
      }
    },
    error: function () {
      console.log("Error occurred while fetching TOT chart data");
    },
  });
}

// Remove all other chart functions since we're not using them anymore
