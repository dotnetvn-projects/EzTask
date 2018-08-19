
function BuildForm(template) {
    $(".task-item-template").html('');
    $(".task-item-template").append(template);
    $.initCommonLib();
    var form = $("#task-form");
    $.validator.unobtrusive.parse(form);
    $("#task-modal .btn-confirm").Submit();
}

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
            $.showDialog({
                dialogId: 'task-modal'
            });
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
                url: form.attr('action'),
                type: form.attr('method'),
                dataType: 'json',
                data: $(form).serialize(),
                success: function (response) {
                    submitSuccess(response);
                },
                error: function (xhr, textStatus, errorThrown) {

                }
            });
        }
    });
}

function submitSuccess(response) {
    var currentId = $("#TaskId").val();
    if (currentId <= 0) {
        var attachmentTab = "<li class=''><a href='#tab_attachment' data-toggle='tab' aria-expanded='false'>Attachment</a></li>";
        var historyTab = "<li class=''><a href='#tab_history' data-toggle='tab' aria-expanded='false'>History</a></li>";
        $(".nav-tabs-custom ul").append(attachmentTab).append(historyTab);

        var attachmentContent = "<div class='tab-pane' id='tab_attachment'></div>";
        var historyContent = "<div class='tab-pane' id='tab_history'></div>";

        $(".tab-content").append(attachmentContent).append(historyContent);

        currentId = response.data.taskId;
        $.ajax({
            url: 'taskitem/attachment-list.html',
            type: "POST",
            async: false,
            data: { taskId: currentId },
            success: function (data) {
                $('#tab_attachment').html(data);
            }
        });

        $.ajax({
            url: 'taskitem/history-list.html',
            type: "POST",
            async: false,
            data: { taskId: currentId },
            success: function (data) {
                $('#tab_history').html(data);
            }
        });
    }

    $("#TaskId").val(currentId);
    $("#task-modal .modal-title").html("Task '<b>" + response.data.taskTitle + "</b>'");
    $.hideLoading();
    refreshTask();
}
