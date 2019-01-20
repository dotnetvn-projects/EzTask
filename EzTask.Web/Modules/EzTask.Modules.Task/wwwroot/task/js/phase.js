
function formatDate(dateString) {
    var date = new Date(dateString);
    return date.getMonth() + 1 + "/" + date.getDate() + "/" + date.getFullYear();
}

$.fn.showModal = function () {
    $(this).click(function () {
        var phaseid = 0;
        var projectid = $('.project-list').val();
        if ($(this).hasClass("edit-phase")) {
            phaseid = $("#phase-id").val();
        }

        $.showLoading();
        $.ajax({
            url: 'phase/generate-phase.html',
            type: "POST",
            data: { phaseId: phaseid, projectId: projectid },
            success: function (data) {
                $(".phase-template").html(data);

                $.initCommonLib();
                $("#phase-modal .btn-confirm").phaseModalAction();
                $.hideLoading();

                $.showDialog({
                    dialogId: 'phase-modal'
                });
                $.triggerCloseDialog('phase-modal');
            }
        });
             
    });
};

$.fn.phaseModalAction = function () {
    $(this).click(function (e) {
        e.preventDefault();
        var form = $("#phase-form");
        $.showLoading();
        if (form.valid()) {
            $.ajax({
                type: 'post',
                url: "phase/phase-modal-action.html",
                data: form.serialize(),
                success: function (response) {                  
                    var phasePanel = $(".phase-list-panel");
                    phasePanel.html('');
                    phasePanel.html(response);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    $.alertDialog({
                        title: 'Error',
                        content: xhr.responseText
                    });
                }
            }).promise().done(function () {
                $.closeDialog('phase-modal');
                $(".phase-list > li > a").loadTask();
                $.hideLoading();
            });
        }
    });
};

$.fn.removePhase = function () {
    $(this).click(function () {
        $.confirmDialog({
            title: 'Warning',
            content: 'All tasks which related to this phase will be removed. Are you sure to continue?',
            action: function () {
                $.showLoading();
                $.ajax({
                    type: 'post',
                    url: "phase/delete-phase.html",
                    data: { phaseId: $("#phase-id").val()},
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
    $(".btn-addnew-phase").showModal();
});