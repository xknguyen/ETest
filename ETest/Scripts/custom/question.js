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
        maxDepth: 1,
        dragFinished: function (currentNode, parentNode) {
            var type = $(currentNode).attr("class");
            if (type.indexOf("new-question") ==-1) {
                var id = $(currentNode).find("textarea").first().attr("id");
                tinymce.EditorManager.execCommand("mceRemoveEditor", true, id);
                type = $(currentNode).attr("data-type");
                if (type == "Gap") {
                    $(currentNode).createGapTiny("#" + id,100);
                } else {
                    $(currentNode).createTiny("#" + id,100);
                }
            }
        }
    });

    

    // khi có sự thay đổi trên các table thì đưa bảng loại câu hỏi về mặc định
    $(".dd").on("change", function () {
        $("#typeQuestion").html(htmlOrgirial);
    });
    // khi có sự thay đổi trên bảng câu hỏi
    $("#contentQuestion").on("change", function (e) {
        var $currentNode = $(this).find(".new-question").first();
        if ($currentNode.length != 0) {
            $currentNode.attr("class", "dd-item dd3-item questions");
            // Kiểm tra loại để tạo câu hỏi
            var type = $currentNode.attr("data-type");
            switch (type) {
                case "Choice":
                    $currentNode.createChoice();
                    break;
                case "Order":
                    $currentNode.createOrder();
                    break;
                case "Associate":
                    $currentNode.createAssociate();
                    break;
                case "Gap":
                    $currentNode.createGap();
                    break;
                case "Slider":
                    $currentNode.createSlider();
                    break;
                    case "ChoiceMedia":
                    $currentNode.createChoiceMedia();
                    break;
                case "Fill":
                    $currentNode.createFill();
                    break;
            }
        } 
    });
    
    // chạy sự kiện ban đầu
    $(this).startQuestionEvent();


    
    

    function validateQuestion() {
        //Trả về giá trị hợp lệ hay không
        var isValid = true;

        // Lấy thẻ đầu tiên bị lỗi
        var focus = null;
        
        // Kiểm tra sự kiện đang sử dụng
        var url = $("#questionForm").attr("action").split("/");
        var isCreate = url[url.length - 2].indexOf("Create") != -1;
        
        // Trước khi kiểm tra thì xóa các thông báo cũ
        $("#Validation").text("");


        // Kiểm tra id của câu hỏi
        if (!isCreate) {
            if ($("#QuestionId").isNullOrEmpty()) {
                focus = $("#QuestionId");
                isValid = false;
                $("#Validation").text("Không tìm thấy mã của câu hỏi. Vui lòng tải lại trang.");
            }
        }

        // Kiểm tra tiêu đề câu hỏi khác rỗng
        if ($("#QuestionTitle").isNullOrEmpty()) {
            focus = focus == null ? $("#questionForm").first() : focus;
            isValid = false;
            $("#QuestionTitleError").text("Tiêu đề không được để trống!");
        } else {
            $("#QuestionTitleError").text("");
        }

        // Kiểm tra chọn nhóm câu hỏi
        if ($("#GroupId").isNullOrEmpty()) {
            focus = focus == null ? $("#GroupId") : focus;
            isValid = false;
            $("#GroupIdError").text("Chưa chọn nhóm câu hỏi!");
        } else {
            $("#GroupIdError").text("");
        }

        // Kiểm tra hợp lệ cho từng loại câu hỏi
        $("#eventQuestion li.questions").each(function () {
            var result = $(this).checkValidateQuestion();
            if (!result.isValid) {
                isValid = result.isValid;
                focus = focus == null ? $(result.focus) : focus;
            }
        });

        if (!isValid) {
            $("html, body").animate({ scrollTop: parseInt($(focus).offset().top - 50) }, 'fast');
        }
        return isValid;
    }
    
    // Sự kiện submit
    $(".submit-button").on("click", function(e) {
        // chặn hành động mặc định của nút submit
        e.preventDefault();

        // Không cho bấm submit 2 lần
        $(".submit-button").each(function() { $(this).attr("disabled", "disabled") });

        // Lấy text từ tinymce bỏ vào 
        tinyMCE.triggerSave();

        // Lấy url để biết là edit hay create
        var url = $("#questionForm").attr("action");
        var btnValue = $(this).attr("value");
        // Kiểm tra hợp lệ dữ liệu
        if (validateQuestion()) {
            var data = $("#questionForm").getQuestion();
            $.ajax({
                type: "POST",
                url: url,
                data: { "data": data },
                success: function(response) {
                    if (response.Success) {
                        // Lấy giá trị để chuyển sang trang thêm mới hay đến trang về danh sách

                        if (btnValue == "save-new")
                            window.location = "/Adm/Question/Create/" + $("#GroupId").val();
                        else
                            window.location = "/Adm/Question/Index?groupId=" + $("#GroupId").val();
                    } else {
                        $("#Validation").html(response.Message);
                        $("html, body").animate({ scrollTop: 0 }, "slow");
                        $(".submit-button").each(function() { $(this).removeAttr("disabled") });
                    }

                },
                error: function(xhr, ajaxOptions, thrownError) {
                    alert(xhr.responseText);
                }
            });
        } else {
            $(".submit-button").each(function() { $(this).removeAttr("disabled") });
        }
    });
});