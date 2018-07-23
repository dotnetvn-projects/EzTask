(function($){
   $.initCommonLib = function(){
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
  
   $.showModal = function(modalId, title, content){
        $('#' + modal + " .modal-title").text(title);
        $('#' + modal + " .modal-body").text(content);
        $('#' + modal).modal('show');      
      });
   }

   $.closeModal = function(modalId){
     $('#' + modal).modal('hide');
     $('.modal-backdrop').hide();
   }
    $.triggerCloseModal = function(modalId){
        $('#' + modal).on('hidden.bs.modal', function () {
        $('.modal-backdrop').hide();
    }
}
})(jQuery);