

$(document).ready(function () {

    $('#settings').hide();

    $('#identity').on('click', function () {
        $('#settings').slideToggle(500);
    });

    $(document.body).on('click', ".edit", function () {
        $(this).find('.actions').fadeToggle(200);
    });

})
