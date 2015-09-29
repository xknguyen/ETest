using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;
using ETest.Models;
using Microsoft.AspNet.Identity;
using PagedList;

namespace ETest.Areas.Adm.Controllers
{
    public class CourseController : AdminController
    {
        public ActionResult Index(string keyword, int? page, int? pageSize, int? status)
        {
            var userId = User.Identity.GetUserId();
            var courses = DbContext.Courses.Where(c=>c.TeacherId == userId).AsQueryable();
            // Tìm nhà cung cấp theo từ khóa (keyword) bằng cách kiểm
            // tra nó có xuất hiện trong tên, mô tả hay địa chỉ của NCC
            if (!string.IsNullOrEmpty(keyword))
                courses = courses.Where(x => x.CourseName.Contains(keyword) ||
                                                 x.Description.Contains(keyword));

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
                new {text = "Không hoạt động", value = 2}
            }, "value", "text", status);
            ViewBag.PageSize = new SelectList(new[] { 5, 10, 25, 50, 100 }, pageSize);
            ViewBag.CurrentPageSize = pageSize;
            ViewBag.CurrentStatus = status;

            // Sắp tăng các nhà cung cấp theo tên và thực hiện việc phân trang bằng 
            // cách sử dụng thư viện PagedList.MVC
            if (status.Value == 0)
            {
                var data1 = courses.OrderBy(x => x.CourseName)
                    .ToPagedList(page.Value, pageSize.Value);
                return View(data1);
            }
            var isActived = status.Value == 1;
            var data = courses.OrderBy(x => x.CourseName).Where(x => x.Actived == isActived)
                                .ToPagedList(page.Value, pageSize.Value);
            return View(data);
        }

        /// <summary>
        /// Get: Adm/Course/Create
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var emptyCourse = new Course() { Actived = true , StartTime = DateTime.Now};
            return View(emptyCourse);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "CourseName, Description, Actived, StartTime, EndTime")] Course course)
        {
            try
            {
                if (course.EndTime.HasValue && course.StartTime >= course.EndTime.Value)
                {
                    ModelState.AddModelError("EndTime","Thời gian kết thúc phải lớn hơn thời gian bắt đầu");
                }

                if (ModelState.IsValid)
                {
                    course.TeacherId = User.Identity.GetUserId();
                    DbContext.Courses.Add(course);
                    if (DbContext.SaveChanges() > 0)
                        return Redirect(null);
                    ModelState.AddModelError("", "Đã có lỗi xảy ra. Vui lòng thử lại sau.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(course);
        }

        protected override bool OnUpdateToggle(string propName, bool value, object[] keys)
        {
            string query = string.Format("UPDATE dbo.Courses SET {0} = @p0 WHERE CourseId = @p1", propName);
            return DbContext.Database.ExecuteSqlCommand(query, value, keys[0]) > 0;
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectErrorPage(Url.Action("Index"));
            }
            var course = DbContext.Courses.Find(id);
            if (course == null)
            {
                return RedirectErrorPage(Url.Action("Index"));
            }
            if (course.TeacherId != User.Identity.GetUserId())
            {
                return RedirectAccessDeniedPage(Url.Action("Index"));
            }
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Course course)
        {
            var courseDb = DbContext.Courses.Find(course.CourseId);
            if (courseDb == null)
            {
                ModelState.AddModelError(string.Empty, "Không thể lưu thay đổi, nhóm đã bị xóa");
            }
            else if (courseDb.TeacherId != User.Identity.GetUserId())
            {
                return RedirectAccessDeniedPage(Url.Action("Index"));
            }

            if (course.EndTime.HasValue && course.StartTime >= course.EndTime.Value)
            {
                ModelState.AddModelError("EndTime", "Thời gian kết thúc phải lớn hơn thời gian bắt đầu");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    TryUpdateModel(courseDb,
                        new[]
                        {"CourseId","CourseName", "Description", "Actived", "StartTime", "EndTime"});
                    DbContext.Entry(courseDb).OriginalValues["RowVersion"] = course.RowVersion;
                    DbContext.Entry(courseDb).State = EntityState.Modified;
                    if (DbContext.SaveChanges() > 0)
                    {
                        return Redirect(null);
                    }
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var clientValues = (Course)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty, "Không thể lưu thay đổi, khóa học đã bị xóa");
                    }
                    else
                    {
                        var databaseValues = (Course)databaseEntry.ToObject();

                        if (databaseValues.CourseName != clientValues.CourseName)
                            ModelState.AddModelError("CourseName", "Giá trị hiện tại: " + databaseValues.CourseName);

                        if (databaseValues.Description != clientValues.Description)
                            ViewBag.CurrentDescription = "Giá trị hiện tại: " + databaseValues.Description;

                        if (databaseValues.EndTime.HasValue && !clientValues.EndTime.HasValue)
                        {
                            ModelState.AddModelError("EndTime",
                                    "Giá trị hiện tại: " + databaseValues.EndTime.Value.ToString("dd/MM/yyyy"));
                        } else if (!databaseValues.EndTime.HasValue && clientValues.EndTime.HasValue)
                        {
                            ModelState.AddModelError("EndTime",
                                "Giá trị hiện tại: chưa có");
                        }
                        else if(clientValues.EndTime.HasValue && databaseValues.EndTime.HasValue && databaseValues.EndTime.Value != clientValues.EndTime.Value)
                        {
                            ModelState.AddModelError("EndTime",
                                    "Giá trị hiện tại: " + databaseValues.EndTime.Value.ToString("dd/MM/yyyy"));
                        }

                        if (databaseValues.StartTime != clientValues.StartTime)
                            ModelState.AddModelError("StartTime", "Giá trị hiện tại: " + databaseValues.StartTime.ToString("dd/MM/yyyy"));

                        if (databaseValues.Actived != clientValues.Actived)
                            ModelState.AddModelError("Actived", "Giá trị hiện tại: " + databaseValues.Actived);

                        ModelState.AddModelError(string.Empty, "Nội dung bạn đang cập nhật đã được cập nhật bởi một người khác. "
                           + "Những giá trị hiện tại trong CSDL sẽ được hiển thị trên màn hình. "
                           + "Nếu bạn vẫn muốn cập nhật nó thì nhấn lại nút Lưu. ");

                        // ReSharper disable once PossibleNullReferenceException
                        course.RowVersion = databaseValues.RowVersion;
                    }
                }
            }
            return View(course);
        }
    }
}