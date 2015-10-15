using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Utilities;
using PagedList;

namespace ETest.Areas.Adm.Controllers
{
    public class TestController : AdminController
    {
        // GET: Adm/Text
        public ActionResult Index(string keyword, int? page, int? pageSize, int? status)
        {
            var tests = DbContext.Tests.AsQueryable();

            // Tìm nhà cung cấp theo từ khóa (keyword) bằng cách kiểm
            // tra nó có xuất hiện trong tên, mô tả hay địa chỉ của NCC
            if (!string.IsNullOrEmpty(keyword))
            {
                tests = tests.Where(x => x.TestTitle.Contains(keyword)
                                         || x.TestDescription.Contains(keyword));
                var date = DataUtil.ToDateTime(keyword);
                if (date != DateTime.MinValue)
                {
                    tests =
                        tests.Where(
                            x =>
                                x.TestStart.Day == date.Day && x.TestStart.Month == date.Month &&
                                x.TestStart.Year == date.Year);
                }
            }

            // Nếu chỉ số trang và số mẫu tin trên mỗi trang không được
            // thiết lập thì gán giá trị mặc định tương ứng là 1 và 5.
            if (!page.HasValue || page.Value < 1) page = 1;
            if (!pageSize.HasValue || pageSize < 10) pageSize = 10;
            if (!status.HasValue) status = 0;

            // Lưu lại từ khóa, số mẫu tin/trang để hiển thị trên trang web
            ViewBag.Keyword = keyword;
            ViewBag.SupStatus = new SelectList(new List<Object>()
            {
                new {text = "Chọn tất cả", value = 0},
                new {text = "Hoạt động", value = 1},
                new {text = "Đang khóa", value = 2}
            }, "value", "text", status);
            ViewBag.PageSize = new SelectList(new[] { 10, 25, 50, 100 }, pageSize);
            ViewBag.CurrentPageSize = pageSize.Value;
            ViewBag.CurrentStatus = status;

            // Sắp tăng các nhà cung cấp theo tên và thực hiện việc phân trang bằng 
            // cách sử dụng thư viện PagedList.MVC
            if (status.Value == 0)
            {
                var data1 = tests.OrderByDescending(x => x.TestId)
                    .ToPagedList(page.Value, pageSize.Value);
                return View(data1);
            }
            var isActived = status.Value == 1;
            var data = tests.OrderByDescending(x => x.TestId).Where(x => x.Actived == isActived)
                                .ToPagedList(page.Value, pageSize.Value);
            return View(data);
        }

    }
}