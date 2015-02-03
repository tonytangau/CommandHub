$(function () {

    var hub = $.connection.commandHub;

    hub.client.broadcastMessage = function (message) {

        // Html encode display name and message.                
        var encodedMsg = $('<div />').text(message).html();

        // Add the message to the page.
        $('#messages').append('<li>' + encodedMsg + '</li>');
    }

    $.connection.hub.start().done(function () {
        //hub.server.updateDisplay(null);
    });
});