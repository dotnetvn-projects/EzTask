﻿$.fn.deleteproject = function () {
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

$(function () {
    $('.remove-project').deleteproject();
});
