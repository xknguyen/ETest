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


    // Tạo Json
    $.fn.createJsonQuestion = function(orderNo) {
        var type = $(this).attr("data-type");
        switch (type) {
        case 'Choice':
            var id, title, choice = "";
            id = $(this).find('input.questionId').first().val();
            title = $(this).find('textarea.description').first().val();
            var div = $(this).find('div.answers').find('div.question-answer');

            div.each(function (index) {
                choice += $(this).createChoiceAnswerJson() + ",";
            });

            if (choice.substr(choice.length - 1) == ",") {
                choice = choice.substr(0, choice.length - 1);
            }

            choice = "[" + choice + "]";
            return createJsonChoice(id, title, choice, orderNo);
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