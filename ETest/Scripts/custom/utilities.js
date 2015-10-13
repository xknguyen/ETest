$(function() {
    $.fn.isNullOrEmpty = function () {
        var value = $(this).val();
        if (value == null || value.trim() == "")
            return true;
        return false;
    }
});