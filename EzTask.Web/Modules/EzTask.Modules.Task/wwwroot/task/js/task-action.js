$.fn.ShowAddNewModal = function () {
    $(this).click(function () {
        ShowModal('task-modal');
    });
}


$(function () {
    $(".btn-addnew-task").ShowAddNewModal();
})