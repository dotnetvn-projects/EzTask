(function ($) {
    $.fn.sliderbar = function (options) {
        var settings = $.extend({
            min: 0,
            max: 100
        }, options);

        $(this).attr("min", settings.min);
        $(this).attr("max", settings.max);
   
        updateValue(this);

        $(this).change(function () {
            updateValue(this);
        }); 
    }

    function updateValue(data) {
        $(".slider-value").text($(data).val() + '%');
    }
})(jQuery)