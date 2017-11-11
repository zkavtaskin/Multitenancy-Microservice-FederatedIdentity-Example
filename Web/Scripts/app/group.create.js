
$(document).ready(function () {

    var mustacheTemplates = '';

    $.get('/Scripts/app/search.mustache.html',
        function (template, textStatus, jqXhr) {
            mustacheTemplates = template;
        }
    );

    $("#Invite").focus(function () {
        $(".ui-autocomplete").css({ "display": "block !important" });
    });

    $("#Invite").focusout(function () {
        $(".ui-autocomplete").css({ "display": "none" });
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

                    var dataFiltered = [];

                    $.each(data, function (index, value) {
                        if ($("#Invited").data()[value.Email] == null) {
                            if (value.Initials != null) {
                                value.InitialFirstLetter = value.Initials.toLowerCase()[0];
                            }
                            dataFiltered.push(value);
                       }
                    });

                    response(dataFiltered);
                }
            });
        },

        select: function (event, ui) {
            if (ui.item.Email != null) {
                addToInviteList(ui.item.Email, ui.item)
                $("#Invite").val('');
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
            addToInviteList(value, {});
            return false;
        }
    });

    $("#groupUsers").width($("#Invite").outerWidth());

    function addToInviteList(value, data) {

        $("#Invited").val($("#Invited").val() + value + ";");

        if ($("#Invited").data() == null) {
            var data = [value];
            $("#Invited").data(data);
        } else {
            $("#Invited").data()[value] = value;
        }

        var ul = $("#groupUsers");
        if (data.Email == null) {
            var obj = { value: value };
            $($.parseHTML(Mustache.render($(mustacheTemplates).filter("#user_inviting").html(), obj))).appendTo(ul);
            return;
        }

        if (data.IsInvited) {
            $($.parseHTML(Mustache.render($(mustacheTemplates).filter("#user_invited").html(), data))).appendTo(ul);
            return;
        }

        $($.parseHTML(Mustache.render($(mustacheTemplates).filter("#user_member").html(), data))).appendTo(ul);
    }

    $("#groupUsers").delegate("#delete", "click", function (event) {
        var card = $(this).closest(".card");
        var email = card.find("#email").val();
        
        var invites = $("#Invited").val();
        $("#Invited").val(invites.replace(email + ';', ''));
        $("#Invited").data()[email] = null;

        $(card).remove();
    });
});