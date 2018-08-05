
function SetPhraseModalValue(projectId, phraseId) {
    $('#inputPhraseName').val('');
    $('#phrase-modal .project-id').val(projectId);
    $('#phrase-modal .phrase-id').val(phraseId);
}

function FormatDate(dateString) {
    var date = new Date(dateString);
    return (date.getMonth() +1) + "/" + date.getDate() + "/" + date.getFullYear();
}

$.fn.ShowModal = function () {
    $(this).click(function () {
        var projectId = $('.project-list').val();
        SetPhraseModalValue(projectId, 0);
        $.showDialog('phrase-modal', 'Add new phrase');
    });
}

$.fn.PhraseModalAction = function () {
    $(this).click(function (e) {
        e.preventDefault();
        var form = $("#phrase-form");
        var projectId = $('.project-list').val();
        var phraseId = $('.phrase-id').val();

        if (form.valid()) {
            $.ajax({
                type: 'post',
                url: "task/phrase-modal-action.html",
                data: form.serialize(),
                success: function (response) {
                    var phrasePanel = $(".phrase-list-panel");
                    phrasePanel.html('');
                    phrasePanel.html(response);

                    $("#inputPhraseName").val('');
                    $("#inputStartDate").val(FormatDate(response.startDate));
                    $("#inputEndDate").val(FormatDate(response.endDate));
                    $.closeDialog('phrase-modal');
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    $('#phrase-modal .error-message').text('Error, EzTask cannot execute deleting data. Try again please !')
                }
            });
        }
    });
}

$(function () {
    $(".btn-addnew-phrase").ShowModal();
    $("#phrase-modal .btn-confirm").PhraseModalAction();
    $.triggerCloseDialog('phrase-modal');
})