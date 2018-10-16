
(function ($) {
    //----common-----
    $.initCommonLib = function () {
        //select2
        $('.select2').select2();

        //Date picker
        $('.datepicker').datepicker({
            autoclose: true,
            format: 'dd/mm/yyyy',
            todayHighlight: true
        }).on('show', function (e) {
            if ($(this).val().length > 0 && $('.datepicker:visible') == false) {
                $(this).datepicker('update', new Date($(this).val()));
            }
        });
    };
    //------End common------


    //------Dialog-------
    $.showDialog = function (options) {
        var settings = $.extend({
            dialogId: '',
            title: '',
            content: '',
            confirmAction: null
        }, options);

        if (settings.title.length > 0) {
            $('#' + settings.dialogId + " .modal-title").text(settings.title);
        }
        if (settings.content.length > 0) {
            $('#' + settings.dialogId + " .modal-body").text(settings.content);
        }


        if (settings.confirmAction) {
            $('#' + settings.dialogId + " .btn-confirm").click(function (e) {
                e.preventDefault();
                settings.confirmAction();
            });
        }

        $('#' + settings.dialogId).modal('show');
    };

    $.closeDialog = function (dialogId) {
        $('#' + dialogId).modal('hide');
        $('.modal-backdrop').hide();
    };

    //hidden backdrop when click outsite
    $.triggerCloseDialog = function (dialogId) {
        $('#' + dialogId).on('hidden.bs.modal', function () {
            $('.modal-backdrop').hide();
        });
    };
    //------End Dialog----


    //------loading-----
    $.showLoading = function () {
        $(".loader-panel").addClass("is-active");
    };

    $.hideLoading = function () {
        $(".loader-panel").removeClass("is-active");
    };
    //-----End loading-----

    //------Confirm dialog----
    $.confirmDialog = function (options) {
        var settings = $.extend({
            title: 'Hello',
            content:'Are you sure to continue?',
            action: null
        }, options);

        $.confirm({
            title: settings.title,
            content: settings.content,
            icon: 'fa fa-question',
            theme: 'modern',
            closeIcon: true,
            animation: 'scale',
            type: 'orange',
            buttons: {
                Yes: function () {
                    if (settings.action) {
                        settings.action();                      
                    }
                },
                close: function () {

                }
            }
        });
    };

    $.alertDialog = function (options) {
        var settings = $.extend({
            title: 'Alert!',
            content:''
        }, options);

        $.alert({
            title: settings.title,
            content: settings.content
        });
    };
    //------Confirm dialog----

    $.formatDate = function (date) {
        var year = date.getFullYear(),
            month = date.getMonth() + 1, // months are zero indexed
            day = date.getDate(),
            hour = date.getHours(),
            minute = date.getMinutes(),
            second = date.getSeconds(),
            hourFormatted = hour % 12 || 12, // hour returned in 24 hour format
            minuteFormatted = minute < 10 ? "0" + minute : minute,
            morning = hour < 12 ? " AM" : " PM";

        return month + "/" + day + "/" + year + " " + hourFormatted + ":" +
            minuteFormatted + morning;
    };

})(jQuery);