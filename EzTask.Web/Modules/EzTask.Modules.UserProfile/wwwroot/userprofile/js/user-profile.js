function readURL(input) {
    if (input.target.files && input.target.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('.img-avatar').attr('src', e.target.result);
        }
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
            error: function () {
                $('#upload-avatar-modal .error-message').text('Error, EzTask cannot execute uploading image. Try again please !')
            }
        });
    });
}

$(function () { 
    $("#image-upload").change(readURL);
    $(".btn-upload-avatar").uploadAvatar();
})