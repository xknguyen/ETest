$(document).ready(function () {
    $('#pageSelect').change(function () {
        $('#pageSize').val($(this).val());
        $('#searchQuestion').submit();
    });

    $("#statusSelect").change(function () {
        $('#pageSize').val($('#pageSelect').val());
        $('#supStatus').val($(this).val());
        $('#searchQuestion').submit();
    });
});