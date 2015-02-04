$(function () {

    var hub = $.connection.commandHub;

    hub.client.broadcastMessage = function (message) {

        // Html encode display name and message.                
        var encodedMsg = $('<div />').text(message).html();

        // Add the message to the page.
        $('#messages').append('<li>' + encodedMsg + '</li>');

        $('#chart').data().kendoChart.dataSource.read();
        $('#chart').data().kendoChart.refresh();
    }

    $.connection.hub.start().done(function() {
    });

    createChart();
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
        }
    });
}