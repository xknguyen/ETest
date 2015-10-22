$(function() {
    $.fn.createDatable = function (selected) {
        return $(this).DataTable(
        {
            "lengthMenu": [10],
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
        "rowCallback": function( row, data ) {
            if ( $.inArray(data[1], selected) != -1 ) {
                $(row).addClass('selected');
            }
        }
        });
    }
    $.fn.createReorderDatable = function () {
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
    $.fn.createTiny = function() {
        tinymce.init({
        selector: "textarea",
        entity_encoding: "raw",
        theme: "modern",
        height: 100,
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

    $.fn.createTreeView = function(data) {
        $(this).html("");
        $(this).treeview({
            color: "#428bca",
            enableLinks: true,
            showBorder: false,
            data: data
        });
    }
});