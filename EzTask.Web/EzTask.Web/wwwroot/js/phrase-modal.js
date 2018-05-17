$(function () {

    function SetDeleteModalValue(projectId, phraseId) {
        $('#phrase-modal .project-id').val(projectId);
        $('#phrase-modal .phrase-id').val(phraseId);
    }

    $(".btn-addnew-phrase").click(function () {
        var projectId = $('.project-list').val();
        SetDeleteModalValue(projectId, 0);
        SetModalTitle("phrase-modal", "Add new phrase");
        ShowModal('phrase-modal');
    });
    //function hijack(form, callback, errorFunction, format) {
    //    $.ajax({
    //        url: form.action,
    //        type: form.method,
    //        dataType: format,
    //        data: $(form).serialize(),
    //        success: callback,
    //        error: function (xhr, textStatus, errorThrown) {
    //            errorFunction(xhr, textStatus, errorThrown);
    //        }
    //    });
    //}
    $(".btn-confirm").click(function (e) {
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
                    window.location = "/project.html";
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    $('#phrase-modal .error-message').text('Error, EzTask cannot execute deleting data. Try again please !')
                }
            });
        }        
    });
});