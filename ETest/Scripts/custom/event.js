$(function() {
    
    var currentGapAnswer = null;
    var currentGapQuestion = null;
    var currentOrder = null;
    // Tạo checkbox
    $.fn.createCheckBox = function() {
        $(this).iCheck({
            checkboxClass: "icheckbox_square-green"
        });
    }

    // Tạo tiny
    $.fn.createTiny = function(name, height) {
        tinymce.init({
            selector: name,
            entity_encoding: "raw",
            theme: "modern",
            height: height,
            menubar: false,
            plugins: [
                "link image lists charmap print preview hr anchor pagebreak spellchecker",
                "searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking",
                "save table contextmenu directionality emoticons template paste textcolor"
            ],
            toolbar: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist | link image | forecolor backcolor",
            style_formats: [
                { title: "Bold text", inline: "b" },
                { title: "Red text", inline: "span", styles: { color: "#ff0000" } },
                { title: "Red header", block: "h1", styles: { color: "#ff0000" } },
                { title: "Example 1", inline: "span", classes: "example1" },
                { title: "Example 2", inline: "span", classes: "example2" },
                { title: "Table styles" },
                { title: "Table row 1", selector: "tr", classes: "tablerow1" }
            ]
        });
    }
    // Tạo tinyGap
    $.fn.selectToGap = function (editor) {
        var current = $("#" + editor.id).closest('div.form-horizontal').first().find("div.gap-answer-selected").first();
        if (current.length != 0) {
            current.removeClass("gap-answer-selected");
            $(editor.getBody()).find("input[name='gapField']").each(function() {
                if ($(this).val() == current.attr("data-id")) {
                    $(this).attr("value", "");
                }
            });
            $(this).attr("value", current.attr("data-id"));
        }
    }
    $.fn.createGapTiny = function(name, height) {
        tinymce.init({
            selector: name,
            entity_encoding: "raw",
            theme: "modern",
            height: height,
            menubar: false,
            plugins: [
                "link image lists charmap print preview hr anchor pagebreak spellchecker",
                "searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking",
                "save table contextmenu directionality emoticons template paste textcolor"
            ],
            toolbar: "addGap |insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist | link image | forecolor backcolor",
            style_formats: [
                { title: "Bold text", inline: "b" },
                { title: "Red text", inline: "span", styles: { color: "#ff0000" } },
                { title: "Red header", block: "h1", styles: { color: "#ff0000" } },
                { title: "Example 1", inline: "span", classes: "example1" },
                { title: "Example 2", inline: "span", classes: "example2" },
                { title: "Table styles" },
                { title: "Table row 1", selector: "tr", classes: "tablerow1" }
            ],
            setup: function(editor) {
                editor.addButton('addGap', {
                    text: 'Thêm chỗ trống',
                    icon: false,
                    tooltip: 'Thêm chỗ trống',
                    onclick: function () {
                        var value = "";
                        if (editor.selection.getContent().trim() != "") {
                            var current = $("#" + editor.id).closest('div.form-horizontal').first().find("div.answers").first();
                            value = $(current).createGapAnswer(editor.selection.getContent());
                        }
                        editor.selection.setContent("<input name='gapField' type='text' value='"+value+"'/>");

                        $(editor.getBody()).off("click", "input");
                        $(editor.getBody()).on("click", "input", function() {
                            $(this).selectToGap(editor);
                        });
                    }
                });
            },
            init_instance_callback: function(editor) {
                $(editor.getBody()).off("click", "input");
                $(editor.getBody()).on("click", "input", function () {
                    $(this).selectToGap(editor);
                });
            }
        });
    }

    // removeTiny
    $.fn.removeTinyInTemplate = function() {
        $(".questionTemplate").each(function() {
            var s = $(this).find("div.description-content").first().find("textarea").first();
            $(s).attr("class", "form-control");
            $(s).removeAttr("id");
        });
    }

    $.fn.removeAnswer = function() {
        $(this).on("click", function(e) {
            e.preventDefault();
            $(this).removeAnswerChoice();
        });
    }

    $.fn.removeQuestionDetail = function() {
        $(this).on("click", function(e) {
            e.preventDefault();
            $(this).removeQuestion();
        });
    }

    $.fn.addChoiceAnswer = function() {
        $(this).on("click", function(e) {
            e.preventDefault();
            $(this).createAnswerChoice();
        });
    }

    // Order
    $.fn.addOrderAnswer = function() {
        $(this).on("click", function(e) {
            e.preventDefault();
            $(this).createAnswerOrder();
        });
    }
    // Sự kiện nút hiển thị popup chọn thứ tự đúng cho dạng câu hỏi sắp xếp
    $.fn.showOrderForm = function() {
        $(this).on("click", function(e) {
            e.preventDefault();
            $("#orderList").html("");
            var parent = $(this).closest("div.form-group").first();
            currentOrder = parent;
            // tìm danh sách đáp án
            var anwers = parent.find("div.answers").first().find("input[name='answer']");
            $(parent).find("span.question-answers-error").first().html("");
            var isValid = true;

            if (anwers.length == 0) {
                $(parent).find("span.question-answers-error").first().html("Bạn chưa có đáp án nào.");
                $("html, body").animate({ scrollTop: parseInt($($(this).closest("div.form-horizontal").first()).offset().top - 50) }, "fast");
            } else {
                anwers.each(function() {
                    if ($(this).isNullOrEmpty()) {
                        isValid = false;
                    }
                });
                if (isValid) {
                    $(this).closest("div.question-answer").first().find("a.order-answer-model").first().click();
                    $(this).createFormOrder();
                } else {
                    $(parent).find("span.question-answers-error").first().html("Bạn chưa nhập đủ nội dung các mục");
                    $("html, body").animate({ scrollTop: parseInt($($(this).closest("div.form-horizontal").first()).offset().top - 50) }, "fast");
                }
            }
        });
    }
    // Sự kiện luu popup order
    $.fn.saveOrderForm = function() {
        $(this).on("click", function(e) {
            e.preventDefault();
            var answers = $("#orderList").find("div.answer-order");
            var list = [];
            answers.each(function(index) {
                var id = parseInt($(this).find("input[name='choiceId']").first().val());
                var content = $(this).find("input[name='answer']").first().val();
                list.push(new OrderQuestion(id, content, index));
            });

            // Sắp xếp list theo order
            list.sort(dynamicSort("id"));

            // Thay đổi thứ tự
            if (currentOrder != null) {
                var lables = $(currentOrder).find("label.result");
                lables.each(function(index) {
                    $(this).html(list[index].result + 1);
                });
            }
            $("#closeButton").click();
        });
    }
    $.fn.closeOrderPopup = function() {
        $(this).on("click", function(e) {
            e.preventDefault();
            $("#orderList").html("");
        });
    }


    // Sự kiện slider
    $.fn.createSliderForm = function() {
        var g = $(this).slider();
        if (g != null)
            g.on('slide', function(ui) {
                var parent = $(this).closest("div.form-group").first();
                var currentvalue = $(parent).find("label.currentValue").first();
                $(currentvalue).html(ui.value);
            });
    }

    // Thay đổi giá trị limit slider
    $.fn.changeValueSlider = function() {
        $(this).on("change paste", function() {
            var parent = $(this).closest("div.form-group").first();
            var answer = $(parent).find("div.slider-content").first();
            var limit = $(parent).find("div.slider-limit").first();
            var minInput = $(limit).find("input[name='Min']").first();
            var maxInput = $(limit).find("input[name='Max']").first();
            var stepInput = $(limit).find("input[name='Step']").first();
            var min = parseFloat($(minInput).val());
            var max = parseFloat($(maxInput).val());
            var step = parseFloat($(stepInput).val());
            min = isNaN(min) ? 0 : min;
            max = isNaN(max) ? 0 : max;
            step = isNaN(step) ? 0 : step;
            min = min > max ? max : min;
            max = max < min ? min : max;
            step = step < 0 ? 0 : step;
            $(minInput).attr("max", max);
            $(maxInput).attr("min", min);
            $(stepInput).attr("min", 0);
            $(minInput).val(min);
            $(maxInput).val(max);
            $(stepInput).val(step);

            $(answer).html("<input type='text' class='answer-slider' name='answer' />");
            var slider = $(answer).find("input[name='answer']").first();
            slider.slider({
                max: max,
                min: min,
                step: step,
                value: min
            }).on("slide", function(ui) {
                var parent = $(this).closest("div.form-group").first();
                var currentvalue = $(parent).find("label.currentValue").first();
                $(currentvalue).html(ui.value);
            });
        });
    }


    // Sự kiện Associate
    // Thêm đáp án
    $.fn.addAssociateAnswer = function() {
        $(this).on("click", function(e) {
            e.preventDefault();
            $(this).createAnswerAssociate();
        });
    }

    // Gap
    // Chọn đáp án
    $.fn.selectGapAnswer = function() {
        $(this).on("click", function() {
            var thisClass = $(this).attr("class");
            if (thisClass.indexOf("gap-answer-selected") == -1) {
                $(this).closest("div.answers").first().find(".gap-answer-selected").each(function() {
                    $(this).removeClass("gap-answer-selected");
                });
                $(this).addClass("gap-answer-selected");
            } else {
                $(this).removeClass("gap-answer-selected");
            }
        });
    }
    $.fn.showAddGapAnswer = function() {
        $(this).on("click", function(e) {
            e.preventDefault();
            //Xóa giá trị trên popup
            $("#gapAnswerPopUp").val("");
            $("#gapAnswerPopUp").attr("data-type","new");

            // gán currentGapQuestion bàng thẻ dic chứa câu hỏi
            currentGapQuestion = $(this).closest("div.quetion-control").first().find("div.answers").first();

            // Hiển thị popup
            var showButton = $(this).closest("div.question-answer").first().find(".gap-answer-model").first();
            (showButton).click();
        });
    }
    $.fn.saveGapAnswer = function () {
        $(this).on("click", function (e) {
            e.preventDefault();
            if ($("#gapAnswerPopUp").isNullOrEmpty()) {
                $("#gapAnswerError").text("Nội dung không được để trống");
            } else {
                var type = $("#gapAnswerPopUp").attr("data-type");
                if (type == "new") {
                    if (currentGapQuestion != null) {
                        var value = $("#gapAnswerPopUp").val();
                        $(currentGapQuestion).createGapAnswer(value);
                    }
                } else {
                    // nếu là cập nhật thì
                    if (currentGapAnswer != null) {
                        $(currentGapAnswer).find("p").first().find("span").first().text($("#gapAnswerPopUp").val());
                    }
                }
                // đóng form
                $("#closeGapButton").click();
            }
        });
    }
    $.fn.showEditGapAnswer = function() {
        $(this).on("click", function(e) {
            e.preventDefault();
            $("#gapAnswerPopUp").attr("data-type", "update");
            currentGapAnswer = $(this).closest("div.gap-answer").first();
            $("#gapAnswerPopUp").val($(currentGapAnswer).find("p").first().text());
            // gán currentGapQuestion bàng thẻ dic chứa câu hỏi
            currentGapQuestion = $(this).closest("div.quetion-control").first().find("div.answers").first();

            // Hiển thị popup
            var showButton = $(this).closest("div.quetion-control").first().find(".gap-answer-model").first();
            (showButton).click();
        });
    }
    $.fn.removeGapAnswer = function() {
        $(this).on("click", function (e) {
            e.preventDefault();
            $(this).closest("div.gap-answer").first().remove();
        });
    }
    $.fn.closeGapAnswer = function () {
        $(this).on("click", function (e) {
            e.preventDefault();
            $("#gapAnswerPopUp").val("");
            $("#gapAnswerError").text("");
        });
    }



    // Khởi động các sự kiện
    $.fn.startQuestionEvent = function() {
        $(this).removeTinyInTemplate();
        $(this).createTiny("textarea.description",100);
        $(this).createGapTiny("textarea.gap-content", 100);
        // Tạo checkbox đẹp
        $("#eventQuestion .i-checks").createCheckBox();

        // Sự kiện xóa đáp án
        $("#eventQuestion .remove-answer-choice").removeAnswer();

        // sự kiện cho xóa loại câu hỏi
        $("#eventQuestion .question-remove").removeQuestionDetail();

        // Sự kiện cho thêm đáp án  của câu hỏi lựa chọn
        $("#eventQuestion .add-choice-answer").addChoiceAnswer();

        // Sự kiện thêm dáp án của câu hỏi sắp xếp
        $("#eventQuestion .add-order-answer").addOrderAnswer();

        // Sự kiện chọn thứ tự đúng cho dạng câu hỏi sắp xếp
        $("#eventQuestion .order-answer").showOrderForm();

        // Sự kiện Thay đổi Popup
        $("#saveButton").saveOrderForm();

        // Sự kiện nút đóng PopUp
        $("#closeButton").closeOrderPopup();

        // Slider
        $('#eventQuestion .answer-slider').createSliderForm();
        // Sự kiện thay đổi giá trị limit slider
        $("#eventQuestion .slider-value").changeValueSlider();
        
        // Associate
        $("#eventQuestion .add-associate-answer").addAssociateAnswer();

        // Gap
        $("#eventQuestion .gap-answer").selectGapAnswer();
        $("#eventQuestion .add-gap-answer").showAddGapAnswer();
        $("#eventQuestion .edit-gap-answer").showEditGapAnswer();
        $("#eventQuestion .remove-gap-answer").removeGapAnswer();
        $("#saveGapButton").saveGapAnswer();
        $("#closeGapButton").closeGapAnswer();
        
    }
    

});