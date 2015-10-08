$(document).ready(function () {
    

    // Tạo câu hỏi lựa chọn
    function createJsonChoice(id, title, choice, orderNo) {
        var result = "{ \"QuestionType\":\"0\",\"QuestionDetailId\":\"" + id + "\",\"QuestionTitle\":" + JSON.stringify(title) + ",\"Choice\":" + choice + ",\"OrderNo\":\"" + orderNo + "\"}";
        return result;
    }
    $.fn.createChoice = function() {
        var choice = questionChoiceTemplate();
        $(this).html(choice);
         // Tạo checkbox đẹp
        $(this).find('.i-checks').iCheck({
            checkboxClass: 'icheckbox_square-green'
        });

        // Sự kiện xóa đáp án
        $(this).find('.remove-answer-choice').on('click', function (e) {
            e.preventDefault();
            $(this).removeAnswerChoice();
        });

        // sự kiện cho xóa loại câu hỏi
        $(this).find('.question-remove').on('click', function (e) {
            e.preventDefault();
            $(this).removeQuestion();
        });

        // Sự kiện cho thêm đáp án 
        $(this).find('.add-choice-answer').on('click', function (e) {
            e.preventDefault();
            $(this).createAnswerChoice();
        });

        // tạo tinymce
        $(this).createTiny("textarea.description", 100);
        return this;
    }
    $.fn.createAnswerChoice = function () {
        var id = parseInt($(this).attr('data-id')) + 1;
        $(this).attr('data-id', id);
        var choice = answerChoiceTemplate(id);
        
        // tìm div.form-group
        var parent = $(this).closest('div.form-group').first();

        // tìm danh sách đáp án
        var anwers = parent.find('div.answers').first();
        $(anwers).append(choice);
        anwers.find('div.i-checks').iCheck({
                checkboxClass: 'icheckbox_square-green'
            });
        anwers.find('a.remove-answer-choice').on('click', function (e) {
            e.preventDefault();
            $(this).removeAnswerChoice();
        });
        return this;
    }
    $.fn.removeAnswerChoice = function () {
        // tìm div.question-answer
        var parent = $(this).closest('div.question-answer').first();
        parent.remove();
        // tìm danh sách đáp án
        return this;
    }
    $.fn.createChoiceAnswerJson = function() {
        var correct = $(this).find("input[name='isCorrect']").first().prop('checked');
        var choiceId = $(this).find("input[name='choiceId']").first().val();
        var content = $(this).find("input[name='answer']").val();
        var score = $(this).find("input[name='score']").val();

        return "{\"ChoiceId\":\"" + choiceId + "\",\"Content\":" + JSON.stringify(content) + ",\"Score\":\""+score+"\",\"IsCorrect\":\""+correct+"\"}";
    }

    // Tạo câu hỏi sắp xếp
    function createJsonOrder(id, title, items, orderNo) {
        var result = "{ \"QuestionType\":\"1\",\"QuestionDetailId\":\"" + id + "\",\"QuestionTitle\":" + JSON.stringify(title) + ",\"Items\":" + items + ",\"OrderNo\":\"" + orderNo + "\"}";
        return result;
    }
    $.fn.createAnswerOrder = function () {
        var id = parseInt($(this).attr('data-id')) + 1;
        $(this).attr('data-id', id);
        var choice = answerOrderTemplate(id);

        // tìm div.form-group
        var parent = $(this).closest('div.form-group').first();

        // tìm danh sách đáp án
        var anwers = parent.find('div.answers').first();
        $(anwers).append(choice);
        anwers.find('a.remove-answer-choice').on('click', function (e) {
            e.preventDefault();
            $(this).removeAnswerChoice();
        });
        return this;
    }
    $.fn.createFormOrder = function() {
        var list = [];
        var parent = $(this).closest("div.form-group").first();
        // tìm danh sách đáp án
        var anwers = parent.find("div.answers").first().find("div.answer-order");

        // Lấy toàn bộ đáp án
        anwers.each(function () {
            var id = parseInt($(this).find("input[name='choiceId']").first().val());

            var orderNo = parseInt($(this).find("input[name='orderNo']").first().val());
            var content = $(this).find("input[name='answer']").first().val();
            var result = parseInt($(this).find("label.result").first().text());
            
            list.push(new OrderQuestion(id, orderNo, content, result));
        });
        
        // sắp xếp thứ tự list theo result
        list.sort(dynamicSort("result"));
        //list.sort(sortByResult);
        // Đưa đáp án vào popup
        $.each(list, function() {
            $("#orderList").addItemPopup(this);
        });
    }
    $.fn.createOrderAnswerJson = function() {
        var choiceId = parseInt($(this).find("input[name='choiceId']").first().val());
        var orderNo = parseInt($(this).find("input[name='orderNo']").first().val());
        var content = $(this).find("input[name='answer']").first().val();
        var result = parseInt($(this).find("label.result").first().text());
        return "{\"ChoiceId\":\"" + choiceId + "\",\"Content\":" + JSON.stringify(content) + ",\"OrderNo\":\"" + orderNo + "\",\"Result\":\"" + result + "\"}";
    }
    $.fn.createOrder = function () {
        var choice = questionOrderTemplate();
        $(this).html(choice);

        // Sự kiện xóa đáp án
        $(this).find('.remove-answer-choice').on('click', function (e) {
            e.preventDefault();
            $(this).removeAnswerChoice();
        });

        // sự kiện cho xóa loại câu hỏi
        $(this).find('.question-remove').on('click', function (e) {
            e.preventDefault();
            $(this).removeQuestion();
        });

        // Sự kiện cho thêm đáp án 
        $(this).find('.add-order-answer').on('click', function (e) {
            e.preventDefault();
            $(this).createAnswerOrder();
        });

        $(".order-answer").on("click", function (e) {
            $("#orderList").html("");
            var parent = $(this).closest("div.form-group").first();
            currentOrder = parent;
            // tìm danh sách đáp án
            var anwers = parent.find("div.answers").first().find("input[name='answer']");
            $(parent).find("span.question-answers-error").first().html("");
            var isValid = true;

            anwers.each(function () {
                if ($(this).isNullOrEmpty()) {
                    isValid = false;
                }
            });
            if (isValid) {
                $(this).closest("div.question-answer").first().find("a.order-answer-model").first().click();
                $(this).createFormOrder();
            }
            else {
                $(parent).find("span.question-answers-error").first().html("Bạn chưa nhập đủ nội dung các mục");
                $("html, body").animate({ scrollTop: parseInt($($(this).closest("div.form-horizontal").first()).offset().top - 50) }, "fast");
                e.preventDefault();
            }
        });
        // tạo tinymce
        $(this).createTiny("textarea.description", 100);
        return this;
    }
    $.fn.addItemPopup = function(order) {
        $(this).append(sortOrderTemplate(order));
    }

    // Tạo câu hỏi Upload
    function createJsonUpload(id, title, orderNo) {
        var result = "{ \"QuestionType\":\"5\",\"QuestionDetailId\":\"" + id + "\",\"QuestionTitle\":" + JSON.stringify(title) + ",\"OrderNo\":\"" + orderNo + "\"}";
        return result;
    }
    $.fn.createUpload = function () {
        var choice = questionUploadTemplate();
        $(this).html(choice);
        // tạo tinymce
        $(this).createTiny("textarea.description", 100);
        return this;
    }

    // Tạo Json
    $.fn.createJsonQuestion = function(orderNo) {
        var type = $(this).attr("data-type");
        var id, title, choice = "";
        switch (type) {
        case 'Choice':
            id = $(this).find('input.questionId').first().val();
            title = $(this).find('textarea.description').first().val();
            var div = $(this).find('div.answers').find('div.question-answer');

            div.each(function(index) {
                choice += $(this).createChoiceAnswerJson() + ",";
            });

            if (choice.substr(choice.length - 1) == ",") {
                choice = choice.substr(0, choice.length - 1);
            }

            choice = "[" + choice + "]";
            return createJsonChoice(id, title, choice, orderNo);
        case "Order":
            id = $(this).find('input.questionId').first().val();
            title = $(this).find('textarea.description').first().val();
            var divOrder = $(this).find('div.answers').find('div.question-answer');

            divOrder.each(function(index) {
                choice += $(this).createOrderAnswerJson() + ",";
            });

            if (choice.substr(choice.length - 1) == ",") {
                choice = choice.substr(0, choice.length - 1);
            }

            choice = "[" + choice + "]";
            return createJsonOrder(id, title, choice, orderNo);
        case "Upload":
            id = $(this).find('input.questionId').first().val();
            title = $(this).find('textarea.description').first().val();
            return createJsonUpload(id, title, orderNo);
            break;
        default:
            return "";
        }
    }

    // Câu hỏi
    // Xóa câu hỏi
    $.fn.removeQuestion = function () {
        // tìm div.question-answer
        var parent = $(this).closest('li.dd-item').first();
        var ol = parent.closest('ol.dd-list').first();
        parent.remove();
        // nếu xóa hết thì thêm div empty
        if (ol.html() == null || ol.html() == '') {
            // lấy thẻ div ngoài cùng
            var div = ol.closest('div.dd');
            div.html("<div class='dd-empty'></div>");
        }
        return this;
    }
});