﻿$(document).ready(function () {

    // Lưu trữ mã html của loại câu hỏi
    var htmlOrgirial = $("#typeQuestion").html();

    // tạo nestable cho loại câu hỏi
    $("#typeQuestion").nestable({
        group: 1, // Chỉ có 1 nhóm được kéo thả
        maxDepth: 1 // Tối đa là 1
        //dragFinished: function (currentNode, parentNode) {
        //    //alert(1);
        //   // $("#typeQuestion").html(htmlOrgirial);
        //}
    });

    // khi có sự thay đổi trên các table thì đưa bảng loại câu hỏi về mặc định
    $(".dd").on("change", function () {
        $("#typeQuestion").html(htmlOrgirial);
    });
    
    // tạo nestable cho bảng câu hỏi
    $("#contentQuestion").nestable({
        group: 1, // Chỉ có 1 nhóm được kéo thả
        maxDepth: 1, // Tối đa là 1
        dragBegin: function () {
        }
        //dragFinished: function (currentNode, parentNode) {
           
        //}
    });

    

    // chạy các sự kiện khi thêm câu hỏi
    function startChoiceEvent() {
        // Tạo checkbox đẹp
        $(".i-checks").iCheck({
            checkboxClass: "icheckbox_square-green"
        });

        // Sự kiện xóa đáp án
        $(".remove-answer-choice").on("click", function(e) {
            e.preventDefault();
            $(this).removeAnswerChoice();
        });

        // sự kiện cho xóa loại câu hỏi
        $(".question-remove").on("click", function (e) {
            e.preventDefault();
            $(this).removeQuestion();
        });

        // Sự kiện cho thêm đáp án  của câu hỏi lựa chọn
        $(".add-choice-answer").on("click", function (e) {
            e.preventDefault();
            $(this).createAnswerChoice();
        });

        // Sự kiện thêm dáp án của câu hỏi sắp xếp
        $(".add-order-answer").on("click", function (e) {
            e.preventDefault();
            $(this).createAnswerOrder();
        });

        // Tạo tinymce
        $(this).createTiny("textarea.description",100);
    }


    
    // chạy sự kiện ban đầu
    startChoiceEvent();

    // khi có sự thay đổi trên bảng câu hỏi
    $("#contentQuestion").on("change", function() {
        var $newQuestion = $("#contentQuestion .new-question").first();
        $newQuestion.attr("class", "dd-item dd3-item questions");

        // Kiểm tra loại để tạo câu hỏi
        var type = $newQuestion.attr("data-type");
        switch (type) {
            case "Choice":
                $newQuestion.createChoice();
            break;
        default:
        }
    });

    function validateQuestion() {
        var isValid = true;
        var focus = null;
        var url = $("#questionForm").attr("action").split("/");

        // Xóa các thông tin lỗi cũ
        $("#Validation").html("");
        $("#QuestionTitleError").html("");
        $("#GroupIdError").html("");
        $("li.questions").each(function() {
            $(this).find("span.question-title-error").first().html("");
            $(this).find("span.question-answers-error").first().html("");
        });

        // Kiểm tra Id
        var isCreate = url[url.length - 1] == "Create";
        if (!isCreate) {
            if ($("#QuestionId").isNullOrEmpty()) {
                focus = $("#QuestionId");
                isValid = false;
                $("#Validation").html("Không tìm thấy mã của câu hỏi. Vui lòng tải lại trang.");
            }
        }

        // Kiểm tra tiêu đề khác rỗng
        if ($("#QuestionTitle").isNullOrEmpty()) {
            focus = focus == null ? $("#questionForm").first() : focus;
            isValid = false;
            $("#QuestionTitleError").html("Tiêu đề không được để trống!");
        }

        // Kiểm tra chọn nhóm câu hỏi
        if ($("#GroupId").isNullOrEmpty()) {
            focus = focus == null ? $("#GroupId") : focus;
            isValid = false;
            $("#GroupIdError").html("Chưa chọn nhóm câu hỏi!");
        }

        // Kiểm tra hợp lệ cho từng loại câu hỏi
        $("li.questions").each(function() {
            var type = $(this).attr("data-type");
            switch (type) {
                case "Choice":
                    // Kiểm tra tiêu đề
                    if ($(this).find("textarea.description").first().isNullOrEmpty()) {
                        isValid = false;
                        focus = focus == null ? $(this) : focus;
                        $(this).find("span.question-title-error").first().html("Tiêu đề không được để trống");
                    }
                
                    // Kiểm tra hợp lệ của đáp án

                    var div = $(this).find("div.answers").find("div.question-answer");
                    var correctNo = 0;
                    var isEmpty = false;
                    div.each(function() {
                        var correct = $(this).find("input[name='isCorrect']").first().prop("checked");
                        if (correct) correctNo++;
                        if ($(this).find("input[name='answer']").isNullOrEmpty()) {
                            isEmpty = true;
                        }
                        // var score = $(this).find("input[name='score']").val();
                    });
                    var mess = "";
                    if (correctNo == 0) {
                        isValid = false;
                        mess = "Bạn chưa chọn đáp án đúng. ";
                    }
                    if (isEmpty) {
                        isValid = false;
                        mess += "Bạn chưa nhập đủ nội dung đáp án!";
                    }
                    if (mess != "") {
                        $(this).find("span.question-answers-error").first().html(mess);
                        focus = focus == null ? $(this) : focus;
                    }
                    
                    break;
            }
        });

        if (!isValid) {
            // alert(parseInt($(focus).offset().top - 50));
            $("html, body").animate({ scrollTop: parseInt($(focus).offset().top - 50) }, 'fast');
        }
        return isValid;
    }

    // Sự kiện submit
    $(".submit-button").on("click", function (e) {
        // chặn hành động mặc định của nút submit
        e.preventDefault();

        // Không cho bấm submit 2 lần
        $(".submit-button").each(function () { $(this).attr("disabled", "disabled") });

        // Lấy text từ tinymce bỏ vào 
        tinyMCE.triggerSave();
        // Kiểm tra hợp lệ dự liệu
        if (validateQuestion()) {
            // Lấy url để biết là edit hay create
            var url = $("#questionForm").attr("action");

            // Lấy giá trị để chuyển sang trang thêm mới hay đến trang về danh sách
            var btnValue = $(this).attr("value");

            // Lấy Id câu hỏi
            var questionId = $("#QuestionId").val();

            // Lấy Tiêu đề câu hỏi
            var questionTitle = $("#QuestionTitle").val();

            // Lấy id nhóm câu hỏi
            var groupId = $("#GroupId").val();

            // Lấy trạng thái câu hỏi
            var actived = $("#Actived").prop("checked");

            // Lưu json toàn bổ câu hỏi
            var questions = "";

            // Lấy json toàn bộ câu hỏi
            $("li.questions").each(function(index) {
                questions += $(this).createJsonQuestion(index) + ",";
            });


            if (questions.substr(questions.length - 1) == ",") {
                questions = questions.substr(0, questions.length - 1);
            }

            questions = "[" + questions + "]";
            var data = "{\"question\":{\"QuestionId\":\"" + questionId + "\",\"QuestionTitle\":" + JSON.stringify(questionTitle) + ",\"GroupId\":\"" + groupId + "\",\"Actived\":\"" + actived + "\",\"Questions\":" + questions + "}}";

            // Thực hiện hành động
            $.ajax({
                type: "POST",
                url: url,
                data: { "data": data },
                success: function(response) {
                    if (response.Success) {
                        if (btnValue == "save-new")
                            window.location ="/Adm/Question/Create";
                        else
                            window.location="/Adm/Question";
                    } else {
                        $("#Validation").html(response.Message);
                        $("html, body").animate({ scrollTop: 0 }, "slow");
                        $(".submit-button").each(function () { $(this).removeAttr("disabled") });
                    }

                },
                error: function(xhr, ajaxOptions, thrownError) {
                    alert(xhr.responseText);
                }
            });
        } else {
            
            $(".submit-button").each(function() { $(this).removeAttr("disabled") });
        }
    });

});