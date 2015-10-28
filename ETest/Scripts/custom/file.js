﻿$(function () {
    $.fn.createTreeViewFolder = function (response, type) {
        $(this).html("");
        $(this).treeview({
            color: "#428bca",
            levels: 2,
            enableLinks: true,
            showBorder: false,
            data: response
        });

        $(this).on("click", "a", function (e) {
            e.preventDefault();
        });

        $(this).on("click", "li", function () {
            $("#fileContent").html("");
            var a = $(this).find("a").attr("href");
            var url = "/Adm/File/GetFile";
            $.ajax({
                type: "POST",
                url: url,
                data: {
                    path: a,
                    type: type
                }
            }).done(function (response) {
                var list = jQuery.parseJSON(response);
                for (var i = 0; i < list.length; i++) {
                    $("#fileContent").appendPicture(list[i]);
                }
                //$('.file-box').each(function () {
                //    animationHover(this, 'pulse');
                //});
            });
        });
    }
    $.fn.showPictureForm = function() {
        $(this).showMediaForm("picture");
    }

    $.fn.showVideoForm = function () {
        $(this).showMediaForm("video");
    }

    $.fn.showAllForm = function () {
        $(this).showMediaForm("all");
    }

    $.fn.showMediaForm = function(type) {
        $(this).on("click", function(e) {
            e.preventDefault();
            $.ajax({
                type: "POST",
                url: "/Adm/File/GetDirectory"
            }).done(function (response) {
                $("#treeviewFolder").createTreeViewFolder(response, type);
                var li = $("#treeviewFolder").find("li").first();
                if (li != null)
                    $(li).click();
                $("#showMediaForm").click();
            });
        });
    }

    $.fn.appendPicture = function (data) {
        var datahtml = $("#fileBoxTemp").html();
        $(this).append(datahtml);
        var last = $(this).find(".file-box").last();
        var img = $(last).find("img").first();
        $(img).attr("alt", data.text);
        $(img).attr("title", data.text);
        $(img).attr("src", data.href);
    }

    $("#fileContent").on("click", "a.image-remove", function(e) {
        e.preventDefault();
        var img = $(this).closest("div.file").first().find("img").first();
       
        var src = $(img).attr("src");
        $.ajax({
            type: "POST",
            url: "/Adm/File/DeleteFile",
            data: {
                path: src
            }
        }).done(function(response) {
            if (response.Success) {
                var parent = $(img).closest("div.file-box").first();
                $(parent).remove();
            } else {
                alert(response.Message);
            }
        });
    });

    $("#fileContent").on("click", "img", function () {
        var thisClass = $(this).attr("class");
        if (thisClass.indexOf("selected") == -1) {
            $("#fileContent").find(".selected").each(function () {
                $(this).removeClass("selected");
            });
            $(this).addClass("selected");
        } else {
            $(this).removeClass("selected");
        }
    });

    $.fn.hidenBox = function(isHidden) {
        $(this).attr("class", "");
        $(this).addClass("row");
        $(this).addClass("animated");
        $(this).addClass(isHidden ? "fadeOutUp" : "fadeInUp");
        $(this).val("");
        if (isHidden) {
            $(this).addClass("hidden");
        }
    }

    // Tạo thư mục
    $("#createFolder").on("click", function(e) {
        e.preventDefault();
        $("#newFolderBox").hidenBox(false);
    });

    $("#cancelNewFolder").on("click", function(e) {
        e.preventDefault();
        $("#newFolderBox").hidenBox(true);
    });

    $("#addNewFolder").on("click", function (e) {
        e.preventDefault();
        // Lấy tên thu muc mói
        var newName = $("#newFolderName").val();
        if (newName == null || newName == "") {
            $("#newFolderName").focus();
        } else {
            // lấy thư mục hiện tại
            var curLi = $("#treeviewFolder").find("li.node-selected").first();
            var path = $(curLi).find("a").first().attr("href");

            if (path != null) {
                path += "\\" + newName;
                $.ajax({
                    type: "POST",
                    url: "/Adm/File/CreateFolder",
                    data: {
                        path: path
                    }
                }).done(function(response) {
                    if (response.Success) {
                        $.ajax({
                            type: "POST",
                            url: "/Adm/File/GetDirectory"
                        }).done(function(response) {
                            $("#treeviewFolder").createTreeViewFolder(response, "picture");
                            var li = $("#treeviewFolder").find("li").first();
                            if (li != null)
                                $(li).click();
                            $("#newFolderBox").hidenBox(true);
                        });
                    } else {
                        alert(response.Message);
                    }
                });
            } else {
                alert("Bạn chưa chọn thư mục!");
            }
        }
    });

    $("#deleteFolder").on("click", function(e) {
        e.preventDefault();
        // lấy thư mục hiện tại
        var curLi = $("#treeviewFolder").find("li.node-selected").first();
        var path = $(curLi).find("a").first().attr("href");

        if (path != null) {
            $.ajax({
                type: "POST",
                url: "/Adm/File/DeleteFolder",
                data: {
                    path: path
                }
            }).done(function (response) {
                if (response.Success) {
                    $.ajax({
                        type: "POST",
                        url: "/Adm/File/GetDirectory"
                    }).done(function (response) {
                        $("#treeviewFolder").createTreeViewFolder(response, "picture");
                        var li = $("#treeviewFolder").find("li").first();
                        if (li != null)
                            $(li).click();
                    });
                } else {
                    alert(response.Message);
                }
            });
        } else {
            alert("Bạn chưa chọn thư mục!");
        }
    });


    // UploadFile
    $("#showUploadFile").on("click", function(e) {
        e.preventDefault();
        $("#uploadBox").hidenBox(false);
    });
    $("#cancelUpload").on("click", function (e) {
        e.preventDefault();
        $("#uploadBox").hidenBox(true);
    });


});