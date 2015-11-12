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
                    // Lấy danh sách Id đã có
                    var ids = [];

                    questiontable.rows().every(function() {
                        var d = this.data();
                        ids.push(d[4]);
                    });

                    $("#questionTable").find("tbody").find("tr").each(function() {
                        var td = $(this).find("td.id").first();
                        var id = $(td).html().trim();
                        if ($.inArray(id, ids) != -1) {
                            this.remove();
                        }
                    });

                    table = $("#questionTable").createDatable(selectedId);

                    $("#randomNo").attr("max", table.rows().data().length);
                    $("#questionTable tbody").on("click", "tr", function() {
                        $(this).toggleClass("selected");
                        var data = table.row(this).data();
                        var id = data[2];
                        if ($.inArray(id, selectedId) == -1) {
                            selected.push(data);
                            selectedId.push(id);
                        } else {
                            selected = jQuery.grep(selected, function(value) {
                                return value[1] != id;
                            });
                            selectedId = jQuery.grep(selectedId, function(value) {
                                return value != id;
                            });
                        }
                        $("#QuestionNo").text(selected.length);
                    });
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
        var inputScore = "<input type='number' min='0' name='detailScore' class='form-control detail-score' value='0'>";
        var data = [orderNo, selected[0], selected[1], inputScore, selected[2], selected[3], buttons];
        var node = questiontable.row.add(data).draw().node();
        var tds = $(node).find("td");
        $(tds[4]).addClass("hidden questionId");
        $(tds[5]).addClass("hidden questionDetail");
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
    $("#randomButton").on("click", function() {
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


    // SỰ kiện chia điểm
    $("#divideScore").on("click", function(e) {
        e.preventDefault();
        var testScore = 100;
        var length = questiontable.rows().data().length;
        var medium = testScore / length;
        questiontable.rows().every(function() {
            var input = $(this.node()).find("input[name='detailScore']").first();
            $(input).val(medium);

            var td = $(this.node()).find("td.questionDetail").first();
            var string = $(td).html().trim();

            var details = $.parseJSON(string);
            var medi = medium / details.length;
            for (var i = 0; i < details.length; i++) {
                details[i].Score = medi;
            }
            $(td).html(JSON.stringify(details));
        });
        questiontable.draw();
    });


    $("#divideDetailScore").on("click", function(e) {
        e.preventDefault();
        var input = $("#tblTests").find("input[name='detailScore']");
        var medium = parseFloat($("#testScorePopup").text()) / input.length;
        input.each(function() {
            $(this).val(medium);
        });
    });

    // Valid
    function validateTest() {
        var test = new TestModel();
        //Trả về giá trị hợp lệ hay không
        var isValid = true;

        // Lấy thẻ đầu tiên bị lỗi
        var focus = null;

        // Kiểm tra sự kiện đang sử dụng
        var url = $("#testForm").attr("action").split("/");
        var isCreate = url[url.length - 2].indexOf("Create") != -1;

        // Trước khi kiểm tra thì xóa các thông báo cũ
        $("#Validation").text("");


        // Kiểm tra id của câu hỏi
        if (!isCreate) {
            if ($("#TestId").isNullOrEmpty()) {
                focus = $("#TestId");
                isValid = false;
                $("#Validation").text($("#Validation").text() + "Không tìm thấy mã bài kiểm tra. Vui lòng tải lại trang. ");
            } else {
                test.id = $("#TestId").val();
            }
        }

        // Kiểm tra tiêu đề câu hỏi khác rỗng
        if ($("#TestTitle").isNullOrEmpty()) {
            focus = focus == null ? $("#testForm").first() : focus;
            isValid = false;
            $("#TestTitleError").text("Tiêu đề không được để trống!");
        } else {
            $("#TestTitleError").text("");
            test.title = $("#TestTitle").val();
        }
        test.description = $("#Description").val();
        // Kiểm tra khóa học
        if ($("#CourseId").isNullOrEmpty()) {
            focus = focus == null ? $("#testForm") : focus;
            isValid = false;
            $("#Validation").text($("#Validation").text() + "Không tìm thấy mã khóa học. ");
        } else {
            test.course = $("#CourseId").val();
        }

        // Lấy thời gian
        test.timeStart = $("#TestStart").val();
        test.timeEnd = $("#TestEnd").val();
        var time = parseInt($("#TestTime").val());
        time = isNaN(time) || time < 0 ? 0 : time;
        if (time == 0) {
            focus = focus == null ? $("#TestTime") : focus;
            isValid = false;
            $("#TestTimeError").text("Thời gian bài kiểm tra phải lớn hơn 0!");
        } else {
            test.testTime = time;
        }


        test.mixedQuestion = $("#MixedQuestions").val();

        // Lấy số lần nộp bài và cách tính điểm
        var submitNo = parseInt($("#SubmitNo").val());
        submitNo = isNaN(submitNo) || submitNo < 0 ? 0 : submitNo;
        if (submitNo == 0) {
            focus = focus == null ? $("#SubmitNo") : focus;
            isValid = false;
            $("#SubmitNoError").text("Số lần nộp bài kiểm tra phải lớn hơn 0!");
        } else {
            test.submitNo = submitNo;
        }

        if ($("#GradeType").isNullOrEmpty()) {
            focus = focus == null ? $("#GradeType") : focus;
            isValid = false;
            $("#GradeTypeError").text("Chưa chọn cách tính điểm!");
        } else {
            test.gradeType = $("#GradeType").val();
        }

        // lấy trạng thái
        test.actived = $("#Actived").val();


        var messageTestDetailError = "";
        var totalScore = 0;
        var listDetail = [];
        // Lấy danh sách câu hỏi và điểm
        questiontable.rows().every(function() {
            var node = $(this.node());
            var tds = node.find("td");
            var testDetail = new TestDetailModel();
            var testDetailError = "";
            testDetail.order = $(tds[0]).text().trim();
            testDetail.id = $(tds[4]).text().trim();

            // Kiểm tra hợp lệ điểm của câu hỏi
            testDetail.score = parseFloat(node.find("input[name='detailScore']").val());
            testDetail.score = isNaN(testDetail.score) || testDetail.score < 0 ? 0 : testDetail.score;
            if (testDetail.score == 0) {
                testDetailError += "Chưa nhập điểm cho câu hỏi. ";
            } else {
                totalScore += testDetail.score;
            }

            testDetail.details = [];
            // Kiểm tra hợp lệ điểm của câu trả lời
            var questionDetails = $.parseJSON($(tds[5]).text().trim());
            var totalDetailScore = 0;
            for (var i = 0; i < questionDetails.length; i++) {
                var s = parseFloat(questionDetails[i].Score);
                s = isNaN(s) || s < 0 ? 0 : s;
                if (s == 0) {
                    testDetailError += "Chưa nhập điểm cho câu hỏi con thứ " + (i + 1) + ". ";
                } else {
                    totalDetailScore += s;
                    testDetail.details.push(new QuestionDetail(questionDetails[i].QuestionDetailId, questionDetails[i].Score));
                }
            }

            if (testDetail.score != totalDetailScore) {
                testDetailError += "Tổng điểm các câu hỏi con không bằng câu hỏi cha!";
            }

            if (testDetailError != "") {
                testDetailError = "Câu " + testDetail.order + ": " + testDetailError;
                messageTestDetailError += testDetailError;
            }

            listDetail.push(testDetail);
        });

        test.details = listDetail;
        if (messageTestDetailError != "") {
            isValid = false;
            $("#Validation").text($("#Validation").text() + messageTestDetailError);
            focus = focus == null ? $("#Validation") : focus;
        }

        if (!isValid) {
            $("#testInfoA").click();
            $("html, body").animate({ scrollTop: parseInt($(focus).offset().top - 50) }, "fast");
            return null;
        }
        return test;
    }

    //Submit
    $(".submit-button").on("click", function (e) {
        // chặn hành động mặc định của nút submit
        e.preventDefault();

        // Không cho bấm submit 2 lần
        $(".submit-button").each(function () { $(this).attr("disabled", "disabled") });
        // Lấy text từ tinymce bỏ vào 
        tinyMCE.triggerSave();

        // Lấy url để biết là edit hay create
        var url = $("#testForm").attr("action");
        var btnValue = $(this).attr("value");
        var result = validateTest();
        // Kiểm tra hợp lệ dữ liệu
        if (result!=null) {
            var data = JSON.stringify(result);
            $.ajax({
                type: "POST",
                url: url,
                data: { "data": data },
                success: function (response) {
                    if (response.Success) {
                        // Lấy giá trị để chuyển sang trang thêm mới hay đến trang về danh sách

                        if (btnValue == "save-new")
                            window.location = "/Adm/Test/Create/" + $("#CourseId").val();
                        else
                            window.location = "/Adm/Test?courseId=" + $("#CourseId").val();
                    } else {
                        $("#Validation").html(response.Message);
                        $("html, body").animate({ scrollTop: 0 }, "slow");
                        $(".submit-button").each(function () { $(this).removeAttr("disabled") });
                    }

                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.responseText);
                }
            });
        } else {
            $(".submit-button").each(function () { $(this).removeAttr("disabled") });
        }
    });
});