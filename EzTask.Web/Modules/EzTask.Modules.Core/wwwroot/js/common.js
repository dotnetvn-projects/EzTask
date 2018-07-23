(function ($) {
    //----common-----
    $.initCommonLib = function () {
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
        });
    }
    //------End common------


    //------modal-------
    $.showModal = function (modalId, title, content) {
        if (title) {
            $('#' + modal + " .modal-title").text(title);
        }
        if (content) {
            $('#' + modal + " .modal-body").text(content);
        }
        $('#' + modal).modal('show');
    };

    $.closeModal = function (modalId) {
        $('#' + modal).modal('hide');
        $('.modal-backdrop').hide();
    }

    //hidden backdrop when click outsite
    $.triggerCloseModal = function (modalId) {
        $('#' + modal).on('hidden.bs.modal', function () {
            $('.modal-backdrop').hide();
        });
    }
    //------End modal----


    //------loading-----
    $.showLoading = function () {
        $(".loader-panel").addClass("is-active");
    }

    $.hideLoading = function () {
        $(".loader-panel").removeClass("is-active");
    }
    //-----End loading-----

})(jQuery);