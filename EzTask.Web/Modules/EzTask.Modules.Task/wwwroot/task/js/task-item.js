
$.fn.buildForm = function (template) {
    $(".task-item-template").html('');
    $(".task-item-template").append(template);  
    var form = $("#task-form");
    $.initCommonLib();
    $.validator.unobtrusive.parse(form);
    $("#task-modal .btn-confirm").Submit();
    $("#file-upload").putAttachment();
};

$.fn.showAddNewModal = function () {
    $(this).click(function () {
        var projectId = $('.project-list').val();
        var phraseId = $("#phrase-id").val();
        $.showLoading();
        $.ajax({
            url: 'taskitem/generate-view.html',
            type: 'POST',
            data: { projectId: projectId, phraseId: phraseId },
            success: function (data) {
                $(this).buildForm(data);
                $.hideLoading();
                $.showDialog({
                    dialogId: 'task-modal'
                });
            }
        });
    });
};

$.fn.putAttachment = function () {
    $(this).change(function () {
        $.showLoading();

        var ajax = new XMLHttpRequest();

        var file = $("#file-upload")[0].files[0];
        var formData = new FormData();
        formData.append("file", file);
        formData.append("taskId", $("#TaskId").val());

        ajax.onreadystatechange = function () {
            if (ajax.status) {
                if (ajax.status === 200 && ajax.readyState === 4) {
                    $(this).loadAttachment($("#TaskId").val());
                    $(this).loadHistory($("#TaskId").val());
                    $.hideLoading();
                }
            }
        };

        ajax.upload.addEventListener("progress", function (event) {
            var percent = (event.loaded / event.total) * 100;        
            console.log(percent);
        });

        ajax.open("POST", 'taskitem/upload-attach-file.html', true);
        ajax.send(formData);
    });
};

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
                success: submitSuccess,
                error: function (xhr, textStatus, errorThrown) {

                }
            });
        }
    });
};

$.fn.displayHistoryDetail = function () {
    $(this).click(function (e) {
        e.preventDefault();
        $.showLoading();
        var id = $(this).data('id');
        $.ajax({
            url: 'taskitem/history-detail.html',
            type: "POST",
            async: false,
            data: { historyId: id },
            success: function (data) {
                $(".task-history-template").html('');
                $(".task-history-template").append(data);  
                $.showDialog({
                    dialogId: 'task-history-detail'
                });
                $.hideLoading();
            }
        });
    });
};

$.fn.loadAttachment = function (taskId) {
    $.ajax({
        url: 'taskitem/attachment-list.html',
        type: "POST",
        async: false,
        data: { taskId: taskId },
        success: function (data) {
            $('#tab_attachment').html(data);
            $("#file-upload").putAttachment();
        }
    });
};

$.fn.loadHistory = function (taskId) {
    $.ajax({
        url: 'taskitem/history-list.html',
        type: "POST",
        async: false,
        data: { taskId: taskId },
        success: function (data) {
            $('#tab_history').html(data);
            $('.history-detail').displayHistoryDetail();
        }
    });
};

$.fn.showEdit = function () {
    $(this).click(function () {
        var taskId = $(this).data('id');
        $.showLoading();
        $.ajax({
            url: 'taskitem/generate-view.html',
            type: 'POST',
            data: { taskId: taskId },
            success: function (data) {
                $(this).buildForm(data);
                $.hideLoading();
                $.showDialog({
                    dialogId: 'task-modal'
                });
            }
        });
    });
};

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

        $(this).loadAttachment(currentId);
        $(this).loadHistory(currentId);

        $("#TaskId").val(currentId);
       
    }

    $("#task-modal .modal-title").html("Task '<b>" + response.data.taskTitle + "</b>'");

    var phraseId = $("#phrase-id").val();
    var projectId = $('.project-list').val();

    $(this).handleLoadTask(projectId, phraseId);

    $.hideLoading();
}


$(function () {
    $('.history-detail').displayHistoryDetail();
    $("#file-upload").putAttachment();
});
