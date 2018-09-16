
function setPhraseModalValue(projectId, phraseId) {
    $('#phrase-modal .project-id').val(projectId);
    $('#phrase-modal .phrase-id').val(phraseId);

    if (phraseId === 0) {
        $(".phrase-status-form").hide();
        var date = new Date();
        $('#inputPhraseName').val('');
        $('#phrase-modal #inputStartDate').val(date.getDate() + "/" + (date.getMonth() + 1) + "/" + date.getFullYear());
        $('#phrase-modal #inputEndDate').val((date.getDate() + 1) + "/" + (date.getMonth() + 1) + "/" + date.getFullYear());
    }
    else {
        $(".phrase-status-form").show();
        $('#inputPhraseName').val($("#phrase-name").val());
        $('#phrase-modal #inputStartDate').val($("#phrase-startdate").val());
        $('#phrase-modal #inputEndDate').val($("#phrase-enddate").val());
    }
}

function formatDate(dateString) {
    var date = new Date(dateString);
    return date.getMonth() + 1 + "/" + date.getDate() + "/" + date.getFullYear();
}

$.fn.showModal = function () {
    $(this).click(function () {
        var phraseid = 0;

        if ($(this).hasClass("edit-phrase")) {
            phraseid = $("#phrase-id").val();
        }

        var projectId = $('.project-list').val();
        setPhraseModalValue(projectId, phraseid);
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

$.fn.removePhrase = function () {
    $(this).click(function () {
        $.confirmDialog({
            title: 'Warning',
            content: 'All tasks which related to this phrase will be removed. Are you sure to continue?',
            action: function () {
                $.showLoading();
                $.ajax({
                    type: 'post',
                    url: "task/delete-phrase.html",
                    data: { phraseId: $("#phrase-id").val()},
                    success: function (response) {
                        $('.project-list').change();
                        $.hideLoading();
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        $.hideLoading();

                        $.alertDialog({
                            title: 'Error',
                            content: xhr.responseText
                        });
                    }
                });
            }
        });
    });
};

$(function () {
    $(".btn-addnew-phrase").showModal();
    $(".edit-phrase").showModal();
    $("#phrase-modal .btn-confirm").phraseModalAction();
    $.triggerCloseDialog('phrase-modal');
    $(".remove-phrase").removePhrase();
});