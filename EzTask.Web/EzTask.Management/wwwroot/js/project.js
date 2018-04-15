$.fn.remove = function () {
    $(this).click(function () {
        var code = $(".value-delete").val();
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

$.fn.DeleteConfirm = function (){
    $(this).click(function () {
        var project = $(this).attr('data-source');
        $(".source-delete").text('project: ' + project)
        $(".value-delete").val($(this).attr('data-code'));
        $('#delete-Modal').modal('show');
    })
}

$(function () {
    $(".remove-project").DeleteConfirm();
    $(".btn-delete").remove();
})
