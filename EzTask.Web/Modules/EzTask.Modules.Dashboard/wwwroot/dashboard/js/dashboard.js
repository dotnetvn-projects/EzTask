$.fn.applyDatatable = function () {
    $(".task-table table").DataTable({
        "lengthMenu": [[10], [10]]
    });
};

$.fn.getTodoList = function () {
    $(this).click(function (e) {
        e.preventDefault();
        var params = {
            currentPage: $(this).data("page")
        };
        var target = $(".todolist");
        $.PaginationRequest('dashboard/todo-list.html', target, params, function () {
            $(this).initJs();
        });
    });   
};

$.fn.removeTodoList = function () {
    $(this).click(function (e) {
        e.preventDefault();
        var todoItem = [];
        if ($(this).hasClass('remove-todoitems')) {
            $('input.todo-check[type=checkbox]').each(function () {
                if (this.checked)
                    todoItem.push($(this).data("id"));
            });
        }
        else {
            todoItem.push($(this).data("id"));
        }

        if (todoItem.length > 0) {
            $.ajax({
                url: 'dashboard/remove-todo-list.html',
                data: { items: todoItem },
                type: "POST",
                success: function (reponse) {
                    if (reponse.data === true) {
                        $(".pagination > li.active > a").trigger('click');
                    }                
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    $.alertDialog({
                        title: $('#error-title').val(),
                        content: xhr.responseText
                    });
                }
            });
        } else {
            $.alertDialog({
                title: $('#warning-title').val(),
                content: $("#delete-todolist-warning").val()
            });
        }
    });
};

$.fn.initJs = function applyJs() {
    $(".pagination > li > a").getTodoList();
    $(".remove-todoitem,.remove-todoitems").removeTodoList();
};

$(function () {
    $(this).applyDatatable();
    $(this).initJs();
});
