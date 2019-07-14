function readURL(input) {
    if (input.target.files && input.target.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('.img-avatar').attr('src', e.target.result);
        };
        reader.readAsDataURL(input.target.files[0]);
    }
}

$.fn.uploadAvatar = function () {
    $(this).click(function () {
        var fdata = new FormData();

        var fileInput = $("#image-upload")[0];
        var file = fileInput.files[0];
        fdata.append("file", file);
        $.ajax({
            type: 'post',
            url: 'upload-avatar.html',
            data: fdata,
            processData: false,
            contentType: false,
            success: function (response) {
                window.location = "profile.html";
            },
            error: function (xhr) {
                $('#upload-avatar-modal .error-message').text(xhr.responseText);
            }
        });
    });
};

$.fn.uploadPassword = function () {
    $(this).click(function (e) {
        e.preventDefault();
        var form = $(".frm-update-pass");
        if (form.valid()) {
            $.ajax({
                url: form.attr('action'),
                type: form.attr('method'),
                dataType: 'json',
                data: $(form).serialize(),
                success: function (xhr) {
                    $.alertDialog({
                        title: $('#success-title').val(),
                        content: xhr.responseText
                    });

                    $(".frm-update-pass input").val('');
                },
                error: function (xhr, textStatus, errorThrown) {
                    $.alertDialog({
                        title: $('#error-title').val(),
                        content: xhr.responseText
                    });
                }
            });
        }     
    });
};


$(function () {
    $("#image-upload").change(readURL);
    $(".btn-upload-avatar").uploadAvatar();
    $(".frm-update-pass button").uploadPassword();
});