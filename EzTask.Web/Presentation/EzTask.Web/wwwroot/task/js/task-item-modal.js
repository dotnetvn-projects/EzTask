function ShowAddNewModal() {
    var projectId = $('.project-list').val();
    var phraseId = $("#phrase-id").val();
    showLoading();
    DoAjax('taskitem/generate-view.html', 'POST', { projectid: projectId, phraseId:phraseId },
        function (data) {
            BuildForm(data);
            hideLoading();
            ShowModal('task-modal');
        },
        function (xhr, textStatus, errorThrown) {
            //handle exception here if any
        }
    );
}


$.fn.Submit = function () {
    $(this).click(function (e) {
        e.preventDefault();
        var form = $("#task-form");
        if (form.valid()) {
            showLoading();
            DoFormAjax(form[0],
                function (data) {
                    hideLoading();
                },
                function (xhr, textStatus, errorThrown) {
                    hideLoading();
                    //handle exception here if any
                }, 'json'
            );
        }
    });
}

function BuildForm(template) {
    $(".task-item-template").html('');
    $(".task-item-template").append(template);
    initLib();
    var form = $("#task-form");
    $.validator.unobtrusive.parse(form);
    $("#task-modal .btn-confirm").Submit();
}
