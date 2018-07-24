$.fn.remove = function () {
    $(this).click(function () {
        var code = $('#delete-modal .value-delete').val();
        $.ajax({
            type: 'post',
            url: 'project/remove.html',
            data: {
                'code': code
            },
            success: function (response) {
                window.location = "/project.html";
            },
            error: function () {
                $('#delete-modal .error-message').text('Error, EzTask cannot execute deleting data. Try again please !')
            }
        });
    });
}

function SetDeleteModalValue(project, code) {
    $('#delete-modal .source-delete').text(project);
    $('#delete-modal .value-delete').val(code);
}

$.fn.DeleteConfirm = function (){
    $(this).click(function () {
        var project = $(this).attr('data-source');
        var code = $(this).attr('data-code');
        SetDeleteModalValue('project: ' + project, code);
        $.showModal('delete-modal');
    })
}

$.fn.CancelDelete = function () {
    $(this).click(function () {
        SetDeleteModalValue('', '');
        $.closeModal('delete-modal');
    })
}


$(function () {
    $('.remove-project').DeleteConfirm();
    $('#delete-modal .btn-delete').remove();
    $('#delete-modal .close').CancelDelete();
    $.triggerCloseModal('delete-modal');
})
