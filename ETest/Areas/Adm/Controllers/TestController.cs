using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
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
        public CheckRoleResult CheckCourseRole(long? id)
        {
            var result = new CheckRoleResult();
            if (id == null)
            {
                result.ActionResult = RedirectErrorPage(Url.Action("Index", "Dashboard"));
                result.IsValid = false;
            }
            else
            {
                Course course = DbContext.Courses.Find(id);
                if (course == null)
                {
                    result.ActionResult = RedirectErrorPage(Url.Action("Index", "Dashboard"));
                    result.IsValid = false;
                }
                else
                {
                    if (course.TeacherId != User.Identity.GetUserId())
                    {
                        result.ActionResult = RedirectAccessDeniedPage(Url.Action("Index", "Dashboard"));
                        result.IsValid = false;
                    }
                    else
                    {
                        result.Course = course;
                        result.IsValid = true;
                    }
                }
            }
            return result;
        }
        // GET: Adm/Text
        public ActionResult Index(string keyword, int? page, int? pageSize, int? status, long? courseId)
        {
            var result = CheckCourseRole(courseId);
            if (!result.IsValid) return result.ActionResult;
            var tests = result.Course.Tests.AsQueryable();

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
            ViewBag.CourseId = courseId;
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

        public ActionResult Create(long? id)
        {
            var result = CheckCourseRole(id);
            if (!result.IsValid) return result.ActionResult;
            var emptytest = new Test()
            {
                Actived = true,
                TestStart = DateTime.Now,
                TestEnd = DateTime.Now.AddDays(7),
                MixedQuestions = true,
                SubmitNo = 1,
                GradeType = GradeType.Maximum,
                TestDetails = new List<TestDetail>(),
                Course = result.Course,
                CourseId = result.Course.CourseId
            };
            InitFormData(emptytest);
            return View(emptytest);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(string data)
        {
            try
            {
                var test = new Test(data);
                // kiểm tra hợp lệ dữ liệu

                var course = DbContext.Courses.FirstOrDefault(s => s.CourseId == test.CourseId);
                if (course == null)
                {
                    return Json(new
                    {
                        Message = "Khóa học này đã bị xóa!",
                        Success = false
                    });
                }

                if (course.TeacherId != User.Identity.GetUserId())
                {
                    return Json(new
                    {
                        Message = "Bạn không có quyền tạo bài kiểm tra cho khóa học này!",
                        Success = false
                    });
                }
                test.TestType = TestType.Test;
                test.Score = 100;
                DbContext.Tests.Add(test);
                var result = DbContext.SaveChanges();
                if (result > 0)
                {
                    return Json(new
                    {
                        Message = "Thêm thành công.",
                        Success = true
                    });
                }
            }
            catch
            {
                //
            }


            return Json(new
            {
                Message = "Đã có lỗi xảy ra! Vui lòng thử lại sau.",
                Success = false
            });
        }

        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return RedirectErrorPage(Url.Action("Index"));
            }
            var test = DbContext.Tests.FirstOrDefault(s => s.TestId == id.Value);
            if (test == null)
            {
                return RedirectErrorPage(Url.Action("Index"));
            }
            if (test.Course.TeacherId != User.Identity.GetUserId())
            {
                return RedirectAccessDeniedPage(Url.Action("Index"));
            }
            InitFormData(test);
            return View(test);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(string data)
        {
            try
            {
                var testTemp = new Test(data);
                var testDb = DbContext.Tests.FirstOrDefault(s => s.TestId == testTemp.TestId);
                if (testDb != null)
                {
                    if (testDb.Course.TeacherId != User.Identity.GetUserId())
                    {
                        return Json(new
                        {
                            Message = "Bạn không có quyền sử dụng khóa học này!",
                            Success = false
                        });
                    }

                    UpdateDetail(testDb.TestDetails);
                    testDb.TestTitle = testTemp.TestTitle;
                    testDb.Description = testTemp.Description;
                    testDb.TestStart = testTemp.TestStart;
                    testDb.TestEnd = testTemp.TestEnd;
                    testDb.TestTime = testTemp.TestTime;
                    testDb.SubmitNo = testTemp.SubmitNo;
                    testDb.Actived = testTemp.Actived;
                    testDb.MixedQuestions = testTemp.MixedQuestions;
                    testDb.GradeType = testTemp.GradeType;
                    testDb.TestDetails = testTemp.TestDetails;

                    DbContext.Entry(testDb).State = EntityState.Modified;
                    var result = DbContext.SaveChanges();

                    if (result > 0)
                    {
                        return Json(new
                        {
                            Message = "Cập nhật thành công.",
                            Success = true
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        Message = "Bài kiểm tra này đã bị xóa.",
                        Success = false
                    });
                }


            }
            catch (Exception )
            {
                //
            }

            return Json(new
            {
                Message = "Đã có lỗi xảy ra! Vui lòng thử lại sau.",
                Success = false
            });
        }

        [HttpPost]
        public ActionResult GetGroupForUser()
        {
            var userId = User.Identity.GetUserId();
            var groups = DbContext.Groups.Where(s => s.Course.TeacherId == userId && !s.ParentGroupId.HasValue).ToList();
            var data = new List<DataGroupModel>();
            foreach (var g in groups)
            {
                data.Add(new DataGroupModel(g));
            }
            return Json(JsonConvert.SerializeObject(data,
                Formatting.Indented,
                new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore }));

        }

        public ActionResult GetQuestion(long? id)
        {
            var userId = User.Identity.GetUserId();
            var questions = DbContext.Questions.Where(s => s.GroupId == id && s.Group.Course.TeacherId == userId).ToList();

            var data = new List<DataQuestionModel>();
            foreach (var question in questions)
            {
                data.Add(new DataQuestionModel(question));
            }
            var group = DbContext.Groups.FirstOrDefault(s => s.GroupId == id);
            ViewBag.GroupName = group != null ? group.GroupName : "";

            return PartialView("_QuestionTable", data);
        }

        private void InitFormData(Test test)
        {
            var id = User.Identity.GetUserId();
            var courses = DbContext.Courses.Where(s => s.TeacherId == id).ToList();
            ViewBag.Id = test.CourseId;
            var types = new List<SelectListItem>
            {
                new SelectListItem {Selected = false, Text = "Điểm cao nhất", Value = "0"},
                new SelectListItem {Selected = false, Text = "Điểm trung bình", Value = "1"},
                new SelectListItem {Selected = false, Text = "Điểm thấp nhất", Value = "2"},
            };
            var type = (int)test.GradeType;
            ViewBag.GradeType = type >= 0
                ? new SelectList(types, "Value", "Text", type)
                : new SelectList(types, "Value", "Text", (object)null);
        }

        private void UpdateDetail(IEnumerable<TestDetail> oldList)
        {
            DbContext.TestDetails.RemoveRange(oldList.ToList());
        }

        public ActionResult Preview(long? id)
        {
            if (!id.HasValue)
            {
                return RedirectErrorPage(Url.Action("Index", "Course"));
            }

            var test = DbContext.Tests.FirstOrDefault(s => s.TestId == id.Value);
            if (test != null)
            {
                if (test.Course.TeacherId != User.Identity.GetUserId())
                {
                    return RedirectAccessDeniedPage(Url.Action("Index", "Course"));
                }
                return View(test);
            }
            return RedirectErrorPage(Url.Action("Index", "Course"));
        }

        [HttpPost]
        public ActionResult GetTestPreview(long? id)
        {
            if (!id.HasValue)
            {
                return RedirectErrorPage(Url.Action("Index", "Course"));
            }

            var test = DbContext.Tests.FirstOrDefault(s => s.TestId == id.Value);
            if (test != null)
            {
                if (test.Course.TeacherId != User.Identity.GetUserId())
                {
                    return RedirectAccessDeniedPage(Url.Action("Index", "Course"));
                }
                return PartialView("_TestPreview",test);
            }
            return RedirectErrorPage(Url.Action("Index", "Course"));
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GradeTest(string data)
        {
            var answerSheet = new AnswerSheet(data) {TestTakerId = User.Identity.GetUserId()};
            var score = 0f;
            var test = DbContext.Tests.FirstOrDefault(s => s.TestId == answerSheet.TestId);
            if (test == null)
            {
                return Json(new
                {
                    Message = "Đã có lỗi xảy ra! Vui lòng thử lại sau.",
                    Success = false
                });
            }
            foreach (var answer in answerSheet.Answers)
            {
                var testDetail = test.TestDetails.FirstOrDefault(s => s.QuestionId == answer.QuestionId);
                if (testDetail != null) score += testDetail.GradeTest(answer);
            }

            return Json(new
            {
                Message = score,
                Success = true
            });
        }

        public ActionResult Statistic(long? id)
        {
            if (!id.HasValue)
            {
                return RedirectErrorPage(Url.Action("Index", "Course"));
            }

            var test = DbContext.Tests.FirstOrDefault(s => s.TestId == id.Value);
            if (test != null)
            {
                if (test.Course.TeacherId != User.Identity.GetUserId())
                {
                    return RedirectAccessDeniedPage(Url.Action("Index", "Course"));
                }
                var students = test.Course.Students;

                foreach (var student in students)
                {
                    student.AnswerSheets = student.AnswerSheets.Where(s => s.TestId == test.TestId).ToList();
                }
                ViewBag.Test = test;
                return View(students);
            }
            return RedirectErrorPage(Url.Action("Index", "Course"));
        }
    }
}