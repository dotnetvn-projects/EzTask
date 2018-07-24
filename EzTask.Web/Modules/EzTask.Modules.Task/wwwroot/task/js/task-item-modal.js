function ShowAddNewModal() {
    var projectId = $('.project-list').val();
    var phraseId = $("#phrase-id").val();
    $.showLoading();
    $.ajax({
        url: 'taskitem/generate-view.html',
        type: 'POST',
        data: { projectid: projectId, phraseId: phraseId },
        success: function (data) {
            BuildForm(data);
            $.hideLoading();
            $.showModal('task-modal');
        }
    });
}

$.fn.Submit = function () {
    $(this).click(function (e) {
        e.preventDefault();
        var form = $("#task-form");
        if (form.valid()) {
            $.showLoading();
            $.ajax({
                url: form.action,
                type: form.method,
                dataType: 'json',
                data: $(form).serialize(),
                success: function (data) {
                    $.hideLoading();
                    $.closeModal("task-modal");
                },
                error: function (xhr, textStatus, errorThrown) {
                }
            });
        }
    });
}

function BuildForm(template) {
    $(".task-item-template").html('');
    $(".task-item-template").append(template);
    $.initCommonLib();
    var form = $("#task-form");
    $.validator.unobtrusive.parse(form);
    $("#task-modal .btn-confirm").Submit();
}
