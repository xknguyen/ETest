function questionChoiceTemplate() {
    var choice =
                "<div class='pull-right m-r-md m-t-sm'>" +
                    "<a href='#' class='question-remove'><i class='fa fa-close'></i></a>" +
                "</div>" +
                "<div class='dd-handle dd3-handle question-type'>Choice</div>" +
                "<div class='dd3-content question-border'>" +
                    "<div class='row'>" +
                        "<div class='col-md-12'>" +
                                "<div class='form-horizontal'>" +
                                    "<div class='form-group quetion-control'>" +
                                        "<div class='col-md-12'>" +
                                        "<input type='hidden' class='questionId' value=''>"+
                                            "<textarea class='form-control description' placeholder='Nhập nội dung câu hỏi'></textarea>" +
                                            "<span class='field-validation-valid text-danger question-title-error'></span>"+
                                        "</div>" +
                                    "</div>" +
                                    "<div class='form-group  quetion-control'>" +
						                "<div class='answers'>" +
                                        "<span class='field-validation-valid text-danger question-answers-error'></span>"+
                                            answerChoiceTemplate(1) +
                                        "</div>" +
                                        "<div class='col-md-12 question-answer'>" +
                                            "<a class='button btn btn-primary add-choice-answer' data-id='1'><i class='fa fa-plus-circle'></i> Thêm đáp án</a>" +
                                        "</div>" +
                                    "</div>" +
                                "</div>" +
                        "</div>" +
                    "</div>" +
                "</div>";
    return choice;
}

function answerChoiceTemplate(id) {
    var answer = "<div class='col-md-12 question-answer'>" +
        "<div class='col-md-1 question-form question-check'>" +
        "<div class='checkbox i-checks'>" +
        "<input type='hidden' name='choiceId' value ='"+id+"'/>"+
        "<input type='checkbox' class='form-control' name='isCorrect' />" +
        "</div>" +
        "</div>" +
        "<div class='col-md-9 question-form'>" +
        "<input type='text' class='form-control answer' placeholder='Nhập nội dung đáp án' name='answer' />" +
        "</div>" +
        "<div class='col-md-2 question-form'>" +
        "<div class='col-md-11 question-form'>" +
        "<input type='number' class='form-control score' placeholder='Điểm' name='score' value='0' />" +
        "</div>" +
        "<div class='col-md-1 question-form question-close'>" +
        "<a href='#' class='remove-answer-choice'><i class='fa fa-close'></i></a>" +
        "</div>" +
        "</div>" +
        "</div>";
    return answer;
}

function answerOrderTemplate(id) {
    var answer = "<div class='col-md-12 question-answer answer-order'>" +
                    "<input type='hidden' name='choiceId' value='"+id+"'>" +
                    "<label class='control-label col-md-1'>0</label>" +
                    "<div class='col-md-9 question-form'>" +
                        "<input type='text' class='form-control answer' placeholder='Nhập nội dung đáp án' name='answer' value=''>" +
                    "</div>" +
                    "<div class='col-md-1 question-form'>" +
                        "<div class='col-md-1 question-form question-close'>" +
                            "<a href='#' class='remove-answer-choice'>" +
                                "<i class='fa fa-close'></i>" +
                            "</a>" +
                        "</div>" +
                    "</div>" +
                "</div>";
    return answer;
}