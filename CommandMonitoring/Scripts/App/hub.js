$(function () {

    var hub = $.connection.commandHub;

    hub.client.broadcastMessage = function (message) {

        // Html encode display name and message.                
        var encodedMsg = $('<div />').text(message).html();

        // Add the message to the page.
        $('#messages').append('<li>' + encodedMsg + '</li>');
    }

    $.connection.hub.start();

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
                "TimeStamp": {type: "DateTime"},
                "WOB": { type: "Number" },
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
            },
            client: {
                read: "read"
            }
        }
    }
});

function createChart() {
    $("#chart").kendoChart({
        dataSource: hubDataSource,
        title: {
            text: "Command monitoring system"
        },
        legend: {
            position: "bottom"
        },
        chartArea: {
            background: ""
        },
        seriesDefaults: {
            type: "line"
        },
        series: [{
            field: "WOB",
            name: "WOB"
        }, {
            field: "ROP",
            name: "ROP"
        }],
        categoryAxis: {
            field: "TimeStamp",
            labels: {
                format: "hh:mm:ss"
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
            //},
            //line: {
            //    visible: false
            //}
        },
        tooltip: {
            visible: true,
            format: "N",
            template: "#= series.name #: #= value #"
        }
    });
}