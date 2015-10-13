$(function () {
    // Tạo json
    function createJsonChoice(id, title, choice, orderNo) {
        var result = "{ \"QuestionType\":\"0\",\"QuestionDetailId\":\"" + id + "\",\"QuestionTitle\":" + JSON.stringify(title) + ",\"Choice\":" + choice + ",\"OrderNo\":\"" + orderNo + "\"}";
        return result;
    }
    function createJsonOrder(id, title, items, orderNo) {
        var result = "{ \"QuestionType\":\"1\",\"QuestionDetailId\":\"" + id + "\",\"QuestionTitle\":" + JSON.stringify(title) + ",\"Items\":" + items + ",\"OrderNo\":\"" + orderNo + "\"}";
        return result;
    }

    // Câu hỏi
    // Xóa câu hỏi
    $.fn.removeQuestion = function () {
        // tìm div.question-answer
        var parent = $(this).closest('li.dd-item').first();
        var ol = parent.closest('ol.dd-list').first();
        parent.remove();
        // nếu xóa hết thì thêm div empty
        if (ol.html() == null || ol.html().trim()=="") {
            // lấy thẻ div ngoài cùng
            var div = ol.closest('div.dd');
            div.html("<div class='dd-empty'></div>");
        }
        return this;
    }
    // Tạo Json
    $.fn.createJsonQuestion = function (orderNo) {
        var type = $(this).attr("data-type");
        var id, title, choice = "";
        switch (type) {
            case 'Choice':
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
            case "Order":
                id = $(this).find('input.questionId').first().val();
                title = $(this).find('textarea.description').first().val();
                var divOrder = $(this).find('div.answers').find('div.question-answer');

                divOrder.each(function (index) {
                    choice += $(this).createOrderAnswerJson() + ",";
                });

                if (choice.substr(choice.length - 1) == ",") {
                    choice = choice.substr(0, choice.length - 1);
                }

                choice = "[" + choice + "]";
                return createJsonOrder(id, title, choice, orderNo);
            default:
                return "";
        }
    }


    // Tạo câu hỏi lựa chọn
    $.fn.createChoice = function () {
        var choice = questionChoiceTemplate();
        $(this).html(choice);
        // Tạo checkbox đẹp
        $(this).find('.i-checks').createCheckBox();

        // tạo tinymce
        $(this).createTiny("textarea#QuestionTitle-" + parseInt($('#tinyCount').val()), 100);
        //tinymce.EditorManager.execCommand('mceAddEditor', true, "description");
        // Sự kiện xóa đáp án
        $(this).find('.remove-answer-choice').removeAnswer();

        // sự kiện cho xóa loại câu hỏi
        $(this).find('.question-remove').removeQuestionDetail();

        // Sự kiện cho thêm đáp án 
        $(this).find('.add-choice-answer').addChoiceAnswer();

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
        anwers.find('div.i-checks').createCheckBox();
        anwers.find('a.remove-answer-choice').removeAnswer();
        return this;
    }
    $.fn.removeAnswerChoice = function () {
        // tìm div.question-answer
        var parent = $(this).closest('div.question-answer').first();
        parent.remove();
        // tìm danh sách đáp án
        return this;
    }
    $.fn.createChoiceAnswerJson = function () {
        var correct = $(this).find("input[name='isCorrect']").first().prop('checked');
        var choiceId = $(this).find("input[name='choiceId']").first().val();
        var content = $(this).find("input[name='answer']").val();
        return "{\"ChoiceId\":\"" + choiceId + "\",\"Content\":" + JSON.stringify(content) + ",\"IsCorrect\":\"" + correct + "\"}";
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
        anwers.find('a.remove-answer-choice').removeAnswer();
        return this;
    }
    $.fn.createFormOrder = function () {
        var list = [];
        var parent = $(this).closest("div.form-group").first();
        // tìm danh sách đáp án
        var anwers = parent.find("div.answers").first().find("div.answer-order");

        // Lấy toàn bộ đáp án
        anwers.each(function (index) {
            var id = index;
            var content = $(this).find("input[name='answer']").first().val();
            var result = parseInt($(this).find("label.result").first().text());
            list.push(new OrderQuestion(id, content, result));
        });

        // sắp xếp thứ tự list theo result
        list.sort(dynamicSort("result"));
        //list.sort(sortByResult);
        // Đưa đáp án vào popup
        $.each(list, function () {
            $("#orderList").addItemPopup(this);
        });
    }
    $.fn.createOrderAnswerJson = function () {
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
        $(this).find('.remove-answer-choice').removeAnswer();

        // sự kiện cho xóa loại câu hỏi
        $(this).find('.question-remove').removeQuestionDetail();

        // Sự kiện cho thêm đáp án 
        $(this).find('.add-order-answer').addOrderAnswer();

        $(this).find(".order-answer").showOrderForm();
        // tạo tinymce
        $(this).createTiny("textarea#QuestionTitle-" + parseInt($('#tinyCount').val()), 100);
        return this;
    }
    $.fn.addItemPopup = function (order) {
        $(this).append(sortOrderTemplate(order));
    }


    // Tạo câu hỏi slider
    $.fn.createSlider = function () {
        var choice = questionSliderTemplate();
        $(this).html(choice);
        
        // sự kiện cho xóa loại câu hỏi
        $(this).find('.question-remove').removeQuestionDetail();
        // tạo tinymce
        $(this).createTiny("textarea#QuestionTitle-" + parseInt($('#tinyCount').val()), 100);

        $(this).find(".answer-slider").createSliderForm();
        // Sự kiện thay đổi giá trị limit slider
        $(this).find(".slider-value").changeValueSlider();
        return this;
    }

    // Tạo câu hỏi ghép đôi
    $.fn.createAssociate = function () {
        var choice = questionAssociateTemplate();
        $(this).html(choice);

        // sự kiện cho xóa loại câu hỏi
        $(this).find('.question-remove').removeQuestionDetail();
        // tạo tinymce
        $(this).createTiny("textarea#QuestionTitle-" + parseInt($('#tinyCount').val()), 100);

        // Thêm loại câu hỏi
        $(this).find(".add-associate-answer").addAssociateAnswer();
        return this;
    }
    $.fn.createAnswerAssociate = function () {
        //var id = parseInt($(this).attr('data-id')) + 1;
        //$(this).attr('data-id', id);
        var choice = answerAssociateTemplate();

        // tìm div.form-group
        var parent = $(this).closest('div.form-group').first();

        // tìm danh sách đáp án
        var anwers = parent.find('div.answers').first();
        $(anwers).append(choice);
        anwers.find('a.remove-answer-choice').removeAnswer();
        return this;
    }



    // Gap
    $.fn.createGap = function () {
        var choice = questionGapTemplate();
        $(this).html(choice);
        
        // sự kiện cho xóa loại câu hỏi
        $(this).find('.question-remove').removeQuestionDetail();
        // tạo tinymce
        $(this).createGapTiny("textarea#QuestionTitle-" + parseInt($('#tinyCount').val()), 100);
        $(this).find(".gap-answer").selectGapAnswer();
        $(this).find(".add-gap-answer").showAddGapAnswer();
        $(this).find(".edit-gap-answer").editGapAnswer();
        $(this).find(".remove-gap-answer").removeGapAnswer();
        return this;
    }
});