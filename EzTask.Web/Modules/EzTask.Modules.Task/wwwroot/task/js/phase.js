﻿
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

                var form = $("#phase-form");
                $.initCommonLib();
                $.validator.unobtrusive.parse(form);

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
        if (form.valid()) {
            $.showLoading();
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
                    $.hideLoading();
                    $.alertDialog({
                        title: $('#error-title').val(),
                        content: xhr.responseText
                    });
                }
            }).promise().done(function () {
                $.closeDialog('phase-modal');
                $(".phase-list > li > a").loadTask();
                var phaseid = $("#phase-id").val();
                if (phaseid > 0) {
                    $(".phase-list > li > a[data-id=" + phaseid + "]").click();
                }
                $.hideLoading();
            });
        }
    });
};

$.fn.removePhase = function () {
    $(this).click(function () {
        $.confirmDialog({
            title: $('#warning-title').val(),
            content: $('#remove-phase-warning').val(),
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
                            title: $('#error-title').val(),
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