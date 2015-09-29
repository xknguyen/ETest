using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PagedList;

namespace ETest.Areas.Adm.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AccountController : AdminController
    {
        // GET: Adm/Account
        public ActionResult Index(string keyword, int? page, int? pageSize, int? status)
        {
            var accounts = DbContext.Accounts.AsQueryable();

            // Tìm nhà cung cấp theo từ khóa (keyword) bằng cách kiểm
            // tra nó có xuất hiện trong tên, mô tả hay địa chỉ của NCC
            if (!string.IsNullOrEmpty(keyword))
                accounts = accounts.Where(x => x.UserName.Contains(keyword) ||
                                                 x.Email.Contains(keyword)||
                                                 x.Profile.FirstName.Contains(keyword) ||
                                                 x.Profile.LastName.Contains(keyword) ||
                                                 x.Profile.Identity.Contains(keyword) ||
                                                 x.Profile.Notes.Contains(keyword) );

            // Nếu chỉ số trang và số mẫu tin trên mỗi trang không được
            // thiết lập thì gán giá trị mặc định tương ứng là 1 và 5.
            if (!page.HasValue || page.Value < 1) page = 1;
            if (!pageSize.HasValue || pageSize < 5) pageSize = 5;
            if (!status.HasValue) status = 0;

            // Lưu lại từ khóa, số mẫu tin/trang để hiển thị trên trang web
            ViewBag.Keyword = keyword;
            ViewBag.SupStatus = new SelectList(new List<Object>()
            {
                new {text = "Chọn tất cả", value = 0},
                new {text = "Hoạt động", value = 1},
                new {text = "Đang bị khóa", value = 2}
            }, "value", "text", status);
            ViewBag.PageSize = new SelectList(new[] { 5, 10, 25, 50, 100 }, pageSize);
            ViewBag.CurrentPageSize = pageSize;
            ViewBag.CurrentStatus = status;

            // Sắp tăng các nhà cung cấp theo tên và thực hiện việc phân trang bằng 
            // cách sử dụng thư viện PagedList.MVC
            if (status.Value == 0)
            {
                var data1 = accounts.OrderBy(x => x.UserName)
                    .ToPagedList(page.Value, pageSize.Value);
                return View(data1);
            }
            var isActived = status.Value == 1;
            var data = accounts.OrderBy(x => x.UserName).Where(x => x.Profile.Actived == isActived)
                                .ToPagedList(page.Value, pageSize.Value);
            return View(data);
        }
    }
}