$(function () {

    var hub = $.connection.commandHub;

    hub.client.broadcastMessage = function (message) {

        // Html encode display name and message.                
        var encodedMsg = $('<div />').text(message).html();

        // Add the message to the page.
        $('#messages').append('<li>' + encodedMsg + '</li>');

        $('#chart').data().kendoChart.dataSource.read();
        //$('#chart').data().kendoChart.refresh();
    }

    $.connection.hub.start().done(function() {
    });

    createChart();
});

$(window).on("resize", function () {
    kendo.resize($(".chart-wrapper"));
});

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
        dataBound: function (e) {
            var data = e.sender.dataSource.view();

            // Find latest
            var lastItem = $(data).first()[0];

            if (lastItem) {
                $('#DFPressureReading').html($('<div />').text(lastItem.DFPressure.toFixed(6)).html());

                $('#DFFlowReading').html($('<div />').text(lastItem.DFFlow.toFixed(6)).html());

                $('#TorqueReading').html($('<div />').text(lastItem.Torque.toFixed(6)).html());

                $('#WOBReading').html($('<div />').text(lastItem.WOB.toFixed(6)).html());

                $('#RPMReading').html($('<div />').text(lastItem.RPM.toFixed(6)).html());

                $('#ROPReading').html($('<div />').text(lastItem.ROP.toFixed(6)).html());

                var minDFPressure = Math.min.apply(Math, data.map(function (h) { return h.DFPressure; }));
                var maxDFPressure = Math.max.apply(Math, data.map(function (h) { return h.DFPressure; }));

                $('#DFPressureReadingMin').html($('<span />').text(minDFPressure.toFixed(6)).html());
                $('#DFPressureReadingMax').html($('<span />').text(maxDFPressure.toFixed(6)).html());

                var minDFFlow = Math.min.apply(Math, data.map(function (h) { return h.DFFlow; }));
                var maxDFFlow = Math.max.apply(Math, data.map(function (h) { return h.DFFlow; }));

                $('#DFFlowReadingMin').html($('<span />').text(minDFFlow.toFixed(6)).html());
                $('#DFFlowReadingMax').html($('<span />').text(maxDFFlow.toFixed(6)).html());

                var minTorque = Math.min.apply(Math, data.map(function (h) { return h.Torque; }));
                var maxTorque = Math.max.apply(Math, data.map(function (h) { return h.Torque; }));

                $('#TorqueReadingMin').html($('<span />').text(minTorque.toFixed(6)).html());
                $('#TorqueReadingMax').html($('<span />').text(maxTorque.toFixed(6)).html());

                var minWOB = Math.min.apply(Math, data.map(function (h) { return h.WOB; }));
                var maxWOB = Math.max.apply(Math, data.map(function (h) { return h.WOB; }));

                $('#WOBReadingMin').html($('<span />').text(minWOB.toFixed(6)).html());
                $('#WOBReadingMax').html($('<span />').text(maxWOB.toFixed(6)).html());

                var minRPM = Math.min.apply(Math, data.map(function (h) { return h.RPM; }));
                var maxRPM = Math.max.apply(Math, data.map(function (h) { return h.RPM; }));

                $('#RPMReadingMin').html($('<span />').text(minRPM.toFixed(6)).html());
                $('#RPMReadingMax').html($('<span />').text(maxRPM.toFixed(6)).html());

                var minROP = Math.min.apply(Math, data.map(function (h) { return h.ROP; }));
                var maxROP = Math.max.apply(Math, data.map(function (h) { return h.ROP; }));

                $('#ROPReadingMin').html($('<span />').text(minROP.toFixed(6)).html());
                $('#ROPReadingMax').html($('<span />').text(maxROP.toFixed(6)).html());
            }
        }
    });
}