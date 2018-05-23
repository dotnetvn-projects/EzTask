var project_delete_url='project/remove.html';
var user_upload_avatar = 'upload-avatar.html';

//modal helper
function CloseModal(modal) {
    $('#' + modal).modal('hide');
    $('.modal-backdrop').hide();
}

function ShowModal(modal) {
    $('#' + modal).modal('show');
}
function SetModalTitle(modal, title) {
    $('#' + modal + " .modal-title").text(title);
}
function ModalCloseTrigger(modal) {
    $('#' + modal).on('hidden.bs.modal', function () {
        $('.modal-backdrop').hide();
    });
}
//end modal helper

function DoAjax(form, callback, errorFunction, format) {
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

$(function () { 
    //select2
    $('.select2').select2();

    //Date picker
     $('.datepicker').datepicker({
         autoclose: true,
         todayHighlight: true
     }).on('show', function (e) {
         if ($(this).val().length > 0 && $('.datepicker:visible') == false) {
             $(this).datepicker('update', new Date($(this).val()));
         }
     })
  })