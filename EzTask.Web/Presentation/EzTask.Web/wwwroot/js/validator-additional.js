$.validator.addMethod("requiredfield",
    function (value, element, params) {
        if (value === null || value === "" || value.length <= 0)
            return false;
        return true;
    });

//$.validator.unobtrusive.adapters.add("requiredfield", function (options) {
//    $('#' + options.element.id).attr("data-msg", options.message);
//});

$.validator.unobtrusive.adapters.addBool("requiredfield");

$.validator.addMethod("stringlengthfield",
    function (value, element, params) {
        var min = params.minLength;
        var max = params.maxLength;
        var valueLength = value.length;
        if (valueLength < min || valueLength > max)
            return false;
        return true;
    });

$.validator.unobtrusive.adapters.addBool("stringlengthfield");

$.validator.addMethod("emailfield",
    function (value, element, params) {
        if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(value)) {
            return true;
        }
        return false;
    });

$.validator.unobtrusive.adapters.addBool("emailfield");


//$.validator.unobtrusive.adapters.add("emailfield", function (options) {
//    $('#' + options.element.id).attr("data-msg", options.message);
//});