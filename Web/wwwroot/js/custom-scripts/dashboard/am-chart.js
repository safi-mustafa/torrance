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
                valueField: "Value",
                categoryField: "Category",
                endAngle: 270
            })
        );

        series.states.create("hidden", {
            endAngle: -90
        });

        // Set data
        // https://www.amcharts.com/docs/v5/charts/percent-charts/pie-chart/#Setting_data
        series.data.setAll(seriesData);

        series.appear(1000, 100);

    }); // end am5.ready()
}
function DisposeRoot(root) {
    if (roots.length > 0) {
        if (root !== undefined && root !== "") {
            var rootObj = roots.find(x => x.type == root)?.root;
            if (!$.isEmptyObject(rootObj))
                rootObj.dispose();
        }
        //else
        //    roots.forEach(function (v, i) {
        //        if (!$.isEmptyObject(v.root))
        //            v.root.dispose();
        //    })
    }
}