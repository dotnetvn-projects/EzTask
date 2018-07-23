
//end modal helper
function DoFormAjax(form, callback, errorFunction, format) {
        $.ajax({
            url: form.action,
            type: form.method,
            dataType: format,
            data: $(form).serialize(),
            success: callback,
            error: function (xhr, textStatus, errorThrown) {
                errorFunction(xhr, textStatus, errorThrown);
            }
        });
}

function DoAjax(url, method, data, callback, errorFunction, format) {
    $.ajax({
        url: url,
        type: method,
        dataType: format,
        data: data,
        success: callback,
        error: function (xhr, textStatus, errorThrown) {
            errorFunction(xhr, textStatus, errorThrown);
        }
    });
}

$(function () { 
    $.initCommonLib();
})