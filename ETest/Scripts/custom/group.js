$(document).ready(function () {
    $('#groups').on("click", "[data-toggle-state=true]", function (e) {
        var $this = $(this);
        var url = $this.attr('data-url');
        var args = $this.attr('data-args');
        $.post(url,
            { "args": args },
            function (data) {
                if (data.Result) {
                    $this.attr('data-args', data.Messege);
                    $this.toggleClass('true');
                } else {
                    alert(data.Message);
                }
            });
    });
    var htmlOrgirial;
    $('#groups').nestable({
        group: 1, // Chỉ có 1 nhóm được kéo thả
        maxDepth: 5, // Tối đa là 3 mức
        dragBegin: function () {
            htmlOrgirial = $('#groups').html();
        },
        dragFinished: function (currentNode, parentNode) {
            var pid = 0;
            // Xác định phần tử cha
            if (parentNode) {
                parentNode = $(parentNode);
                pid = parentNode.data('id');
            } else parentNode = $('#groups');

            //alert(parentNode.data('id'));

            // Tìm các phần tử li cùng cấp
            var list = parentNode.children('ol');
            var items = $(list).children('li'), arrayIds = [];
            // alert(items.join());
            // Lấy id của từng category trong các phần tử li
            // đưa vào mảng để gửi lên server xử lý
            items.each(function () {
                arrayIds.push($(this).data('id'));
            });
            //  alert(arrayIds.join());
            // Gửi id của category bị thay đổi vào danh sách
            // id của các category cùng cấp (để sắp xếp).
            $.post('/Group/Reorder',
                {
                    "cid": $(currentNode).data('id'),
                    "pid": pid,
                    "siblings": arrayIds
                },
                function (data) {
                    if (!data) {
                        $('#groups').html(htmlOrgirial);
                    }
                }
            );
        }
    });

    $('div.ibox-tools').on('click', function (e) {
        var target = $(e.target),
            action = target.data('action');
        if (action === 'expand-all') {
            $('.dd').nestable('expandAll');
        }
        if (action === 'collapse-all') {
            $('.dd').nestable('collapseAll');
        }
    });
});