// load todo list base on page number
function loadTodoList(page) {
    var params = {
        currentPage: page
    };
    var target = $(".todolist");
    $.PaginationRequest('dashboard/todo-list.html', target, params, function () {
        $(this).initJs();
    });
}

// apply datatablejs
$.fn.applyDatatable = function () {
    $(".task-table table").DataTable({
        "lengthMenu": [[10], [10]]
    });
};

// get todo list
$.fn.getTodoList = function () {
    $(this).click(function (e) {
        loadTodoList($(this).data("page"));
    });   
};

// show add or edit modal
$.fn.showModal = function () {
    $(this).click(function (e) {
        var itemId = $(this).data('itemid');
        $.showLoading();
        $.ajax({
            url: 'dashboard/todolist/generateview.html',
            data: { itemId: itemId },
            type: "POST",
            success: function (reponse) {
                $(".view-template").html(reponse);

                $.initCommonLib();

                $("#todoitem-modal .btn-confirm").todoItemModalAction();

                $.showDialog({
                    dialogId: 'todoitem-modal'
                });

                $.triggerCloseDialog('todoitem-modal');
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
    });
};

// excute add or edit todo item
$.fn.todoItemModalAction = function () {
    $(this).click(function (e) {
        e.preventDefault();
        var form = $("#todoItem-form");
        $.showLoading();
        if (form.valid()) {
            $.ajax({
                type: 'POST',
                url: "dashboard/todolist/save.html",
                data: form.serialize(),
                success: function (response) {
                    $.closeDialog('todoitem-modal');
                    loadTodoList(1);
                    $.hideLoading();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    $.alertDialog({
                        title: $('#error-title').val(),
                        content: xhr.responseText
                    });
                    $.hideLoading();
                }
            });
        }
    });  
};

// remove item from todo list
$.fn.removeTodoList = function () {
    $(this).click(function (e) {
        e.preventDefault();
        var todoItem = [];
        if ($(this).hasClass('remove-todoitems')) {
            $('input.todo-check[type=checkbox]').each(function () {
                if (this.checked)
                    todoItem.push($(this).data("itemid"));
            });
        }
        else {
            todoItem.push($(this).data("itemid"));
        }

        if (todoItem.length > 0) {
            $.confirmDialog({
                title: $('#warning-title').val(),
                content: $('#delete-todoitem-warning').val(),
                action: function () {
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
                }
            });

            
        } else {
            $.alertDialog({
                title: $('#warning-title').val(),
                content: $("#no-todoitem-seleted-warning").val()
            });
        }
    });
};

// apply common js
$.fn.initJs = function applyJs() {
    $(".todolist-pager .pagination > li > a").getTodoList();
    $(".remove-todoitem,.remove-todoitems").removeTodoList();
    $(".add-todo-item,.edit-todo").showModal();
};

// js main
$(function () {
    $(this).applyDatatable();
    $(this).initJs();
});
