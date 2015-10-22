using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Core.Utilities;
using ETest.Areas.Adm.Models;
using ETest.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
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
                                         || x.Description.Contains(keyword));
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
        protected override bool OnUpdateToggle(string propName, bool value, object[] keys)
        {
            var query = string.Format("UPDATE dbo.Tests SET {0} = @p0 WHERE TestId = @p1", propName);
            return DbContext.Database.ExecuteSqlCommand(query, value, keys[0]) > 0;
        }

        public ActionResult Create()
        {
            var question = DbContext.Questions.ToList();
            var emptytest = new Test()
            {
                Actived = true,
                TestDetails = new List<TestDetail>()
                {
                    new TestDetail()
                    {
                        Question = question[0]
                    },
                    new TestDetail()
                    {
                        Question = question[2]
                    }
                }
            };
            InitFormData(emptytest);
            return View(emptytest);
        }

        [HttpPost]
        public ActionResult GetGroupForUser()
        {
            var userId = User.Identity.GetUserId();
            var groups = DbContext.Groups.Where(s => s.TeacherId == userId && !s.ParentGroupId.HasValue).ToList();
            var data = new List<DataGroupModel>();
            foreach (var g in groups)
            {
                data.Add(new DataGroupModel(g));
            }
            return Json(JsonConvert.SerializeObject(data,
                Formatting.Indented,
                new JsonSerializerSettings {DefaultValueHandling = DefaultValueHandling.Ignore}));

        }

        public ActionResult GetQuestion(int? id)
        {
            var userId = User.Identity.GetUserId();
            var questions = DbContext.Questions.Where(s =>s.GroupId == id && s.Group.TeacherId == userId ).ToList();

            var data = new List<DataQuestionModel>();
            foreach (var question in questions)
            {
                data.Add(new DataQuestionModel(question));
            }
            var group = DbContext.Groups.FirstOrDefault(s => s.GroupId == id);
            ViewBag.GroupName = group != null ? group.GroupName : "";

            return PartialView("_QuestionTable",data);
        }


        private void InitFormData(Test test)
        {
            var id = User.Identity.GetUserId();
            var courses = DbContext.Courses.Where(s => s.TeacherId == id).ToList();
            ViewBag.CourseId = test.CourseId > 0
                ? new SelectList(courses, "CourseId", "CourseName", test.CourseId)
                : new SelectList(courses, "CourseId", "CourseName", (object) null);
        }

    }
}