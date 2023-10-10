var roots = [];
const colorArray = [
    "#77B7D8",
    "#94C18D",
    "#D9A97A",
    "#B3AED0",
    "#8CB8A8",
    "#C0C6E1",
    "#A3C2B6",
    "#E1C340",
    "#B6D3A3",
    "#D3B6C6",
    "#B7D1E4",
    "#C4E59D",
    "#E1BBA3",
    "#A6C6E4",
    "#D0D39C",
    "#BFC5D9",
    "#A3D1C6",
    "#D9B8B3",
    "#C7D8A3",
    "#E7C9A7"
];
// For API Charts json helper converts our data into camelCase
const categoryFieldName = "category";
const valueFieldName = "value";
function GeneratePieChart(id, seriesData) {
    console.log(seriesData);
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
                valueField: valueFieldName,
                categoryField: categoryFieldName,
                endAngle: 270,
                alignLabels: false
            })
        );

        //series.labels.template.setAll({
        //    maxWidth: 150,
        //    oversizedBehavior: "truncate", // to truncate labels, use "truncate"
        //    fontSize: 10,
        //    textType: "circular", inside: true
        //});

        series.states.create("hidden", {
            endAngle: -90
        });

        // Set data
        // https://www.amcharts.com/docs/v5/charts/percent-charts/pie-chart/#Setting_data
        series.data.setAll(seriesData);

        series.labels.template.setAll({
            fontSize: "3.5vw",
            /* text: "{category}: [bold]{valuePercentTotal.formatNumber('0.00')}%[/] ({value})",*/
            text: "{category}: {valuePercentTotal.formatNumber('0.00')}%[/]",
            textType: "radial",
            radius: 0,
            centerX: am5.percent(100),
            fill: am5.color(0xffffff)
        })

        series.appear(1000, 100);

    }); // end am5.ready()
}
function GenerateBarChart(id, seriesData, setCustomBarChartSeriesColor = null) {
    const container = $("#" + id);
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
            panX: false,
            panY: false,
            wheelX: "none",
            wheelY: "none",
            pinchZoomX: false,
            touchEnabled: false,
            hoverEnabled: false
        }));
        const chartRoot = chart.root;

        // Disable all user interactions.
        chartRoot.interactionsEnabled = false;
        chart.interactive = false;
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
            categoryField: categoryFieldName,
            renderer: xRenderer,
            tooltip: am5.Tooltip.new(root, {})
        }));

        var yAxis = chart.yAxes.push(am5xy.ValueAxis.new(root, {
            maxDeviation: 0.3,
            renderer: am5xy.AxisRendererY.new(root, {
                strokeOpacity: 0.1,
                fontSize: 50
            })
        }));


        // Create series
        // https://www.amcharts.com/docs/v5/charts/xy-chart/series/
        var series = chart.series.push(am5xy.ColumnSeries.new(root, {
            name: "Series 1",
            xAxis: xAxis,
            yAxis: yAxis,
            valueYField: valueFieldName,
            sequencedInterpolation: true,
            categoryXField: categoryFieldName,
            tooltip: am5.Tooltip.new(root, {
                labelText: "{valueY}"
            }),
            maskBullets: false
        }));

        series.columns.template.setAll({ cornerRadiusTL: 5, cornerRadiusTR: 5, strokeOpacity: 0 });
        setBarChartToolTip(series, root, chart);
        //setBarChartSeriesBullets(series, root);
        if (setCustomBarChartSeriesColor && typeof setCustomBarChartSeriesColor === 'function') {
            setCustomBarChartSeriesColor(series);
        } else {
            // If series is longer than colorArray, add missing colors
            for (let i = colorArray.length; i < seriesData.length; i++) {
                colorArray.push(getRandomColor());
            }
            setBarChartSeriesColors(series);
        }

        xAxis.data.setAll(seriesData);
        xAxis.get("renderer").labels.template.setAll({
            oversizedBehavior: "truncate",
            textAlign: "center",
            maxHeight: 100,
            fontSize: "2vw"
        });
        yAxis.get("renderer").labels.template.set("fontSize", "1.5vw"); // Set your desired font size here
        series.data.setAll(seriesData);

        // Simulate an asynchronous action with a timeout
        // Make stuff animate on load
        // https://www.amcharts.com/docs/v5/concepts/animations/
        setTimeout(() => {
            container.find('.chart-loader').remove();
        }, 1000);
        series.appear(1000);
        chart.appear(1000, 100);

    });
}

// Function to generate a random hex color
function getRandomColor() {
    const letters = '0123456789ABCDEF';
    let color = '#';
    for (let i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
}

function setBarChartSeriesColors(series) {
    series.columns.template.adapters.add("fill", function (fill, target) {
        // Get the series index
        const seriesIndex = series.columns.indexOf(target);

        // Check if the index is within the bounds of colorArray
        if (seriesIndex >= 0 && seriesIndex < colorArray.length) {
            return am5.color(colorArray[seriesIndex]);
        }
    });

    series.columns.template.adapters.add("stroke", function (stroke, target) {
        // Get the series index
        const seriesIndex = series.columns.indexOf(target);

        // Check if the index is within the bounds of colorArray
        if (seriesIndex >= 0 && seriesIndex < colorArray.length) {
            return am5.color(colorArray[seriesIndex]);
        }
    });
}
function setBarChartSeriesBullets(series, root) {
    series.bullets.push(function () {
        return am5.Bullet.new(root, {
            locationX: 0.5,
            locationY: 0.5,
            sprite: am5.Label.new(root, {
                text: "{Value}",
                fill: root.interfaceColors.get("alternativeText"),
                centerX: am5.percent(50),
                centerY: am5.percent(50),
                populateText: true
            })
        });
    });
}
function setBarChartToolTip(series, root, barChart) {
    var tooltip = series.set("tooltip", am5.Tooltip.new(root, {
        getFillFromSprite: false,
        getStrokeFromSprite: true,
        autoTextColor: false,
        pointerOrientation: "horizontal"
    }));

    tooltip.get("background").setAll({
        fill: am5.color(0xffffff),
        fillOpacity: 0.8
    });

    tooltip.get("background").setAll({
        fill: am5.color(0xffffff)
    })

    tooltip.label.setAll({
        text: "{Category}[/]",
        fill: am5.color(0x000000)
    });

    tooltip.label.adapters.add("text", function (text, target) {
        return getBarChartToolTipContent(barChart, text);
    });
}
function getBarChartToolTipContent(barChart, text) {
    barChart.series.each(function (series) {
        //text += '\n[' + series.get("stroke").toString() + ']●[/] [bold width:100px]' + series.get("name") + ':[/] {' + series.get("valueYField") + '}'
        text += ': {' + series.get("valueYField") + '}'
    })
    return text;
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