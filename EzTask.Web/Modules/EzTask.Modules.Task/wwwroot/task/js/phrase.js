
function setPhraseModalValue(projectId, phraseId) {
    var date = new Date();
    $('#inputPhraseName').val('');
    $('#phrase-modal .project-id').val(projectId);
    $('#phrase-modal .phrase-id').val(phraseId);
    $('#phrase-modal #inputStartDate').val(date.getDate() + "/" + (date.getMonth() + 1) + "/" + date.getFullYear());
    $('#phrase-modal #inputEndDate').val((date.getDate() + 1) + "/" + (date.getMonth() + 1) + "/" + date.getFullYear());
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
            title: 'Add new phrase'
        });
    });
};

$.fn.phraseModalAction = function () {
    $(this).click(function (e) {
        e.preventDefault();
        var form = $("#phrase-form");

        if (form.valid()) {
            $.ajax({
                type: 'post',
                url: "task/phrase-modal-action.html",
                data: form.serialize(),
                success: function (response) {
                    var phrasePanel = $(".phrase-list-panel");
                    phrasePanel.html('');
                    phrasePanel.html(response);
                    $(".phrase-list > li > a").loadTask();
                    $.closeDialog('phrase-modal');
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    $('#phrase-modal .error-message').text('Error, EzTask cannot create phrase. Try again please !');
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