$(document).ready(function () {
    // Auto-dismiss alerts after 5 seconds
    setTimeout(function () {
        $('.alert').alert('close');
    }, 5000);

    // Add hover effects to cards
    $('.card').hover(
        function () {
            $(this).css('transform', 'translateY(-5px)');
            $(this).css('box-shadow', '0 8px 15px rgba(0,0,0,0.1)');
        },
        function () {
            $(this).css('transform', 'translateY(0)');
            $(this).css('box-shadow', '0 4px 6px rgba(0,0,0,0.1)');
        }
    );
});