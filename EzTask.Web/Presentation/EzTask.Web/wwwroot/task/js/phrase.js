
function formatDate(dateString) {
    var date = new Date(dateString);
    return date.getMonth() + 1 + "/" + date.getDate() + "/" + date.getFullYear();
}

$.fn.showModal = function () {
    $(this).click(function () {
        var phraseid = 0;
        var projectid = $('.project-list').val();
        if ($(this).hasClass("edit-phrase")) {
            phraseid = $("#phrase-id").val();
        }

        $.showLoading();
        $.ajax({
            url: 'phase/generate-phase.html',
            type: "POST",
            data: { phraseId: phraseid, projectId: projectid },
            success: function (data) {
                $(".phrase-template").html(data);

                $.initCommonLib();
                $("#phrase-modal .btn-confirm").phraseModalAction();
                $.hideLoading();

                $.showDialog({
                    dialogId: 'phrase-modal'
                });
                $.triggerCloseDialog('phrase-modal');
            }
        });
             
    });
};

$.fn.phraseModalAction = function () {
    $(this).click(function (e) {
        e.preventDefault();
        var form = $("#phrase-form");
        $.showLoading();
        if (form.valid()) {
            $.ajax({
                type: 'post',
                url: "phase/phase-modal-action.html",
                data: form.serialize(),
                success: function (response) {                  
                    var phrasePanel = $(".phrase-list-panel");
                    phrasePanel.html('');
                    phrasePanel.html(response);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    $.alertDialog({
                        title: 'Error',
                        content: xhr.responseText
                    });
                }
            }).promise().done(function () {
                $.closeDialog('phrase-modal');
                $(".phrase-list > li > a").loadTask();
                $.hideLoading();
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
                    url: "phase/delete-phase.html",
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
});