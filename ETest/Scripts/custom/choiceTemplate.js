// Choice
function questionChoiceTemplate() {
    var temp = $("#choiceTemplate").find("li.questions").first();
    var text =$(temp).find("div.description-content").first().find("textarea").first();
    var tinyCount = parseInt($('#tinyCount').val()) + 1;
    $('#tinyCount').val(tinyCount);
    text.attr("id", "QuestionTitle-"+tinyCount);
    var choice = $(temp).html();
    text.attr("id", "");
    return choice;
}

function answerChoiceTemplate(id) {
    var choice = $("#choiceTemplate").find("li.questions").first().find("div.answers").first().find("div.question-answer").first();
    //$(choice).find("div.i-checks").first().html("<input type='checkbox' name='isCorrect'/>");
    var answer = "<div class='col-md-12 question-answer'>" + choice.html() + "</div>";
    return answer;
}

// Order
function questionOrderTemplate() {
    var temp = $("#orderTemplate").find("li.questions").first();
    var text = $(temp).find("div.description-content").first().find("textarea").first();
    var tinyCount = parseInt($('#tinyCount').val()) + 1;
    $('#tinyCount').val(tinyCount);
    text.attr("id", "QuestionTitle-" + tinyCount);
    var choice = $(temp).html();
    text.attr("id", "");
    return choice;
}

function answerOrderTemplate(id) {
    var choice = $("#orderTemplate").find("li.questions").first().find("div.answers").first().find("div.question-answer").first();
    var answer = "<div class='col-md-12 question-answer answer-order'>" + choice.html() + "</div>";
    return answer;
}

function sortOrderTemplate(order) {
    var answer =
        "<li class='dd-item'>" +
            "<div class='dd-handle'>" +
            "<div class='col-md-12 question-answer answer-order'>" +
            "<input type='hidden' name='orderNo' value='" + order.orderNo + "'>" +
            "<input type='hidden' name='choiceId' value='" + order.id + "'>" +
            "<label class='hidden result'>" + order.result + "</label>" +
            "<div class=' question-form'>" +
            "<input type='text' class='form-control answer'readonly placeholder='Nhập nội dung đáp án' name='answer' value='" + order.content + "'>" +
            "</div>" +
            "</div>" +
            "</div>" +
            "</li>";
    return answer;
}

// slider
function questionSliderTemplate() {
    var temp = $("#sliderTemplate").find("li.questions").first();
    var text = $(temp).find("div.description-content").first().find("textarea").first();
    var tinyCount = parseInt($('#tinyCount').val()) + 1;
    $('#tinyCount').val(tinyCount);
    text.attr("id", "QuestionTitle-" + tinyCount);
    var choice = $(temp).html();
    text.attr("id", "");
    return choice;
}


// Assocate
function questionAssociateTemplate() {
    var temp = $("#associateTemplate").find("li.questions").first();
    var text = $(temp).find("div.description-content").first().find("textarea").first();
    var tinyCount = parseInt($('#tinyCount').val()) + 1;
    $('#tinyCount').val(tinyCount);
    text.attr("id", "QuestionTitle-" + tinyCount);
    var choice = $(temp).html();
    text.attr("id", "");
    return choice;
}

function answerAssociateTemplate() {
    var choice = $("#associateTemplate").find("li.questions").first().find("div.answers").first().find("div.question-answer").first();
    var answer = "<div class='col-md-12 question-answer'>" + choice.html() + "</div>";
    return answer;
}

// Gap
function questionGapTemplate() {
    var temp = $("#gapTemplate").find("li.questions").first();
    var text = $(temp).find("div.description-content").first().find("textarea").first();
    var tinyCount = parseInt($('#tinyCount').val()) + 1;
    $('#tinyCount').val(tinyCount);
    text.attr("id", "QuestionTitle-" + tinyCount);
    var choice = $(temp).html();
    text.attr("id", "");
    return choice;
}
function creatGapAnswer() {
    var choice = $("#gapTemplate").find("li.questions").first().find("div.answers").first();
    //var item = $(choice).find("div.gap-answer");
    //$(item).data("id", id);
    //var cont = $(item).find("p").first();
    //cont.text(content);
    //var answer = "<div class='col-md-12 question-answer'>" + choice.html() + "</div>";
    return choice.html();
}