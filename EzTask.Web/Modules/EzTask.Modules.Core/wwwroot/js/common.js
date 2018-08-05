
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


    //------Dialog-------
    $.showDialog = function (options) {
        var settings = $.extend({
            dialogId : 'id1',
            title: 'Sample title',
            content: 'sample content',
            confirmAction: null
        }, options);

        if (settings.title) {
            $('#' + settings.dialogId + " .modal-title").text(settings.title);
        }
        if (settings.content) {
            $('#' + ettings.dialogId  + " .modal-body").text(content);
        }
        

        if (settings.confirmAction) {
            $('#' + modalId + " .btn-confirm").click(function () {
                settings.confirmAction();
            });
        }

        $('#' + ettings.dialogId ).modal('show');
    }

    $.closeDialog = function (dialogId) {
        $('#' + dialogId).modal('hide');
        $('.modal-backdrop').hide();
    }

    //hidden backdrop when click outsite
    $.triggerCloseDialog = function (dialogId) {
        $('#' + dialogId).on('hidden.bs.modal', function () {
            $('.modal-backdrop').hide();
        });
    }
    //------End Dialog----


    //------loading-----
    $.showLoading = function () {
        $(".loader-panel").addClass("is-active");
    }

    $.hideLoading = function () {
        $(".loader-panel").removeClass("is-active");
    }
    //-----End loading-----

})(jQuery);