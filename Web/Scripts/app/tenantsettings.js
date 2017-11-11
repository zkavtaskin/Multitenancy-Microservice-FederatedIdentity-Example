
$(document).ready(function () {

    $(".delete").on('click', function () {

        var email = $(this).parent().find("#Email").val();

        return confirm("Are you sure you want to completely remove " + email + " from your account?");
    });

    $(".account-manager").on('click', function () {

        var email = $(this).parent().find("#Email").val();

        return confirm("Are you sure you want to make " + email + " an admin?");
    });

    $("#TimeZoneId").change(function () {
        $(this).closest("form").submit();
    });

});