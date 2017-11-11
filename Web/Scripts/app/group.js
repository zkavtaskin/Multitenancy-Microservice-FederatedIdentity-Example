
$(document).ready(function () {

    $(".delete").on('click', function () {

        var groupName = $(this).closest("form").find("#Name").val();

        return confirm("Are you sure you want to remove " + groupName + " from production lines?");
    });

});