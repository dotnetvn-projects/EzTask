$.fn.deleteproject = function () {
    $(this).click(function () {
        var code = $(this).data('code');
        $.confirmDialog({
            action: function () {
                $.ajax({
                    type: 'post',
                    url: 'project/remove.html',
                    data: {
                        code: code
                    },
                    success: function (response) {
                        window.location = "/project.html";
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        $.alertDialog({
                            title: $('#alert-title').val(),
                            content: xhr.responseText
                        });
                    }
                });
            }
        });
    });
};

$.fn.applyDatatable = function () {
    $(".task-table table").DataTable();
};

$.fn.showAddNewMemberModal = function () {
    $(this).click(function () {
        $("#project-id").val($(this).data('id'));
        $.showDialog({
            dialogId: 'add-member-modal'
        });
    });
};

$.fn.confirmAddMember = function () {
    $(this).click(function () {
        $.showLoading();
        $.ajax({
            type: 'post',
            url: 'project/invite-new-member.html',
            data: {
                projectId: $("#project-id").val(),
                accountName: $('.txt-account').val()
            },
            success: function (response) {
                $(".contacts-list").append(response);
                $.hideLoading();
                var message = $('#add-member-success').val().replace('{0}', '<b>'+$('.txt-account').val()+'</b>');
                $.alertDialog({
                    title: $('#success-title').val(),
                    content: message,
                    action: function () {
                        $.closeDialog('add-member-modal');
                    }
                });
            },
            error: function (xhr, ajaxOptions, thrownError) {
                $.hideLoading();
                $.alertDialog({
                    title: $('#error-title').val(),
                    content: xhr.responseText
                });
            }
        });
    });
};

$(function () {
    $(this).applyDatatable();
    $('.remove-project').deleteproject();
    $.registeriCheck('.ckstatus');
    $(".btn-add-member").showAddNewMemberModal();
    $("#add-member-modal .btn-confirm").confirmAddMember();
});
