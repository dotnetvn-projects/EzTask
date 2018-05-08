var project_delete_url='project/remove.html';
var user_upload_avatar = 'upload-avatar.html';

function CloseModal(modalName) {
    $('#' + modalName).modal('hide');
    $('.modal-backdrop').hide();
}

function ShowModal(modalName) {
    $('#' + modalName).modal('show');
}
function ModalCloseTrigger(modalName) {
    $('#' + modalName).on('hidden.bs.modal', function () {
        $('.modal-backdrop').hide();
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