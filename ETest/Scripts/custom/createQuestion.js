$(function () {
    
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
        $(this).find(".edit-gap-answer").showEditGapAnswer();
        $(this).find(".remove-gap-answer").removeGapAnswer();
        return this;
    }
    $.fn.createGapAnswer = function(value) {
        // Lấy id hiện tại của câu trả lời
        var idButton = $(this).closest("div.quetion-control").first().find(".add-gap-answer").first();
        //alert($(idButton).attr("data-id"));
        var id = parseInt($(idButton).attr("data-id")) + 1;

        // Gán lại id mới
        $(idButton).attr("data-id", id);

        // Lấy mẫu câu hỏi
        var newItem = creatGapAnswer();

        // Thêm câu hỏi vào
        $(this).append(newItem);
        var item = $(this).find("div.gap-answer").last();
        
        // Gán giá trị
        $(item).attr("data-id", id);
        $(item).find("p").first().find("span").first().text(value);
        $(item).find("p").first().find("b").first().text(id);
        // Gán sự kiện
        $(item).find(".edit-gap-answer").showEditGapAnswer();
        $(item).find(".remove-gap-answer").removeGapAnswer();
        $(item).selectGapAnswer();
        return id;
    }
});