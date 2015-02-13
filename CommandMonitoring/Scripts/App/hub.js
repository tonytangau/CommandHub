var hub = $.connection.commandHub;

$(function () {

    $.connection.hub.start();

    createChart();

    $('#DFPressureButton').click(function () {
        filterSeries('DFPressure');
    });

    $('#DFFlowButton').click(function () {
        filterSeries('DFFlow');
    });

    $('#TorqueButton').click(function () {
        filterSeries('Torque');
    });

    $('#WOBButton').click(function () {
        filterSeries('WOB');
    });

    $('#RPMButton').click(function () {
        filterSeries('RPM');
    });

    $('#ROPButton').click(function () {
        filterSeries('ROP');
    });
});

function filterSeries(fieldName) {
    var chart = $('#chart').data().kendoChart;

    $(chart.options.series).each(function (index) {
        if (chart.options.series[index].field != fieldName) {
            chart.options.series[index].visible = false;
        } else {
            chart.options.series[index].visible = true;
        }
    });

    chart.redraw();
}

hub.client.broadcastMessage = function () {

    $('#chart').data().kendoChart.dataSource.read();
}

var hubDataSource = new kendo.data.DataSource({
    type: "signalr",
    autoSync: true,
    schema: {
        model: {
            id: "DrillHoleId",
            fields: {
                "DrillHoleId": { type: "Number" },
                "TimeStamp": { type: "DateTime" },
                "DFPressure": { type: "Number" },
                "DFFlow": { type: "Number" },
                "Torque": { type: "Number" },
                "WOB": { type: "Number" },
                "RPM": { type: "Number" },
                "ROP": { type: "Number" }
            }
        }
    },
    transport: {
        signalr: {
            promise: $.connection.hub.start(),
            hub: $.connection.commandHub,
            server: {
                read: "read"
            }
        }
    }
});

function createChart() {
    $("#chart").kendoChart({
        dataSource: hubDataSource,
        title: {
            text: "DMS monitoring system"
        },
        legend: {
            position: "top"
        },
        chartArea: {
            background: ""
        },
        seriesDefaults: {
            type: "line",
            style: "smooth"
        },
        series: [{
            field: "DFPressure",
            name: "DF Pressure"
        }, {
            field: "DFFlow",
            name: "DF Flow"
        }, {
            field: "Torque",
            name: "Torque"
        }, {
            field: "WOB",
            name: "WOB"
        }, {
            field: "RPM",
            name: "RPM"
        }, {
            field: "ROP",
            name: "ROP"
        }],
        categoryAxis: {
            field: "TimeStamp",
            type: "Date",
            baseUnit: "seconds",
            labels: {
                rotation: 90,
                dateFormats:
                {
                    seconds: "HH:mm:ss",
                    minutes: "HH:mm",
                    hours: "HH:mm",
                    days: "dd/MM",
                    months: "MMM yy",
                    years: "yyyy"
                }
            }
            //},
            //majorGridLines: {
            //    visible: false
            //}
        },
        valueAxis: {
            labels: {
                format: "N"
            }
        },
        tooltip: {
            visible: true,
            format: "N",
            template: "#= series.name #: #= value #"
        },
        dataBound: chartDataBound
    });
}

function chartDataBound(e) {
    var data = e.sender.dataSource.view();

    // Find latest
    var lastItem = $(data).first()[0];

    if (lastItem) {
        $('#DFPressureReading').html($('<div />').text(lastItem.DFPressure).html());

        $('#DFFlowReading').html($('<div />').text(lastItem.DFFlow).html());

        $('#TorqueReading').html($('<div />').text(lastItem.Torque).html());

        $('#WOBReading').html($('<div />').text(lastItem.WOB).html());

        $('#RPMReading').html($('<div />').text(lastItem.RPM).html());

        $('#ROPReading').html($('<div />').text(lastItem.ROP).html());

        var minDFPressure = Math.min.apply(Math, data.map(function (h) { return h.DFPressure; }));
        var maxDFPressure = Math.max.apply(Math, data.map(function (h) { return h.DFPressure; }));

        $('#DFPressureReadingMin').html($('<span />').text(minDFPressure).html());
        $('#DFPressureReadingMax').html($('<span />').text(maxDFPressure).html());

        var minDFFlow = Math.min.apply(Math, data.map(function (h) { return h.DFFlow; }));
        var maxDFFlow = Math.max.apply(Math, data.map(function (h) { return h.DFFlow; }));

        $('#DFFlowReadingMin').html($('<span />').text(minDFFlow).html());
        $('#DFFlowReadingMax').html($('<span />').text(maxDFFlow).html());

        var minTorque = Math.min.apply(Math, data.map(function (h) { return h.Torque; }));
        var maxTorque = Math.max.apply(Math, data.map(function (h) { return h.Torque; }));

        $('#TorqueReadingMin').html($('<span />').text(minTorque).html());
        $('#TorqueReadingMax').html($('<span />').text(maxTorque).html());

        var minWOB = Math.min.apply(Math, data.map(function (h) { return h.WOB; }));
        var maxWOB = Math.max.apply(Math, data.map(function (h) { return h.WOB; }));

        $('#WOBReadingMin').html($('<span />').text(minWOB).html());
        $('#WOBReadingMax').html($('<span />').text(maxWOB).html());

        var minRPM = Math.min.apply(Math, data.map(function (h) { return h.RPM; }));
        var maxRPM = Math.max.apply(Math, data.map(function (h) { return h.RPM; }));

        $('#RPMReadingMin').html($('<span />').text(minRPM).html());
        $('#RPMReadingMax').html($('<span />').text(maxRPM).html());

        var minROP = Math.min.apply(Math, data.map(function (h) { return h.ROP; }));
        var maxROP = Math.max.apply(Math, data.map(function (h) { return h.ROP; }));

        $('#ROPReadingMin').html($('<span />').text(minROP).html());
        $('#ROPReadingMax').html($('<span />').text(maxROP).html());
    }
}

$(window).on("resize", function () {
    kendo.resize($(".chart-wrapper"));
});