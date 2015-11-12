$(document).ready(function () {
    $('#pageSelect').change(function () {
        $('#pageSize').val($(this).val());
        $('#searchAccount').submit();
    });

    $("#statusSelect").change(function () {
        $('#pageSize').val($('#pageSelect').val());
        $('#supStatus').val($(this).val());
        $('#searchAccount').submit();
    });
});