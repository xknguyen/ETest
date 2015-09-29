$(document).ready(function() {
    $('.datetime').datetimepicker({
        format: 'dd/MM/yyyy'
    });
    jQuery.validator.methods["date"] = function(value, element) { return true; }
});