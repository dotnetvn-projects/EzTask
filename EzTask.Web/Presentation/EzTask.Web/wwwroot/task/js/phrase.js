
function setPhraseModalValue(projectId, phraseId) {
    $('#inputPhraseName').val('');
    $('#phrase-modal .project-id').val(projectId);
    $('#phrase-modal .phrase-id').val(phraseId);
}

function formatDate(dateString) {
    var date = new Date(dateString);
    return date.getMonth() + 1 + "/" + date.getDate() + "/" + date.getFullYear();
}

$.fn.showModal = function () {
    $(this).click(function () {
        var projectId = $('.project-list').val();
        setPhraseModalValue(projectId, 0);
        $.showDialog({
            dialogId: 'phrase-modal',
            title: 'Add new phrase',
        });
    });
};

$.fn.phraseModalAction = function () {
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
                    $("#inputStartDate").val(formatDate(response.startDate));
                    $("#inputEndDate").val(formatDate(response.endDate));
                    $.closeDialog('phrase-modal');
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    $('#phrase-modal .error-message').text('Error, EzTask cannot execute deleting data. Try again please !')
                }
            });
        }
    });
};

$(function () {
    $(".btn-addnew-phrase").showModal();
    $("#phrase-modal .btn-confirm").phraseModalAction();
    $.triggerCloseDialog('phrase-modal');
});