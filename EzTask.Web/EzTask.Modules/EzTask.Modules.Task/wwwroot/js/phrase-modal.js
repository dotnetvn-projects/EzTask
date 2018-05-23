
function SetDeleteModalValue(projectId, phraseId) {
    $('#phrase-modal .project-id').val(projectId);
    $('#phrase-modal .phrase-id').val(phraseId);
}

$.fn.ShowModal = function () {
    $(this).click(function () {
        var projectId = $('.project-list').val();
        SetDeleteModalValue(projectId, 0);
        SetModalTitle("phrase-modal", "Add new phrase");
        ShowModal('phrase-modal');
    });
}

$.fn.PhraseModalAction = function () {
    $(this).click(function (e) {
        e.preventDefault();
        var t = $("#phrase-form").validate();
        var c = t.settings.messages;
        var projectId = $('.project-list').val();
        if ($("#phrase-form").valid()) {
            $.ajax({
                type: 'post',
                url: "task/phrase-modal-action.html",
                data: $("#phrase-form").serialize(),
                success: function (response) {
                    var phrasePanel = $(".phrase-list");
                    var item = '<li><a href="#"><i class="fa fa-building-o"></i> ' + response.phraseName + '<span class="label label-warning pull-right">65</span></a></li>';
                    phrasePanel.append(item);
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