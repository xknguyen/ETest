$(function() {
    // Lưu trữ mã html của loại câu hỏi
    var htmlOrgirial = $("#typeQuestion").html();

    // tạo nestable cho loại câu hỏi
    $("#typeQuestion").nestable({
        group: 1, // Chỉ có 1 nhóm được kéo thả
        maxDepth: 1 // Tối đa là 1
    });

    // Tạo nettable cho bảng sắp xếp thứ tự
    $("#orderContent").nestable({
        group: 1, // Chỉ có 1 nhóm được kéo thả
        maxDepth: 1 // Tối đa là 1
    });

    // tạo nestable cho bảng câu hỏi
    $("#contentQuestion").nestable({
        group: 1, // Chỉ có 1 nhóm được kéo thả
        maxDepth: 1
    });

    

    // khi có sự thay đổi trên các table thì đưa bảng loại câu hỏi về mặc định
    $(".dd").on("change", function () {
        $("#typeQuestion").html(htmlOrgirial);
    });

    
    // chạy sự kiện ban đầu
    $(this).startQuestionEvent();


    // khi có sự thay đổi trên bảng câu hỏi
    $("#contentQuestion").on("change", function () {
        var $newQuestion = $(this).find(".new-question").first();
        $newQuestion.attr("class", "dd-item dd3-item questions");

        // Kiểm tra loại để tạo câu hỏi
        var type = $newQuestion.attr("data-type");
        switch (type) {
            case "Choice":
                $newQuestion.createChoice();
                break;
            case "Order":
                $newQuestion.createOrder();
                break;
            case "Associate":
                $newQuestion.createAssociate();
                break;
            case "Map":
                break;
            case "Gap":
                $newQuestion.createGap();
                break;
            case "Inline":
                break;
            case "Slider":
                $newQuestion.createSlider();
                break;
            default:
        }
    });
});