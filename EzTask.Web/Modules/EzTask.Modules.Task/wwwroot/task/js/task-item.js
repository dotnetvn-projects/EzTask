$.fn.ShowAddNewModal = function () {
    $(this).click(function () {
        var projectId = $('.project-list').val();
        showLoading();
        DoAjax('taskitem/generate-new-item.html', 'POST', { projectId: projectId },
            function (data) {
                BuildForm(data);
                hideLoading();
                ShowModal('task-modal');
            },
            function (xhr, textStatus, errorThrown) {
                //handle exception here if any
            }
        );           
    });
}

function BuildForm(template) {
    $(".task-item-template").html('');
    $(".task-item-template").append(template);
    initLib();
}

$(function () {
    $(".btn-addnew-task").ShowAddNewModal();
})