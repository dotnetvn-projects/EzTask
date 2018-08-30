﻿//execute call ajax to load task list
$.fn.handleLoadTask = function (projectId, phraseid) {
    $.ajax({
        url: "task/task-list.html",
        data: { projectId: projectId, phraseId: phraseid },
        success: function (response) {
            var taskListPanel = $(".task-list-panel");
            taskListPanel.html('');
            taskListPanel.html(response);
            $.handleEvent();
        }
    });
}

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
                var data = td[j]
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
                $.handleLoadTask(id, phraseId);
                $(".phrase-list > li > a").loadTask();
            }
        });
    });
};

//handle load task event when click on item in phrase list
$.fn.loadTask = function () {
    $(this).click(function () {
        var phraseid = $(this).attr('data-id');
        var projectId = $('.project-list').val();
        $.handleLoadTask(projectId, phraseid);
    });
}

//refresh task list event
$.fn.refreshTask = function () {
    $(this).click(function () {
        var phraseId = $("#phrase-id").val();
        var projectId = $('.project-list').val();
        $.handleLoadTask(projectId, phraseId);
    });
}

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

            //ask for delete
            $.showDialog({
                dialogId: 'modal-confirm',
                content: 'Do you wanna delete ?',
                confirmAction: function () {
                    $.showLoading();
                    $.ajax({
                        type: 'post',
                        url: 'task/delete-task.html',
                        data: { taskIds: ids },
                        success: function (response) {
                            var phraseId = $("#phrase-id").val();
                            var projectId = $('.project-list').val();

                            $.handleLoadTask(projectId, phraseId);
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
            $.showDialog({
                dialogId: 'modal-warning',
                content: 'No items to delete, please select at least 1 item.'
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
}

$(function () {
    $('.project-list').loadPhrase();
    $(this).handleEvent();
});