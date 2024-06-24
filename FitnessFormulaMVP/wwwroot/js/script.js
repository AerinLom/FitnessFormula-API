$(document).ready(function () {
    $('.post-box').click(function () {
        var imageUrl = $(this).data('image');
        $('.image-container img').attr('src', imageUrl);
        $('.full-screen-image').fadeIn();
        $('body').addClass('no-scroll');
    });

    $('.close-btn').click(function () {
        $('.full-screen-image').fadeOut();
        $('body').removeClass('no-scroll');
    });
});
