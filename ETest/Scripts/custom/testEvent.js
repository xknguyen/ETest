$(function() {
    var currentEditor = null;
    var currentRow = null;
    // Tạo tiny
    $.fn.createTiny = function () {
        tinymce.init({
            selector: "textarea.description",
            entity_encoding: "raw",
            theme: "modern",
            height: 150,
            menubar: false,
            plugins: [
                "link image lists charmap print preview hr anchor pagebreak spellchecker",
                "searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking",
                "save table contextmenu directionality emoticons template paste textcolor"
            ],
            toolbar: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist | link addPicture | forecolor backcolor",
            style_formats: [
                { title: "Bold text", inline: "b" },
                { title: "Red text", inline: "span", styles: { color: "#ff0000" } },
                { title: "Red header", block: "h1", styles: { color: "#ff0000" } },
                { title: "Example 1", inline: "span", classes: "example1" },
                { title: "Example 2", inline: "span", classes: "example2" },
                { title: "Table styles" },
                { title: "Table row 1", selector: "tr", classes: "tablerow1" }
            ],
            setup: function (editor) {
                editor.addButton('addPicture', {
                    icon: "image",
                    tooltip: "Thêm hình ảnh",
                    onclick: function () {
                        $(this).showPictureForm();
                        $("#selectMediaButton").attr("data-type", "tinymce");
                        currentEditor = editor;
                    }
                });
            }
        });
    }

    $("#selectMediaButton").on("click", function (e) {
        e.preventDefault();
        var img = $("#fileContent").find("img.selected").first();
        if (img.length != 0) {
            var type = $("#selectMediaButton").attr("data-type");
            switch (type) {
                case "tinymce":
                    if (currentEditor != null) {
                        // get img
                        var im = "<img src='" + $(img).attr("src") + "' data-mce-selected='1' height='100'>";
                        currentEditor.insertContent(im);
                    }
                    break;
            }
            $("#closeMediaButton").click();
        } else {
            alert("Bạn chưa chọn hình ảnh nào");
        }
    });


    function parseNumber(number) {
        var i = parseInt(number);
        return isNaN(i) ? 0 : i;

    }

    function getDate(dateString) {
        var dateStrings = dateString.split(" ");
        var dates = dateStrings[0].split("/");
        var yyyy = parseNumber(dates[2]);
        var mM = parseNumber(dates[1]);
        var dd = parseNumber(dates[0]);
        var hours = dateStrings[1].trim().split(":");
        var hh = parseNumber(hours[0]);
        var mm = parseNumber(hours[1]);
        var dat = new Date(yyyy, mM - 1, dd, hh, mm, 0, 0);
        return dat;
    }

    function addMinutes(date, minutes) {
        return new Date(date.getTime() + minutes * 60000);
    }

    $.fn.getDate = function() {
        var dateString = $(this).val().trim();
        return getDate(dateString);
    }

    $.fn.getDateAttr = function() {
        var dateString = $(this).attr("value").trim();
        return getDate(dateString);
    }

    var testEnd = null;
    // cài đặt ngày tháng
    var testStart = $("#TestStart").datetimepicker({
        format: "DD/MM/YYYY HH:mm",
        showTodayButton: true
    });

    $("#TestStart").on("dp.change", function (e) {
        var date = addMinutes($("#TestStart").getDate(), $("#TestTime").val());
        $("#TestEnd").data("DateTimePicker").minDate(date);
    });

    testEnd = $("#TestEnd").datetimepicker({
        format: "DD/MM/YYYY HH:mm",
        showTodayButton: true,
        minDate: addMinutes($("#TestStart").getDate(), $("#TestTime").val())
    });
    $("#TestEnd").val($("#TestEnd").attr("value").trim());


    $("#TestTime").on("change paste", function () {
        var time = parseInt($(this).val());
        time = isNaN(time) || time<0 ? 0 : time;
        var date = addMinutes($("#TestStart").getDate(), time);
        $("#TestEnd").data("DateTimePicker").minDate(date);
    });

    $.fn.createReorderDatable = function() {
        return $(this).DataTable(
        {
            rowReorder: true,
            responsive: true,
            "dom": 'T<"clear">lfrtip',
            "language": {
                "lengthMenu": "Số dòng hiển thị _MENU_",
                "zeroRecords": "Không có câu hỏi nào",
                "info": "Đang xem _START_ đến _END_ trong tổng số _TOTAL_ mục",
                "infoEmpty": "Đang xem 0 đến 0 trong tổng số 0 mục",
                "search": "Tìm: ",
                "infoFiltered": " ",
                "paginate": {
                    "first": "Đầu",
                    "previous": "Trước",
                    "next": "Tiếp",
                    "last": "Cuối"
                }
            },
            columnDefs: [
                { orderable: true, className: 'reorder', targets: 0 },
                { orderable: false, targets: '_all' }
            ]
        });
    }

    $.fn.createDatable = function (selected) {
        return $(this).DataTable(
        {
            "lengthMenu": [5],
            "dom": 'T<"clear">lfrtip',
            "language": {
                "lengthMenu": "Số dòng hiển thị _MENU_",
                "zeroRecords": "Không có câu hỏi nào",
                "info": "Đang xem _START_ đến _END_ trong tổng số _TOTAL_ mục",
                "infoEmpty": "Đang xem 0 đến 0 trong tổng số 0 mục",
                "search": "Tìm: ",
                "infoFiltered": " ",
                "paginate": {
                    "first": "Đầu",
                    "previous": "Trước",
                    "next": "Tiếp",
                    "last": "Cuối"
                }
            },
            "rowCallback": function (row, data) {
                if ($.inArray(data[2], selected) != -1) {
                    $(row).addClass('selected');
                }
            }
        });
    }

    $.fn.createTreeView = function (data) {
        $(this).html("");
        $(this).treeview({
            color: "#428bca",
            enableLinks: true,
            showBorder: false,
            data: data
        });
    }

    // Xóa một dòng trong bản danh sách câu hỏi đã chọn
    $.fn.removeQuestion = function (table) {
        $(this).on("click", "a.question-remove", function (e) {
            e.preventDefault();
            var order = parseInt($($(this).parents("tr").find("td")[0]).html().trim()) - 1;
            table.row($(this).parents("tr")).remove().draw();
            table.rows().every(function (rowIdx, tableLoop, rowLoop) {
                if (rowIdx >= order) {
                    var td = $(this.node()).find("td")[0];
                    $(td).html(rowIdx + 1);
                }
            });
            table.draw();
        });
    }

    
    $.fn.showSetScoreForm = function (table) {
        $(this).on("click", "a.question-view", function(e) {
            e.preventDefault();
            $("#tblTests").html("");
            currentRow = $(this).closest("tr").first();
            var testScoreInput = $(currentRow).find("input[name='detailScore']").first();
            var testScore = $(testScoreInput).val();
            testScore = isNaN(testScore) ? 0 : testScore;
            if (testScore != 0) {
                var details = $.parseJSON($(currentRow).find("td.questionDetail").html().trim());
                for (var i = 0; i < details.length; i++) {
                    var trPopup = "<tr><td>Câu " + (i + 1) + "</td><td><input class='form-control' name='detailScore' type='number' min='0' value='" + details[i].Score + "'></td></tr>";
                    $("#tblTests").append(trPopup);
                }
                $("#testScorePopup").text(testScore);
                $("#showScoreForm").click();
            } else {
                alert("Bạn chưa đặt điểm!");
                testScoreInput.focus();
            }
        });
    }

    $("#saveScoreButton").on("click", function(e) {
        e.preventDefault();
        if (currentRow != null) {
        // Lấy tổng Điểm câu hỏi lớn
            var testScoreInput = $(currentRow).find("input[name='detailScore']").first();
            var testScore = $(testScoreInput).val();
            var details = $.parseJSON($(currentRow).find("td.questionDetail").html().trim());
            // Kiểm tra hợp lệ tổng điểm câu hỏi con
            var total = 0;

            var isValid = true;
            $("#tblTests").find("input[name='detailScore']").each(function(index) {
                var s = parseFloat($(this).val());
                if (isNaN(s) || s == 0) {
                    isValid = false;
                } else {
                    details[index].Score = s;
                    total += s;
                }
                
            });
            if (isValid) {
                if (total == testScore) {
                    var td = $(currentRow).find("td.questionDetail").first();
                    $(td).html(JSON.stringify(details));
                    $("#tbQuestions").DataTable().draw();
                    $("#closeScoreButton").click();
                } else {
                    alert("Tổng điểm câu hỏi con phải bằng điểm câu hỏi cha!");
                }
            } else {
                alert("Có câu hỏi con chưa có điểm!");
            }
            
        }
    });


// Hết hàm ready    
});