
$(document).ready(function () {

    var mustacheTemplates = '';

    $.get('/Scripts/app/search.mustache.html',
        function (template, textStatus, jqXhr) {
            mustacheTemplates = template;
        }
    );

    var existingEmails = [];

    $(".actions").each(function (index, value) {
        var email = $(value).find("#Email").val();
        existingEmails[email] = email;
    });

    $("#Invite").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: urlLineSearchDataSource,
                dataType: "json",
                data: {
                    term: request.term.toLowerCase()
                },
                success: function (data) {

                    var filteredEmails = [];

                    $.each(data, function (index, value) {

                        if (existingEmails[value.Email] == null) {

                            if (value.Initials != null) {
                                value.InitialFirstLetter = value.Initials.toLowerCase()[0];
                            }

                            filteredEmails.push(value);
                        }

                    });
                    response(filteredEmails);
                }
            });
        },

        select: function (event, ui) {
            if (ui.item.Email != null) {
                $("#groupUserInviteModel_Email").val(ui.item.Email);
                $("#addnewuser").submit();
                return false;
            }
        }
    });

    $("#Invite").autocomplete("instance")._renderItem = function (ul, data) {
        if (data.Email == null) {
            var obj =  { value: $("#Invite").val() };
            return $($.parseHTML(Mustache.render($(mustacheTemplates).filter("#user_doesnt_exist").html(), obj))).appendTo(ul);
        }

        if (data.IsInvited) {
            return $($.parseHTML(Mustache.render($(mustacheTemplates).filter("#user_invited").html(), data))).appendTo(ul);
        }

        return $($.parseHTML(Mustache.render($(mustacheTemplates).filter("#user_member").html(), data))).appendTo(ul);
    }

    $("#Invite").autocomplete("instance")._resizeMenu = function() {
        this.menu.element.outerWidth( $("#Invite").outerWidth() );
    }

    $("#Invite").keypress(function (e) {
        if (e.which == 13) {
            var value = $("#Invite").val();
            $("#Invite").val('');
            $("#groupUserInviteModel_Email").val(value);
            return true;
        }
    });


    $("#addnew").on('click', function () {
        $('#addnew').fadeToggle(0);
        $('#addnewuser').fadeToggle(200);
        $('#addnewuser input').focus();
    });

    $(".delete").on('click', function () {

        var email = $(this).parent().find("#Email").val();
        var groupName = $("h1").html();

        return confirm("Are you sure you want to remove " + email + " from " + groupName + " ?");
    });

});