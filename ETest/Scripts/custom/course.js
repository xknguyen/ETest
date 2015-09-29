$(document).ready(function () {
    $('#pageSelect').change(function () {
        $('#pageSize').val($(this).val());
        $('#searchCourse').submit();
    });

    $("#statusSelect").change(function () {
        $('#pageSize').val($('#pageSelect').val());
        $('#supStatus').val($(this).val());
        $('#searchCourse').submit();
    });
});