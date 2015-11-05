$(document).ready(function() {
    $(".datetime").datetimepicker({
        format: "DD/MM/YYYY HH:mm",
        showTodayButton: true
    });
    jQuery.validator.methods["date"] = function(value, element) { return true; }
});