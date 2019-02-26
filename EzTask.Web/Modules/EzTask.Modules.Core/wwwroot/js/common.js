
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
            if ($(this).val().length > 0 && $('.datepicker:visible') === false) {
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
            title: '',
            content:'',
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
            title: '',
            content: '',
            action: null
        }, options);

        $.alert({
            title: settings.title,
            content: settings.content,
            onClose: function () {
                if (settings.action !== null) {
                    settings.action();
                }
            }
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

    //iCheck for checkbox and radio inputs
    $.registeriCheck = function (element) {
        $(element).iCheck({
            checkboxClass: 'icheckbox_flat-green',
            radioClass: 'iradio_flat-green'
        });
    };

    $.loadNewNotifyList = function () {
        $.ajax({
            url: '/notify/new-list.html',
            type: "POST",
            success: function (data) {
                $(".notifications-menu").html('');
                $(".notifications-menu").html(data);
            }
        });
    };

    $.loadTaskNotifyList = function () {
        $.ajax({
            url: '/notify/task-list.html',
            type: "POST",
            success: function (data) {
                $(".tasks-menu").html('');
                $(".tasks-menu").html(data);
            }
        });
    };

    // Read a page's GET URL variables and return them as an associative array.
    $.queryString = function getUrlVars() {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        return vars;
    };

})(jQuery);