$.fn.applyDatatable = function () {
    $(".task-table table").DataTable({
        "lengthMenu": [[10], [10]]
    });
};

$(function () {
    $(this).applyDatatable();
});
