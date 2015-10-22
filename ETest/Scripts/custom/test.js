$(function() {
    $(".i-checks").iCheck({
        checkboxClass: 'icheckbox_square-green'
    });
    $("textarea").createTiny();

    $("#tbQuestions").createReorderDatable();

    var table = null;
    var selected = [];
    var selectedId = [];
    $("#addQuestion").on("click", function(e) {
        e.preventDefault();
        selected = [];
        //Lấy danh sách group
        $.ajax({
            type: "POST",
            url: "/Adm/Test/GetGroupForUser"
        }).done(function(response) {
            $("#treeviewGroups").createTreeView(response);
            $("#QuestionNo").text(0);

            $("#treeviewGroups").on("click", "a", function(e) {
                e.preventDefault();
            });
            $("#treeviewGroups").on("click", "li", function() {
                if (table != null) table.destroy();
                var url = "/Adm/Test/GetQuestion/" + $(this).find("a").first().attr("href");
                $.ajax({
                    type: "POST",
                    url: url
                }).done(function(data) {
                    $("#questionDiv").html(data);
                    table = $("#questionTable").createDatable(selectedId);

                    // Lấy danh sách Id đã có
                    var ids = [];
                    $("#tbQuestions").find("td.questionId").each(function() {
                        ids.push($(this).html().trim());
                    });
                    console.log(table.rows().length);
                    var listDelete = [];
                    table.rows().every(function() {
                        var d = this.data();
                        console.log(d[1]);
                        if ($.inArray(d[1], ids) != -1) {
                            listDelete.push(this);
                        }
                    });

                    for (var i = listDelete.length - 1; i >= 0; i--) {
                        table.row(listDelete[i]).remove();
                    }
                    // Draw once all updates are done
                    table.draw();

                    $('#questionTable tbody').on("click", "tr", function() {
                        $(this).toggleClass("selected");
                        var data = table.row(this).data();
                        if ($.inArray(data[1], selectedId) == -1) {
                            selected.push(data);
                            selectedId.push(data[1]);
                        } else {
                            selected = jQuery.grep(selected, function (value) {
                                return value[1] != data[1];
                            });
                            selectedId = jQuery.grep(selectedId, function (value) {
                                return value != data[1];
                            });
                        }
                        $("#QuestionNo").text(selected.length);
                    });
                    // Set sự kiện thay đổi các câu hỏi đã chọn
                });
            });

            $("#showChoiceQuestionForm").click();
        });
    });
    
    // Sự kiện lưu các câu hỏi đã chọn
    $(".saveButton").on("click", function() {
        if (selected.length > 0) {

        } else {
            alert("Bạn chưa chọn câu hỏi nào!");
        }
    });
});