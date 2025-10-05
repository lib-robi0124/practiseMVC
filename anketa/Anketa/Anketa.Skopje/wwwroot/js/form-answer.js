$(document).ready(function () {
    // Scale answer selection styling
    $('.scale-input').change(function () {
        $(this).closest('.scale-option').removeClass('btn-outline-primary').addClass('btn-primary');
        $(this).closest('.btn-group').find('.scale-option').not(this).removeClass('btn-primary').addClass('btn-outline-primary');
    });

    // Character counter for text areas
    $('textarea').on('input', function () {
        var length = $(this).val().length;
        $(this).siblings('.char-count').text(length);

        if (length > 450) {
            $(this).siblings('.text-muted').addClass('text-warning');
        } else {
            $(this).siblings('.text-muted').removeClass('text-warning');
        }
    });

    // Initialize character counts
    $('textarea').each(function () {
        var length = $(this).val().length;
        $(this).siblings('.char-count').text(length);
    });

    // Form validation
    $('form').on('submit', function () {
        var isValid = true;
        $('.question-group').each(function () {
            var hasAnswer = false;
            var questionType = $(this).find('.scale-input').length > 0 ? 'scale' : 'text';

            if (questionType === 'scale') {
                hasAnswer = $(this).find('.scale-input:checked').length > 0;
            } else {
                hasAnswer = $(this).find('textarea').val().trim().length > 0;
            }

            if (!hasAnswer) {
                isValid = false;
                $(this).addClass('border-danger');
            } else {
                $(this).removeClass('border-danger');
            }
        });

        if (!isValid) {
            alert('Ве молам одговорете на сите прашања пред да поднесете.');
            return false;
        }

        return true;
    });
});