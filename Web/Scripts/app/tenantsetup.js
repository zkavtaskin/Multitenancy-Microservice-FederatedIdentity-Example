
$(document).ready(function () {

    var tenantNameOld = '';
    var tenantNameFriendlyOld = '';

    setInterval(function () { generateFriendlyName($('#TenantName').val()); }, 500);

    $("#TenantName").blur(function () {
        checkFriendlyNameAvailabilityUniqueOnce($('#TenantFriendlyName').val());
    });

    $("#TenantFriendlyName").blur(function () {
        checkFriendlyNameAvailability($('#TenantFriendlyName').val());
    });

    $("#TenantFriendlyName").focus(function () {
        $("#available").hide();
        $("#unavailable").hide();
    });


    $('input[type="submit"]').on("click", function () {
        ga('send', 'event', 'Sign Up Button', 'submit');
    });

    function generateFriendlyName(tenantName) {

        if (tenantName == null || tenantName == '')
            return;

        if (tenantNameOld == tenantName)
            return;

        $.ajax({
            url: urlGenerateFriendlyName,
            type: "GET",
            data: { name: tenantName }
        }).done(function (data) {
            $('#TenantFriendlyName').val(data.FriendlyName);
            tenantNameOld = tenantName;
        });
    }

    function checkFriendlyNameAvailabilityUniqueOnce(tenantNameFriendly) {
        if (tenantNameFriendlyOld == tenantNameFriendly) {
            return;
        }

        checkFriendlyNameAvailability(tenantNameFriendly, function () {
            tenantNameFriendlyOld = tenantNameFriendly;
        });
    }

    function checkFriendlyNameAvailability(tenantNameFriendly, callback) {

        if (!$('form').validate().element('#TenantFriendlyName')) {
            $("#available").hide();
            $("#unavailable").hide();
            return;
        }
           
        $.ajax({
            url: urlTenantFriendlyNameCheck,
            type: "GET",
            data: { friendlyName: tenantNameFriendly }
        }).done(function (data) {

            if (data.IsAvailable) {
                $("#unavailable").hide();
                $("#available").show();
            } else {
                $("#available").hide();
                $("#unavailable").show();
            }

            callback();
        });
    }

})
