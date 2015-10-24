$(function() {
    $(".i-checks").iCheck({
        checkboxClass: 'icheckbox_square-green'
    });
    $("textarea").createTiny();

    var questiontable = $("#tbQuestions").createReorderDatable();


    var buttons = "<a href='#' class='question-view'><i class='fa fa-2x fa-cog'></i></a> " +
                    "<a href='#' class='question-edit'><i class='fa fa-2x fa-edit'></i></a> " +
                    "<a href='#' class='question-remove'><i class='fa fa-2x fa-close'></i></a>";
    var table = null;
    var selected = [];
    var selectedId = [];
    $("#addQuestion").on("click", function(e) {
        e.preventDefault();
        if (table != null) {
            table.rows().remove().draw();

        }
        selected = [];
        selectedId = [];
        $("#groupName").text("Vui lòng chọn nhóm!");
        $("#randomNo").val(0);
        $("#randomNo").attr("max", 0);
        $("#randomNo").attr("min", 0);
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
                if (table != null) {
                    table.destroy();
                    $("#groupName").text("Vui lòng chọn nhóm");
                }
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
                    $("#randomNo").attr("max", table.rows().data().length);
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

    function getRandomNo(max) {
        return Math.floor((Math.random() * 1000) % max);
    }
    function isExists(array, value) {
        return ($.inArray(value, array) != -1);
    }
    function addData(selected, orderNo) {
        var data = [orderNo, selected[0], selected[2], selected[1], selected[3], 1, buttons];
        var node = questiontable.row.add(data).draw().node();
        var tds = $(node).find("td");
        $(tds[3]).addClass("hidden questionId");
        $(tds[4]).addClass("hidden questionDetail");
        $(tds[5]).addClass("hidden score");
        //$(node).find(".question-remove").removeQuestion(questiontable);
        //$(node).find(".question-view").setScore(questiontable);
        // Set sự kiện cho nút xóa câu hỏi
    }
    // Sự kiện lưu các câu hỏi đã chọn
    $("#saveButton").on("click", function() {
        if (selected.length > 0) {
            if (questiontable != null) {
                var orderNo = questiontable.rows().data().length + 1;
                for (var i = 0; i < selected.length; i++) {
                    addData(selected[i], orderNo);
                    orderNo++;
                }
                $("#closeButton").click();
            }
        } else {
            alert("Bạn chưa chọn câu hỏi nào!");
        }
    });

    // Sự kiện chọn ngẫu nhiên
    $("#randomButton").on("click", function () {
        if (table != null) {
            var data = table.rows().data();
            if (data.length > 0) {
                var numberNo = parseInt($("#randomNo").val());
                numberNo = isNaN(numberNo) ? 0 : numberNo;
                if (numberNo == 0) {
                    alert("Bạn chưa chọn số câu hỏi");
                } else {
                    if (numberNo <= data.length) {
                        var ids = [];
                        while (ids.length < numberNo) {
                            var number = getRandomNo(numberNo);
                            if (!isExists(ids, number)) {
                                ids.push(number);
                            }
                        }
                        var orderNo = questiontable.rows().data().length + 1;
                        for (var j = 0; j < ids.length; j++) {
                            var i = ids[j];
                            addData(data[i], orderNo);
                            orderNo++;
                        }
                        $("#closeButton").click();
                    } else {
                        alert("Số câu hỏi ngẫu nhiên lớn hơn tổng số câu khả dụng của nhóm!");
                    }
                }
            } else {
                alert("Nhóm này không có câu hỏi nào khả dụng!!!");
            }
        } else {
            alert("Bạn chưa chọn nhóm câu hỏi!!!");
        }
    });



    // Sự kiện xóa câu hỏi
    $("#tbQuestions").removeQuestion(questiontable);
    $("#tbQuestions").showSetScoreForm(questiontable);
});