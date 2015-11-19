$(function () {

    
    //Associate
    $.fn.createAssociateBox = function () {
        var leftChoice = $(this).find(".left-choice");
        var leftAnswer = $(this).find(".left-answer");
        var leftChoiceBox = $(this).find(".associate-answer-box");
        var leftAnswerDragged = $(this).find(".left-answer-dragged");
        var associateBoxId = $(this).attr("id");

        $(leftAnswerDragged).draggable({
            revert: "invalid",
            containment: "#" + associateBoxId
        });

        $(leftChoice).draggable({
            revert: "invalid",
            containment: "#" + associateBoxId
        });

        $(leftAnswer).droppable({
            activeClass: "ui-state-default",
            hoverClass: "ui-state-hover",
            accept: ":not(.ui-sortable-helper)",
            drop: function (event, ui) {
                // Kiểm tra nếu đã có thì thay thế, 1 cái chuyển về ô trả lời
                var text = $(this).find("div.left-answer-dragged");
                if (text.length > 0) {
                    text.attr("class", "left-choice associate-answer-border");
                    $(this).html("");
                    text.appendTo(leftChoiceBox);
                    $(text).draggable({
                        revert: "invalid",
                        containment: "#" + associateBoxId
                    });
                }
                var div = $("<div></div>");
                div.text(ui.draggable.text()).addClass("left-answer-dragged").attr("data-right-id", $(ui.draggable).attr("data-right-id"));
                div.appendTo(this);
                $(div).draggable({
                    revert: "invalid",
                    containment: "#" + associateBoxId
                });
                $(ui.draggable).remove();
            }
        });
        $(leftChoiceBox).droppable({
            activeClass: "ui-state-default",
            hoverClass: "ui-state-hover",
            accept: ":not(.ui-sortable-helper)",
            drop: function (event, ui) {
                var div = $("<div></div>");
                div.text(ui.draggable.text()).addClass("left-choice associate-answer-border").attr("data-right-id", $(ui.draggable).attr("data-right-id"));
                div.appendTo(this);
                $(div).draggable({
                    revert: "invalid",
                    containment: "#" + associateBoxId
                });
                $(ui.draggable).remove();
            }
        });
    }

    //GAP
    $.fn.createFillBox = function() {
        var oldAnswer = $(this).find(".fill-old-answer").first();
        if (oldAnswer != null) {
            var input = $(this).find("input[name='gapField']");
            $(oldAnswer).find("div").each(function () {
                var index = parseInt($(this).attr("data-index"));
                $(input[index]).attr("value",$(this).text().trim());
                $(this).remove();
            });
        }
    }
    $.fn.createGapBox = function () {
        var gapField = $(this).find(".gap-field-drogged");
        var gapAnswer = $(this).find(".answer-gap-dragged");
        var gapAnswerBox = $(this).find(".gap-answer-box");
        var gapBoxId = $(this).attr("id");

        var oldAnswer = $(this).find(".old-anwser-gap").first();
        if (oldAnswer != null) {
            $(oldAnswer).find("div").each(function() {
                var index = parseInt($(this).attr("data-index"));
                var div = $("<div></div>").text($(this).text().trim());
                $(div).draggable({
                    revert: "invalid",
                    containment: "#" + gapBoxId,
                    scroll: false
                });
                $(gapField[index]).html(div);
                
            });
        }

        $(gapAnswer).draggable({
            revert: "invalid",
            containment: "#" + gapBoxId,
            scroll: false
        });

        $(gapField).droppable(
        {
            activeClass: "ui-state-default",
            hoverClass: "ui-state-hover",
            accept: ":not(.ui-sortable-helper)",
            drop: function (event, ui) {
                // Kiểm tra nếu đã có thì thay thế, 1 cái chuyển về ô trả lời
                var text = $(this).find("div");
                if ($(text).text() != ui.draggable.text()) {
                    if (text.length > 0) {
                        $(this).text("");
                        var gap = $("<div></div>");
                        $(gap).attr("class", "answer-gap-dragged");
                        $(gap).text($(text).text());
                        gap.prependTo(gapAnswerBox);
                        $(gap).draggable({
                            revert: "invalid",
                            containment: "#" + gapBoxId,
                            scroll: false
                        });
                    }
                    var div = $("<div></div>");
                    div.text(ui.draggable.text());
                    div.appendTo(this);
                    $(div).draggable({
                        revert: "invalid",
                        containment: "#" + gapBoxId,
                        scroll: false
                    });
                    $(ui.draggable).remove();
                } else {
                    ui.draggable.attr("style", "position: relative;");
                }
            }
        });
        $(gapAnswerBox).droppable({
            activeClass: "ui-state-default",
            hoverClass: "ui-state-hover",
            accept: ":not(.ui-sortable-helper)",
            drop: function (event, ui) {
                var div = $("<div></div>");
                div.text(ui.draggable.text()).addClass("answer-gap-dragged");
                div.prependTo(this);
                $(div).draggable({
                    revert: "invalid",
                    containment: "#" + gapBoxId,
                    scroll: false
                });
                $(ui.draggable).remove();
            }
        });
    }

    //Slider
    $.fn.createSliderBox = function () {
        var slider = $(this).find(".answer-slider").first();
        var currrent = $(this).find(".currentValue").first();
        var g = $(slider).slider();
        if (g != null)
            g.on("slideStop", function (ui) {
                $(currrent).text(ui.value);
            });
    }

    $.fn.createPreview = function () {
        var ichecks = $(this).find(".i-checks");
        var that = $(this);
        //Choice and Choice media
        $(ichecks).iCheck({
            checkboxClass: 'icheckbox_square-green',
            radioClass: 'iradio_square-green'
        });

        // Associate
        var associateBox = $(this).find(".associate-box");
        $(associateBox).each(function () {
            $(this).createAssociateBox();
        });

        // Gap and Fill
        var gapBox = $(this).find(".gap-box");
        $(gapBox).each(function () {
            $(this).createGapBox();
        });
        var fillBox = $(this).find(".fill-box");
        $(fillBox).each(function () {
            $(this).createFillBox();
        });

        //Slider
        var sliderPreview = $(this).find(".slider-preview");
        $(sliderPreview).each(function () { $(this).createSliderBox(); });

        // Order
        var sorable = $(this).find(".sortable");
        $(sorable).sortable({
            stop: function () {
                that.find(".ui-state-default").each(function () {
                    var c = $(this).attr("class");
                    if (c.indexOf("ui-sortable-handle") == -1) {
                        $(this).removeClass("ui-state-default");
                    }
                });
            }
        });
        $(sorable).disableSelection();
    }


});