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
            async: false,
            url: 'project/invite-new-member.html',
            data: {
                projectId: $("#project-id").val(),
                accountName: $('.txt-account').val()
            },
            success: function (response) {
                $(".contacts-list").append(response);
                $.alertDialog({
                    title: 'Notification!',
                    content: 'Your invitation has been sent to <b>duylinh191@gmail.com</b>, you have to wait for accepting from this member !'
                });
            },
            error: function (xhr, ajaxOptions, thrownError) {
                $.alertDialog({
                    title: 'Error!',
                    content: xhr.responseText
                });
            }
        });
        $.hideLoading();
    });
};


$(function () {
    $(this).applyDatatable();

    $('.remove-project').deleteproject();

    $.registeriCheck('.ckstatus');

    $(".btn-add-member").showAddNewMemberModal();
    $("#add-member-modal .btn-confirm").confirmAddMember();

});
