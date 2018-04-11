$.fn.remove = function () {
    $(this).click(function () {
        var code = $(this).attr('data-code');
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
               
            }
        });
    });
}

$(function () {
    $(".remove-project").remove();
})
