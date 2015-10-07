﻿$(function () {
    $.fn.isNullOrEmpty = function () {
        var value = $(this).val();
        if (value == null || value == "")
            return true;
        return false;
    }
    
    $.fn.createTiny = function (name, height) {
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

});


