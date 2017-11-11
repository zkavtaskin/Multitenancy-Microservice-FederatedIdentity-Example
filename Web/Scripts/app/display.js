
$(document).ready(function () {

    TimeCountUp.init({
        selector: ".timer",
        dateTimeSelector: "#stoppedDateTime",
        valueSelector: "value",
        updateInterval: 1000
    });
    TimeCountUp.start();

    function updateDashboard() {
        $.ajax({
            cache: false,
            url: urlDataSource,
            type: "GET"
        }).done(function (data) {
            $('#display').html('');
            $('#display').html(data);
            $("#connectionInformation").hide();
            $.validator.unobtrusive.parse("form");
        }).fail(function () {
            $("#connectionInformation").show();
        });
    }

    var group = $.connection.groupHub;
    group.client.groupLifeCycleStateChange = function (tenantFriendlyName, groupLifeCycleState) {
        if (tenantFriendlyNameCurrent == tenantFriendlyName) {
            updateDashboard();
        }
    }
    
    var connection = $.connection.hub;
    var disconnected = false;
    connection.disconnected(function () {
        $("#connectionInformation").show();
        disconnected = true;
        setInterval(connectionStart(), 10000);
    });

    function connectionStart() {
        connection.start().done(function () {
            if (disconnected) {
                $("#connectionInformation").hide();
                updateDashboard();
                disconnected = false;
            }        
        });
    }

    connectionStart();

});
