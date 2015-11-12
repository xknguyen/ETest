$(function () {
    $.fn.createDatable = function () {
        return $(this).DataTable(
        {
            "dom": 'T<"clear">lfrtip',
            "language": {
                "lengthMenu": "Số dòng hiển thị _MENU_",
                "zeroRecords": "Không có học viên nào nào",
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
                //if ($.inArray(data[2], selected) != -1) {
                //    $(row).addClass('selected');
                //}
            }
        });
    }

    var currentUserTable = $("#tblAccounts").createDatable();

    function getCurrentIds() {
        var ids = [];
        currentUserTable.rows().every(function () {
            var d = this.data();
            ids.push(d[0].trim());
        });
        return ids.join();
    }

    var userSelected = [];
    var usernames = [];
    // ReSharper disable once NativeTypePrototypeExtending
    Array.prototype.containSelecteds = Array.prototype.contains || function (obj) {
        var i, l = this.length;
        for (i = 0; i < l; i++) {
            if (this[i][0] == obj) return true;
        }
        return false;
    };
    $.fn.createUserTable = function (response) {
        $("#AddUserContent").html(response);
        $("#AddUserContent").pagedClick();
        $("#numberUser").text(userSelected.length);
        $("#AddUserContent").userTableClick();
        // đánh dấu các dòng đã được chọn
        var trs = $("#tblUsers").find("tbody").first().find("tr");
        $(trs).each(function () {
            var tds = $(this).find("td");
            if (userSelected.containSelecteds($(tds[0]).html())) {
                $(this).addClass("selected");
            }
        });
    }

    $.fn.pagedClick = function () {
        var a = $(this).find("a");
        $(a).on("click", function (e) {
            e.preventDefault();
            var li = $(this).parent();
            var cla = $(li).attr("class");
            if (cla == null || cla.indexOf("disabled") == -1) {
                var url = $(this).attr("href");
                if (url != null) {
                    $.ajax({
                        type: "POST",
                        url: url,
                        data: {
                            ids: getCurrentIds
                        }
                    }).done(function (response) {
                        $("#AddUserContent").createUserTable(response);
                    });
                }
            }
        });
    }


    $.fn.userTableClick = function () {
        var trs = $(this).find("tbody").first().find("tr");
        $(trs).on("click", function () {
            var thisClass = $(this).attr("class");
            var tds = $(this).find("td");

            if (thisClass == null || thisClass.indexOf("selected") == -1) {
                $(this).addClass("selected");
                var data = [$(tds[0]).html(), $(tds[1]).html(), $(tds[2]).html(), $(tds[3]).html()];
                userSelected.push(data);
                usernames.push($(tds[0]).html().trim());
            } else {
                $(this).removeClass("selected");
                userSelected = jQuery.grep(userSelected, function (value) {
                    return value[0] != $(tds[0]).html();
                });
                usernames = jQuery.grep(usernames, function (value) {
                    return value != $(tds[0]).html().trim();
                });
            }
            $("#numberUser").text(userSelected.length);
        });
    }



    $("#show-users").on("click", function (e) {
        e.preventDefault();
        $("#AddUserContent").html("");
        userSelected = [];
        usernames = [];
        var url = "/Adm/Course/GetList";
        $.ajax({
            type: "POST",
            url: url,
            data: {
                ids: getCurrentIds
            }
        }).done(function (response) {
            $("#AddUserContent").createUserTable(response);
            $("#showAddUserForm").click();
        });
    });


    // Them menber
    $("#saveUserButton").on("click", function (e) {
        e.preventDefault();

        if (userSelected.length > 0) {
            var pathname = window.location.pathname;
            var paths = pathname.split("/");
            var courseId = paths[paths.length - 1];
            var users = usernames.join();
            $.ajax({
                type: "POST",
                url: "/Adm/Cousre/GetList",
                data: {
                    courseId: courseId,
                    usernames: users
                }
            }).done(function (response) {
                if (response.Success) {
                    for (var i = 0; i < userSelected.length; i++) {
                        var node = currentUserTable.row.add(userSelected[i]).draw().node();
                        var tds = $(node).find("td");
                        $(tds[2]).addClass("hidden-sm hidden-xs");
                    }
                    $("#closeUserButton").click();
                } else {
                    alert(response.Message);
                }
            });


        } else {
            alert("Bạn chưa chọn người dùng nào");
        }

    });
});
