$(document).ready(function() {
    $('.tbldata tr').on("click", "[data-toggle-state=true]", function(e) {
        var $this = $(this);
        var url = $this.attr('data-url');
        var args = $this.attr('data-args');
        $.post(url,
            { "args": args },
            function(data) {
                if (data.Result) {
                    var check = $('#cb' + args);
                    var $checkbox = $(check);
                    $this.attr('id', data.message);
                    $this.attr('data-args', data.Message);
                    var id = 'cb' + $this.attr('data-args');
                    $checkbox.attr('id', id);
                    $checkbox.attr('value', $this.attr('data-args'));
                    //alert(id);
                    $this.toggleClass('true');
                } else {
                    alert(data.Message);
                }
            }
        );
    });
});