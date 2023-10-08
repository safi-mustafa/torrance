var roots = [];
function GeneratePieChart(id, seriesData) {
    am5.ready(function () {
        DisposeRoot(id);
        var root = am5.Root.new(id);
        if (roots.find(x => x.type == id) === undefined) {
            roots.push({ type: id, root: root });
        }

        root.setThemes([
            am5themes_Animated.new(root)
        ]);

        var chart = root.container.children.push(
            am5percent.PieChart.new(root, {
                endAngle: 270
            })
        );

        var series = chart.series.push(
            am5percent.PieSeries.new(root, {
                valueField: "value",
                categoryField: "category",
                endAngle: 270,
                alignLabels: false
            })
        );

        series.states.create("hidden", {
            endAngle: -90
        });

        // Set data
        // https://www.amcharts.com/docs/v5/charts/percent-charts/pie-chart/#Setting_data
        series.data.setAll(seriesData);

        series.labels.template.setAll({
            fontSize: "3.5vw",
            //style: {
            //    fontSize: "30px", // Set font size using vw units
            //},
            /* text: "{category}: [bold]{valuePercentTotal.formatNumber('0.00')}%[/] ({value})",*/
            text: "{category}: {valuePercentTotal.formatNumber('0.00')}%[/]",
            textType: "radial",
            radius: 0,
            centerX: am5.percent(100),
            fill: am5.color(0xffffff)
        });

        series.appear(1000, 100);

    }); // end am5.ready()
}
function GenerateBarChart(id, seriesData) {
    am5.ready(function () {

        DisposeRoot(id);
        var root = am5.Root.new(id);
        if (roots.find(x => x.type == id) === undefined) {
            roots.push({ type: id, root: root });
        }
        // Set themes
        // https://www.amcharts.com/docs/v5/concepts/themes/
        root.setThemes([
            am5themes_Animated.new(root)
        ]);


        // Create chart
        // https://www.amcharts.com/docs/v5/charts/xy-chart/
        var chart = root.container.children.push(am5xy.XYChart.new(root, {
            panX: true,
            panY: true,
            wheelX: "panX",
            wheelY: "zoomX",
            pinchZoomX: true
        }));

        // Add cursor
        // https://www.amcharts.com/docs/v5/charts/xy-chart/cursor/
        var cursor = chart.set("cursor", am5xy.XYCursor.new(root, {}));
        cursor.lineY.set("visible", false);


        // Create axes
        // https://www.amcharts.com/docs/v5/charts/xy-chart/axes/
        var xRenderer = am5xy.AxisRendererX.new(root, { minGridDistance: 30 });
        xRenderer.labels.template.setAll({
            rotation: -90,
            centerY: am5.p50,
            centerX: am5.p100,
            paddingRight: 15
        });

        xRenderer.grid.template.setAll({
            location: 1
        })

        var xAxis = chart.xAxes.push(am5xy.CategoryAxis.new(root, {
            maxDeviation: 0.3,
            categoryField: "Category",
            renderer: xRenderer,
            tooltip: am5.Tooltip.new(root, {})
        }));

        var yAxis = chart.yAxes.push(am5xy.ValueAxis.new(root, {
            maxDeviation: 0.3,
            renderer: am5xy.AxisRendererY.new(root, {
                strokeOpacity: 0.1
            })
        }));


        // Create series
        // https://www.amcharts.com/docs/v5/charts/xy-chart/series/
        var series = chart.series.push(am5xy.ColumnSeries.new(root, {
            name: "Series 1",
            xAxis: xAxis,
            yAxis: yAxis,
            valueYField: "Value",
            sequencedInterpolation: true,
            categoryXField: "Category",
            tooltip: am5.Tooltip.new(root, {
                labelText: "{valueY}"
            })
        }));

        series.columns.template.setAll({ cornerRadiusTL: 5, cornerRadiusTR: 5, strokeOpacity: 0 });
        //series.columns.template.adapters.add("fill", function (fill, target) {
        //    return chart.get("colors").getIndex(series.columns.indexOf(target));
        //});

        //series.columns.template.adapters.add("stroke", function (stroke, target) {
        //    return chart.get("colors").getIndex(series.columns.indexOf(target));
        //});
        series.columns.template.adapters.add("fill", function (fill, target) {
            if (target.dataItem.dataContext.Category == "Pending") {
                return am5.color("#ffa500");
            }
            else if (target.dataItem.dataContext.Category == "Approved") {
                return am5.color("#25b372");
            }
            else {
                return am5.color("#ff0000");
            }
        });

        series.columns.template.adapters.add("stroke", function (stroke, target) {
            if (target.dataItem.dataContext.Category == "Pending") {
                return am5.color("#ffa500");
            }
            else if (target.dataItem.dataContext.Category == "Approved") {
                return am5.color("#25b372");
            }
            else {
                return am5.color("#ff0000");
            }
        });

        xAxis.data.setAll(seriesData);
        series.data.setAll(seriesData);


        // Make stuff animate on load
        // https://www.amcharts.com/docs/v5/concepts/animations/
        series.appear(1000);
        chart.appear(1000, 100);
    });
}
function setBarChartSeriesColors(barChart) {
    barChart.get("colors").set("colors", [
        am5.color("#528d73"),
        am5.color("#ab0b00"),
    ]);
}
function DisposeRoot(root) {
    if (roots.length > 0) {
        if (root !== undefined && root !== "") {
            am5.array.each(am5.registry.rootElements, function (r) {
                if (r !== undefined && r.dom.id == root) {
                    r.dispose();
                }
            });
        }
        //else
        //    roots.forEach(function (v, i) {
        //        if (!$.isEmptyObject(v.root))
        //            v.root.dispose();
        //    })
    }
}