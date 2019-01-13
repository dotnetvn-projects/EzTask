$.fn.deleteNotifyItem = function () {
    $(this).click(function () {
        $.showLoading();
        var id = $(this).data('notifyid');
        $.ajax({
            url: '/notification/delete.html',
            type: "POST",
            data: { notifyId: id },
            success: function (data) {
                $.loadNotifyList();
            }
        });
    });
};

$.loadNotifyList = function () {
    $.ajax({
        url: '/notification/load-list.html',
        type: "POST",
        success: function (data) {
            $('.notification-box-content').html('');
            $('.notification-box-content').html(data);
            $('.btn-delete').deleteNotifyItem();      
            $.hideLoading();
        }
    });
};


$(function () {
    $.showLoading();
    $.loadNotifyList();
});
