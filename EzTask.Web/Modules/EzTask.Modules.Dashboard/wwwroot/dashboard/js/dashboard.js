$.fn.applyDatatable = function () {
    $(".task-table table").DataTable({
        "lengthMenu": [[10], [10]]
    });
};

$.fn.getTodoList = function () {
    var params = {
        currentPage: $(this).data("page")
    };

    $.PaginationRequest('dashboard/todo-list.html',this, ".todolist", params);
};

$(function () {
    $(this).applyDatatable();
    $(".pagination > li > a").getTodoList();
});
