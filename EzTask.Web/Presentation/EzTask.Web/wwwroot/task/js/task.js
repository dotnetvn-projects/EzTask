//execute call ajax to load task list
$.fn.handleLoadTask = function (projectId, phraseid) {
    $.ajax({
        url: "task/task-list.html",
        data: { projectId: projectId, phraseId: phraseid },
        success: function (response) {
            var taskListPanel = $(".task-list-panel");
            taskListPanel.html('');
            taskListPanel.html(response);
            $(this).handleEvent();
        }
    });
};

//search on task table
$.fn.searchTask = function () {
    $(this).keyup(function () {
        var input, filter, table, tr, td, i;
        input = this;
        filter = input.value.toUpperCase();
        table = $('.task-table > table')[0];
        tr = table.getElementsByTagName("tr");
        for (i = 0; i < tr.length; i++) {
            td = tr[i].getElementsByTagName("td");
            for (j = 1; j < td.length; j++) {
                var data = td[j];
                if (data) {
                    if (data.innerText.toUpperCase().indexOf(filter) > -1) {
                        tr[i].style.display = "";
                        break;
                    } else {
                        tr[i].style.display = "none";
                    }
                }
            }
        }
    });
}

//load phrase and task list when project list changed
$.fn.loadPhrase = function () {
    $(this).change(function () {
        var id = $(this).val();
        $.ajax({
            url: "task/phrase-list.html",
            data: { projectId: id },
            success: function (response) {
                var phrasePanel = $(".phrase-list-panel");
                phrasePanel.html('');
                phrasePanel.html(response);

                var phrase = $(".phrase-list > li > a").first();
                var phraseId = phrase.attr('data-id');
                $(this).handleLoadTask(id, phraseId);
                $(".phrase-list > li > a").loadTask();
            }
        });
    });
};

//handle load task event when click on item in phrase list
$.fn.loadTask = function () {
    $(this).click(function (e) {
        e.preventDefault();
        var phraseid = $(this).attr('data-id');
        var projectId = $('.project-list').val();
        $(this).handleLoadTask(projectId, phraseid);
    });
};

//refresh task list event
$.fn.refreshTask = function () {
    $(this).click(function () {
        var phraseId = $("#phrase-id").val();
        var projectId = $('.project-list').val();
        $(this).handleLoadTask(projectId, phraseId);
    });
};

//delete task event
$.fn.deleteTask = function () {
    $(this).click(function () {
        var isChecked = $(".task-table input:checkbox:checked").length > 0;
        if (isChecked) {
            //get ids
            var ids = [];
            $(".task-table input[type='checkbox']").each(function () {
                var chk = $(this);
                if (chk.prop('checked')) {
                    var id = chk.attr("data-id");
                    ids.push(id);
                }
            });

            $.confirmDialog({
                action: function () {
                    $.showLoading();
                    $.ajax({
                        type: 'post',
                        url: 'task/delete-task.html',
                        data: { taskIds: ids },
                        success: function (response) {
                            var phraseId = $("#phrase-id").val();
                            var projectId = $('.project-list').val();

                            $(this).handleLoadTask(projectId, phraseId);
                            $.closeDialog('modal-confirm');
                            $.hideLoading();
                        },
                        error: function (xhr, ajaxOptions, thrownError) {

                        }
                    });
                }
            });
        }
        else {
            //warning when don't have any items
            $.alertDialog({
                content: 'No item to delete, please select at least 1 item.'
            });
        }
    });
};

$.fn.assignTask = function () {

    $(this).click(function () {
        var isChecked = $(".task-table input:checkbox:checked").length > 0;
        if (isChecked) {

            //get ids
            var ids = [];
            $(".task-table input[type='checkbox']").each(function () {
                var chk = $(this);
                if (chk.prop('checked')) {
                    var id = chk.attr("data-id");
                    ids.push(id);
                }
            });

            var projectId = $('.project-list').val();
            $.showLoading();
            $.ajax({
                url: 'task/generate-assign-view.html',
                type: 'POST',
                data: { projectId: projectId },
                success: function (data) {
                    $(".task-template").html('');
                    $(".task-template").append(data);
                    $.initCommonLib();
                    $.hideLoading();
                    $.showDialog({
                        dialogId: 'assign-task-modal',
                        confirmAction: function () {
                            var accountid = $("#selectAssignList").val();
                            $.ajax({
                                url: 'task/assign-task.html',
                                type: 'POST',
                                data: { taskids: ids, accountId: accountid },
                                success: function (data) {
                                    var phraseId = $("#phrase-id").val();
                                    var projectId = $('.project-list').val();
                                    $(this).handleLoadTask(projectId, phraseId);
                                }
                            }).promise().done(function () {
                                $.closeDialog('assign-task-modal');
                                $.hideLoading();
                            });
                        }
                    });
                }
            });
        }
        else {
            //warning when don't have any items
            $.alertDialog({
                content: 'No item selected, please select at least 1 item.'
            });
        }
    });
};

//iCheck for checkbox and radio inputs
$.fn.registeriCheck = function () {
    $(this).iCheck({
        checkboxClass: 'icheckbox_flat-green',
        radioClass: 'iradio_flat-green'
    });
};

//Enable check and uncheck all functionality
$.fn.checkboxtoggle = function () {
    $(this).click(function () {
        var clicks = $(this).data('clicks');
        if (clicks) {
            //Uncheck all checkboxes
            $(".task-table input[type='checkbox']").iCheck("uncheck");
            $(".fa", this).removeClass("fa-check-square-o").addClass('fa-square-o');
        } else {
            //Check all checkboxes
            $(".task-table input[type='checkbox']").iCheck("check");
            $(".fa", this).removeClass("fa-square-o").addClass('fa-check-square-o');
        }
        $(this).data("clicks", !clicks);
    });
};

$.fn.handleEvent = function () {
    $('.search-task').searchTask();
    $(".checkbox-toggle").checkboxtoggle();
    $('.task-table input[type="checkbox"]').registeriCheck();
    $('.btn-addnew-task').showAddNewModal();
    $('.btn-delete-task').deleteTask();
    $('.btn-refresh-task').refreshTask();
    $('.task-list > tbody > tr').showEdit();
    $('.btn-assign-task').assignTask();
    $(".remove-phrase").removePhrase();
    $(".edit-phrase").showModal();
};

$(function () {
    $('.project-list').loadPhrase();
    $(".phrase-list > li > a").loadTask();
    $(this).handleEvent();
});