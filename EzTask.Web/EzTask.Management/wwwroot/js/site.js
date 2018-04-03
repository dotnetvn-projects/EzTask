 $(function () { 
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