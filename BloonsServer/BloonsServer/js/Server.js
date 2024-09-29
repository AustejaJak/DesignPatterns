$(function () {
    var connection = $.connection('/server');

    connection.received(function (data) {
        $("#data").append(data + '<br />');
    });

    connection.start().done();

    $("#send").clicl(function () {
        connection.send($("#message").val());
    });
});