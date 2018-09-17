
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

        $.showLoading();
        $.ajax({
            url: 'task/generate-phrase.html',
            type: "POST",
            async: false,
            data: { phraseId: phraseid },
            success: function (data) {
                $(".phrase-template").html('');
                $(".phrase-template").append(data);
                
                $.hideLoading();
                $.showDialog({
                    dialogId: 'phrase-modal'
                });
            }
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
                    $(".phrase-template").html('');
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
    $("#phrase-modal .btn-confirm").phraseModalAction();
    $.triggerCloseDialog('phrase-modal');
});