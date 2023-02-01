$(function ($) {
    $.ajax({
        type: "GET",
        url: "/Home/GetProgressChartData",
        data: "",
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            
         let  newdata= data.map(item => {
             return Object.values(item)
           })
           // console.log(newdata);
            drawprogressmap(newdata);

        },
        error: function () {
            console.log("Error occured!!")
        }
    }); 

    $.ajax({
        type: "GET",
        url: "/Home/GetPieChartdata",
        data: "",
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
          

        },
        error: function () {
            console.log("Error occured!!")
        }
    }); 

    function drawprogressmap(data) {
        console.log(data,'afteras');
        anychart.onDocumentLoad(function () {
            // create line chart// create line chart
            var chart = anychart.line();

            // set chart padding
           // chart.padding([10, 20, 5, 20]);

            // turn on chart animation
            chart.animation(true);

            // turn on the crosshair
            chart.crosshair(true);

            // set chart title text settings
            chart.title('Over View');

            // set y axis title
            chart.yAxis().title('Count');


            // create logarithmic scale
            var logScale = anychart.scales.log();
            //logScale.minimum(1).maximum(45000);

            // set scale for the chart, this scale will be used in all scale dependent entries such axes, grids, etc
            chart.yScale(logScale);

            // create data set on our data,also we can pud data directly to series
            var dataSet = anychart.data.set(data);

            // map data for the first series,take value from first column of data set
            var firstSeriesData = dataSet.mapAs({ x: 0, value: 1 });

            // map data for the second series,take value from second column of data set
            var secondSeriesData = dataSet.mapAs({ x: 0, value: 2 });

            // map data for the third series, take x from the zero column and value from the third column of data set
            var thirdSeriesData = dataSet.mapAs({ x: 0, value: 3 });

            // temp variable to store series instance
            var series;

            // setup first series
            series = chart.line(firstSeriesData);
            series.name('WRR');
            // enable series data labels
            series.labels().enabled(true).anchor('left-bottom').padding(5);
            // enable series markers
            series.markers(true);

            // setup second series
            series = chart.line(secondSeriesData);
            series.name('OverRide');
            // enable series data labels
            series.labels().enabled(true).anchor('left-bottom').padding(5);
            // enable series markers
            series.markers(true);

            // setup third series
            series = chart.line(thirdSeriesData);
            series.name('TOT');
            // enable series data labels
            series.labels().enabled(true).anchor('left-bottom').padding(5);
            // enable series markers
            series.markers(true);

            // turn the legend on
            chart.legend().enabled(true).fontSize(13).padding([0, 0, 20, 0]);

            // set container for the chart and define padding
            chart.container('progress-chart-div');
            // initiate chart drawing
            chart.draw();
        });
    }

    anychart.onDocumentLoad(function () {
        // create an instance of a pie chart
        var chart = anychart.pie();
        // set the data
        chart.data([
            ["Shift 1", 5],
            ["Shift 2", 2],
        ]);
        // set chart title
        chart.title("Shifts");
        // set the container element 
        chart.container("pie-shift-chart");

        // initiate chart display
        chart.draw();
    });
})