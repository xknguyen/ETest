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

    $(".preview-question").on("click", function(e) {
        e.preventDefault();
        var url = $(this).attr("href");
        $("#preview-question-content").html("");
        $.ajax({
            type: "POST",
            url: url
        }).done(function(response) {
            if (response != null) {
                $("#preview-question-content").html(response);
                $("#preview-question-form").createPreview();
                $("#showPreviewQuestion").click();
            } else {
                alert("Đã có lỗi xảy ra!");
            }

        });
    });

});