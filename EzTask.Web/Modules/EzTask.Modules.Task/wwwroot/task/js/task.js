﻿//execute call ajax to load task list
$.fn.handleLoadTask = function (projectId, phaseid, doneAction, allowAsync = true) {
    $.ajax({
        url: "task/task-list.html",
        async: allowAsync,
        data: { projectId: projectId, phaseId: phaseid },
        success: function (response) {
            var taskListPanel = $(".task-list-panel");
            taskListPanel.html('');
            taskListPanel.html(response);
            $(this).handleEvent();
            if (doneAction !== null)
                doneAction();
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
};

var currentPhaseId = 0;
//load phase and task list when project list changed
$.fn.loadPhase = function () {
    $(this).change(function () {
        $.showLoading();
        var id = $(this).val();
        $.ajax({
            url: "phase/phase-list.html",
            data: { projectId: id },
            success: function (response, status, request) {
                var phasePanel = $(".phase-list-panel");
                phasePanel.html('');
                phasePanel.html(response);
                var phase = $(".phase-list > li > a").first();
                var phaseId = phase.attr('data-id');
                if (currentPhaseId !== null && currentPhaseId > 0) {
                    phaseId = currentPhaseId;
                }

                if (phaseId !== 0 && phaseId !== undefined) {
                    $(this).handleLoadTask(id, phaseId, null, false);
                    if (phase.length > 0) {
                        $(".phase-list > li > a").loadTask();
                    }
                }
                else {
                    $(this).handleLoadTask(id, 0, null, false);
                }
                var authorizeAdd = request.getResponseHeader("authorized-add-phase");

                if (authorizeAdd === "authorized") {
                    if ($(".btn-addnew-phase").length <= 0) {
                        $.post('phase/generate-addbutton.html', function (res) {
                            $(".project-box").append(res);
                            $(".btn-addnew-phase").showModal();
                            $.hideLoading();
                        });
                    }
                    else {
                        $.hideLoading();
                    }
                }
                else {
                    $(".btn-addnew-phase").remove();
                    $.hideLoading();
                }
                currentPhaseId = 0;
            }
        });
    });
};

//handle load task event when click on item in phase list
$.fn.loadTask = function () {
    $(this).click(function (e) {
        e.preventDefault();

        var phaseid = $(this).attr('data-id');
        var projectId = $('.project-list').val();

        $.showLoading();
        
        $(this).handleLoadTask(projectId, phaseid, function () {
            $.hideLoading();
        });
    });
};

//refresh task list event
$.fn.refreshTask = function () {
    $(this).click(function () {
        var phaseId = $("#phase-id").val();
        var projectId = $('.project-list').val();
        $(this).handleLoadTask(projectId, phaseId);
    });
};

//delete task event
$.fn.deleteTask = function () {
    $(this).click(function () {
        var projectId = $('.project-list').val();
        var phaseId = $("#phase-id").val();

        if (projectId > 0 && phaseId > 0) {
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
                    title: $('#warning-title').val(),
                    content: $('#delete-task-warning').val(),
                    action: function () {
                        $.showLoading();
                        $.ajax({
                            type: 'post',
                            url: 'task/delete-task.html',
                            data: {
                                taskIds: ids,
                                projectId: projectId
                            },
                            success: function (response) {
                                $(this).handleLoadTask(projectId, phaseId);
                                $.hideLoading();
                            },
                            error: function (xhr, ajaxOptions, thrownError) {
                                $.hideLoading();
                                $.alertDialog({
                                    title: $('#error-title').val(),
                                    content: xhr.responseText
                                });
                            }
                        });
                    }
                });
            }
            else {
                //warning when don't have any items
                $.alertDialog({
                    title: $('#alert-title').val(),
                    content: $('#no-task-selected-delete-warning').val()
                });
            }
        }
    });
};

$.fn.assignTask = function () {
    $(this).click(function () {
        var projectId = $('.project-list').val();
        var phaseId = $("#phase-id").val();

        if (projectId > 0 && phaseId > 0) {
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

                $.showLoading();
                $.ajax({
                    url: 'task/generate-assign-view.html',
                    type: 'POST',
                    data: { projectId: projectId },
                    success: function (data) {
                        if (data !== "") {
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
                                            $(this).handleLoadTask(projectId, phaseId);
                                        },
                                        error: function (xhr, ajaxOptions, thrownError) {
                                            $.alertDialog({
                                                title: $('#error-title').val(),
                                                content: xhr.responseText
                                            });
                                        }
                                    }).promise().done(function () {
                                        $.closeDialog('assign-task-modal');
                                        $.hideLoading();
                                    });
                                }
                            });
                        }
                    }
                });
            }
            else {
                //warning when don't have any items
                $.alertDialog({
                    title: $('#alert-title').val(),
                    content: $('#no-task-selected-warning').val()
                });
            }
        }
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
    $.registeriCheck('.task-table input[type="checkbox"]');
    $('.btn-addnew-task').showAddNewModal();
    $('.btn-delete-task').deleteTask();
    $('.btn-refresh-task').refreshTask();
    $('.task-list > tbody > tr').showEdit();
    $('.btn-assign-task').assignTask();
    $(".remove-phase").removePhase();
    $(".edit-phase").showModal();
};

$.fn.showDetailFromCode = function () {
    var taskCode = $.queryString()["code"];
    if (taskCode !== undefined && taskCode !== '' && taskCode !== null) {
        $.ajax({
            url: "task/validate-code.html",
            type: 'POST',
            data: { code: taskCode },
            success: function (response, status, request) {
                $.showLoading();
                $(this).handleLoadTask(response.projectId, response.phaseId, function () {
                    $("#phase-id").val(response.phaseId);
                    $('.project-list').val(response.projectId);
                    $.hideLoading();
                    $('.task-list > tbody > tr[data-code="' + taskCode + '"]').trigger("click");
                });
            },
            error: function (xhr, ajaxOptions, thrownError) {
                window.location.href = '/';
            }
        });
    }
};

$(function () {
    $('.project-list').loadPhase();
    $(".phase-list > li > a").loadTask();
    $(this).handleEvent();
    $(this).showDetailFromCode();
});