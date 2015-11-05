$(function() {
    $.fn.checkValidateQuestion = function () {
        var result = new ValidatedResult(true,null);
        // Kiểm tra title
        //if ($(this).find("textarea").first().isNullOrEmpty()) {
        //    result.isValid = false;
        //    result.focus = result.focus == null ? $(this) : result.focus;
        //    $(this).find("span.question-title-error").first().text("Tiêu đề không được để trống");
        //} else {
        //    $(this).find("span.question-title-error").first().text("");
        //}


        // Kiểm tra đáp án
        var type = $(this).attr("data-type");
        switch (type) {
        case "Choice":
            return $(this).checkValidateChoice(result);
        case "Order":
            return $(this).checkValidateOrder(result);
        case "Associate":
            return $(this).checkValidateAssociate(result);
        case "Fill":
        case "Gap":
            return $(this).checkValidateGap(result);
        case "Slider":
            return $(this).checkValidateSlider(result);
        case "ChoiceMedia":
            return $(this).checkValidateChoiceMedia(result);
        }
        return result;
    }

    $.fn.checkValidateChoice = function (result) {
        var div = $(this).find("div.answers").find("div.question-answer");
        var correctNo = 0;
        var isEmpty = false;

        if (div.length == 0) {
            result.isValid = false;
            result.focus = result.focus == null ? $(this) : result.focus;
            $(this).find("span.question-answers-error").first().text("Bạn chưa có đáp án nào");
        } else {
            div.each(function () {
                var correct = $(this).find("input[name='isCorrect']").first().prop("checked");
                if (correct) correctNo++;
                if ($(this).find("input[name='answer']").isNullOrEmpty()) {
                    isEmpty = true;
                }
            });

            var mess = "";
            if (correctNo == 0) {
                result.isValid = false;
                mess = "Bạn chưa chọn đáp án đúng. ";
            }

            if (isEmpty) {
                result.isValid = false;
                mess += "Bạn chưa nhập đủ nội dung đáp án!";
            }

            if (mess != "") {
                $(this).find("span.question-answers-error").first().text(mess);
                result.focus = result.focus == null ? $(this) : result.focus;
            } else {
                $(this).find("span.question-answers-error").first().text("");
            }
        }
        return result;
    }

    $.fn.checkValidateChoiceMedia = function (result) {
        var div = $(this).find("div.answers").find("div.choice-media-box");
        if (div.length == 0) {
            result.isValid = false;
            result.focus = result.focus == null ? $(this) : result.focus;
            $(this).find("span.question-answers-error").first().text("Bạn chưa có đáp án nào!");
        } else {

            var correct = $(this).find("img.selected");
            var mess = "";
            if (correct.length == 0) {
                result.isValid = false;
                mess = "Bạn chưa chọn đáp án đúng. ";
                $(this).find("span.question-answers-error").first().text(mess);
                result.focus = result.focus == null ? $(this) : result.focus;
            } else {
                $(this).find("span.question-answers-error").first().text("");
            }
        }
        return result;
    }


    $.fn.checkValidateOrder = function (result) {
        var isEmpty = false;

        var divOrder = $(this).find("div.answers").find("div.question-answer");

        if (divOrder.length == 0) {
            result.isValid = false;
            result.focus = result.focus == null ? $(this) : result.focus;
            $(this).find("span.question-answers-error").first().text("Bạn chưa có đáp án nào.");
        } else {
            divOrder.each(function () {
                if ($(this).find("input[name='answer']").isNullOrEmpty()) {
                    isEmpty = true;
                }
            });

            var messOrder = "";
            if (isEmpty) {
                result.isValid = false;
                messOrder += "Bạn chưa nhập đủ nội dung đáp án!";
            }
            if (messOrder != "") {
                $(this).find("span.question-answers-error").first().text(messOrder);
                result.focus = result.focus == null ? $(this) : result.focus;
            } else {
                $(this).find("span.question-answers-error").first().text("");
            }
        }
        return result;
    }

    $.fn.checkValidateAssociate = function (result) {
        var answers = $(this).find("div.answers").find("input.answer");
        var isEmpty = false;

        if (answers.length == 0) {
            result.isValid = false;
            result.focus = result.focus == null ? $(this) : result.focus;
            $(this).find("span.question-answers-error").first().text("Bạn chưa có đáp án nào.");
        } else {
            answers.each(function () {
                if ($(this).isNullOrEmpty()) {
                    isEmpty = true;
                }
            });

            if (isEmpty) {
                result.isValid = false;
                result.focus = result.focus == null ? $(this) : result.focus;
                $(this).find("span.question-answers-error").first().text("Bạn chưa nhập đủ nội dung đáp án.");
            } else {
                $(this).find("span.question-answers-error").first().text("");
            }
        }
        return result;
    }
    function checkExists(array, value) {
        for (var i = 0; i < array.length; i++) {
            if (array[i] == value)
                return true;
        }
        return false;
    }
    $.fn.checkValidateGap = function (result) {
        // Lấy tiny
        var tinyid = $(this).find("textarea").first().attr("id");
        var tiny = tinyMCE.get(tinyid);

        // Lấy các thẻ input trong tiny
        var inputs = $(tiny.getBody()).find("input[name='gapField']");
        var answers = $(this).find("div.gap-answer");

        if (inputs.length == 0) {
            result.isValid = false;
            $(this).find("span.question-answers-error").first().text("Chưa có chỗ trống nào.");
        } else {
            if (answers.length == 0) {
                result.isValid = false;
                $(this).find("span.question-answers-error").first().text("Chưa có đáp án nào.");
            } else {
                // Lấy danh sách id câu trả lời và kiểm tra đáp án rỗng
                var ids = [];
                var isEmpty = false;
                answers.each(function() {
                    var id = $(this).attr("data-id");
                    ids.push(id);
                    var content = $(this).find("span").first().text();
                    if (content == null || content == "") {
                        isEmpty = true;
                    }
                });

                if (isEmpty) {
                    result.isValid = false;
                    $(this).find("span.question-answers-error").first().text("Tồn tại đáp án có nội dung rỗng.");
                } else {
                    // Lấy danh sách
                    var inputIds = [];
                    var isValid = true;
                    var notExists = false;
                    isEmpty = false;
                    // Lấy danh sách và kiểm tra hợp lệ chỗ trống
                    inputs.each(function () {
                        // Kiểm tra có cho trống nào chưa map với đáp án
                        if ($(this).isNullOrEmpty()) {
                            isValid = false;
                            isEmpty = true;
                        } else {
                            var id = $(this).attr("value");
                            // Kiểm tra có 2 đáp án tồn tại
                            if (checkExists(inputIds, id)) {
                                isValid = false;
                            } else {
                                // Kiểm tra chỗ trống có trùng với đáp án ko 
                                inputIds.push(id);

                                // Kiểm tra đáp án được map với chỗ trống có tồn tại không
                                if (!checkExists(ids, id)) {
                                    notExists = true;
                                }
                            }
                        }
                    });

                    // thông báo lỗi
                    if (isEmpty) {
                        result.isValid = false;
                        $(this).find("span.question-answers-error").first().text("Chưa chọn đáp án cho chỗ trống.");
                    } else {
                        if (!isValid) {
                            result.isValid = false;
                            $(this).find("span.question-answers-error").first().text("Một đáp án chỉ sử dụng cho 1 chỗ trống.");
                        } else {
                            if (notExists) {
                                result.isValid = false;
                                $(this).find("span.question-answers-error").first().text("Có đáp án đang sử dụng cho chỗ trống không còn tồn tại.");
                            }
                        }
                    }
                }
            }
        }

        if (result.isValid) {
            $(this).find("span.question-answers-error").first().text("");
        } else {
            result.focus = result.focus == null ? $(this) : result.focus;
        }

        // Lấy danh sách id
        return result;
    }
    
    $.fn.checkValidateSlider = function (result) {
        var min = parseFloat($(this).find("input[name='Min']").first().val());
        var max = parseFloat($(this).find("input[name='Max']").first().val());
        var step = parseFloat($(this).find("input[name='Step']").first().val());
        var value = parseFloat($(this).find("label.currentValue").first().val());

        min = isNaN(min) ? 0 : min;
        max = isNaN(max) ? 0 : max;
        step = isNaN(step) ? 0 : step;
        value = isNaN(value) ? 0 : value;

        var message = "";

        if (min > max) {
            message = "Giá trị giới hạn không hợp lệ. ";
        }

        var diff = max - min;
        if ((diff) < step) {
            message += "Giá trị bước tiến phải nằm trong giá trị: " + diff + ". ";
        }

        if (value > max || value < min) {
            message += "Giá trị hiện tại phải nằm trong giá trị: " + min + "->" + max + ".";
        }

        if (message != "") {
            result.isValid = false;
            $(this).find("span.question-answers-error").first().text(message);
            result.focus = result.focus == null ? $(this) : result.focus;
        } else {
            $(this).find("span.question-answers-error").first().text("");
        }

        return result;
    }
});