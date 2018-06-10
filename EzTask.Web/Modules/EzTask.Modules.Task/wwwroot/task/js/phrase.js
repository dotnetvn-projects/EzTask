
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
        SetModalTitle("phrase-modal", "Add new phrase");
        ShowModal('phrase-modal');
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
                data: $("#phrase-form").serialize(),
                success: function (response) {
                    var phrasePanel = $(".phrase-list");
                    phrasePanel.html('');
                    phrasePanel.html(response);

                    $("#inputPhraseName").val('');
                    $("#inputStartDate").val(FormatDate(response.startDate));
                    $("#inputEndDate").val(FormatDate(response.endDate));
                    CloseModal('phrase-modal');
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
    $(".btn-confirm").PhraseModalAction();
    ModalCloseTrigger('phrase-modal');
})