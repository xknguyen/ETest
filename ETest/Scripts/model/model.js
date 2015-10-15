function OrderQuestion(id, content, result) {
    this.id = id;
    this.content = content;
    this.result = result;
}

function sortByResult(a, b) {
    var aName = parseInt(a.result);
    var bName = parseInt(b.result);
    return ((aName < bName) ? -1 : ((aName > bName) ? 1 : 0));
}
function dynamicSort(property) {
    var sortOrder = 1;
    if (property[0] === "-") {
        sortOrder = -1;
        property = property.substr(1);
    }
    return function (a, b) {
        var result = (a[property] < b[property]) ? -1 : (a[property] > b[property]) ? 1 : 0;
        return result * sortOrder;
    }
}
function ValidatedResult(isValid, focus) {
    this.isValid = isValid;
    this.focus = focus;
}

$(function() {
    // Tạo json

    $.fn.getQuestion = function() {
        var questionId = $("#QuestionId").val();

        // Lấy Tiêu đề câu hỏi
        var questionTitle = $("#QuestionTitle").val();

        // Lấy id nhóm câu hỏi
        var groupId = $("#GroupId").val();

        // Lấy trạng thái câu hỏi
        var actived = $("#Actived").prop("checked");

        var questionDetails = "";

        // Lấy  toàn bộ câu hỏi
        $(this).find("li.questions").each(function(index) {
            questionDetails += $(this).getQuestionDetail(index) + ",";
        });
        if (questionDetails.substr(questionDetails.length - 1) == ",") {
            questionDetails = questionDetails.substr(0, questionDetails.length - 1);
        }

        questionDetails = "[" + questionDetails + "]";
        return "{\"question\":{\"QuestionId\":\"" + questionId + "\",\"QuestionTitle\":" + JSON.stringify(questionTitle) + ",\"GroupId\":\"" + groupId + "\",\"Actived\":\"" + actived + "\",\"Questions\":" + questionDetails + "}}";
    }
    $.fn.getQuestionDetail = function(orderNo) {
        var type = $(this).attr("data-type");
        switch (type) {
        case "Choice":
            return $(this).getChoice(orderNo);
        case "Order":
            return $(this).getOrder(orderNo);
        case "Associate":
            return $(this).getAssociate(orderNo);
        case "Gap":
            return $(this).getGap(orderNo);
        case "Slider":
            return $(this).getSlider(orderNo);
        }
        return "";
    }

    // Json
    function createJsonDetail(type, id, title, choice, orderNo) {
        var result = "{ \"QuestionType\":\"" + type + "\",\"QuestionDetailId\":\"" + id + "\",\"QuestionTitle\":" + JSON.stringify(title) + ",\"Choices\":" + choice + ",\"OrderNo\":\"" + orderNo + "\"}";
        return result;
    }

    $.fn.getChoice = function(orderNo) {
        var choice = "";
        var id = $(this).find('input.questionId').first().val();
        var title = $(this).find('textarea').first().val();
        var div = $(this).find('div.answers').find('div.question-answer');

        div.each(function() {
            choice += $(this).createChoiceAnswerJson() + ",";
        });

        if (choice.substr(choice.length - 1) == ",") {
            choice = choice.substr(0, choice.length - 1);
        }

        choice = "[" + choice + "]";
        return createJsonDetail(0, id, title, choice, orderNo);
    }
    $.fn.createChoiceAnswerJson = function() {
        var correct = $(this).find("input[name='isCorrect']").first().prop('checked');
        var content = $(this).find("input[name='answer']").val();
        return "{\"Content\":" + JSON.stringify(content) + ",\"IsCorrect\":\"" + correct + "\"}";
    }

    // Order
    $.fn.getOrder = function(orderNo) {
        var choice = "";
        var id = $(this).find('input.questionId').first().val();
        var title = $(this).find('textarea').first().val();
        var divOrder = $(this).find('div.answers').find('div.question-answer');

        divOrder.each(function(index) {
            choice += $(this).createOrderAnswerJson(index) + ",";
        });

        if (choice.substr(choice.length - 1) == ",") {
            choice = choice.substr(0, choice.length - 1);
        }

        choice = "[" + choice + "]";
        return createJsonDetail(1, id, title, choice, orderNo);
    }
    $.fn.createOrderAnswerJson = function (orderNo) {
        var content = $(this).find("input[name='answer']").first().val();
        var result = parseInt($(this).find("label.result").first().text());
        return "{\"Content\":" + JSON.stringify(content) + ",\"ChoiceId\":\"" + orderNo + "\",\"Result\":\"" + result + "\"}";
    }

    // Associate
    $.fn.getAssociate = function(orderNo) {
        var choice = "";
        var id = $(this).find('input.questionId').first().val();
        var title = $(this).find('textarea').first().val();
        var divOrder = $(this).find('div.answers').find('div.question-answer');

        divOrder.each(function() {
            choice += $(this).createAssociateAnswerJson() + ",";
        });

        if (choice.substr(choice.length - 1) == ",") {
            choice = choice.substr(0, choice.length - 1);
        }

        choice = "[" + choice + "]";
        return createJsonDetail(2, id, title, choice, orderNo);
    }
    $.fn.createAssociateAnswerJson = function() {
        var rightId = $(this).find("input[name='choiceRightId']").first().val();
        var rightContent = $(this).find("input[name='answerRight']").first().val();
        var leftId = $(this).find("input[name='choiceLeftId']").first().val();
        var leftContent = $(this).find("input[name='answerLeft']").first().val();
        return "{\"RightContent\":" + JSON.stringify(rightContent) + ",\"LeftContent\":" + JSON.stringify(leftContent) + ",\"RightId\":\"" + rightId + "\",\"LeftId\":\"" + leftId + "\"}";
    }

    // Gap
    $.fn.getGap = function(orderNo) {
        var choice = "";
        var id = $(this).find('input.questionId').first().val();
        var title = $(this).find('textarea').first().val();
        var div = $(this).find('div.answers').find('div.gap-answer');

        div.each(function() {
            choice += $(this).createGapAnswerJson() + ",";
        });

        if (choice.substr(choice.length - 1) == ",") {
            choice = choice.substr(0, choice.length - 1);
        }

        choice = "[" + choice + "]";
        return createJsonDetail(3, id, title, choice, orderNo);
    }
    $.fn.createGapAnswerJson = function() {
        var id = parseInt($(this).attr("data-id"));
        var content = $(this).find("span").first().text();
        return "{\"content\":" + JSON.stringify(content) + ",\"id\":\"" + id + "\"}";
    }

    // Slider
    $.fn.getSlider = function (orderNo) {
        var id = $(this).find('input.questionId').first().val();
        var title = $(this).find('textarea').first().val();
        var choice = $(this).createSliderAnswerJson();

        return createJsonDetail(4, id, title, choice, orderNo);
    }
    $.fn.createSliderAnswerJson = function () {
        var min = parseFloat($(this).find("input[name='Min']").first().val());
        var max = parseFloat($(this).find("input[name='Max']").first().val());
        var step = parseFloat($(this).find("input[name='Step']").first().val());
        var value = parseFloat($(this).find("label.currentValue").first().html());
        return "{\"Min\":" + min + ",\"Max\":\"" + max + "\",\"Step\":\"" + step + "\",\"Value\":\"" + value + "\"}";
    }
});