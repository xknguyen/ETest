using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Utilities;
using ETest.Areas.Adm.Models;
using ETest.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;

namespace ETest.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index(string keyword, int? page, int? pageSize)
        {
            var id = User.Identity.GetUserId();
            var account = DbContext.Accounts.FirstOrDefault(s => s.Id == id);
            var testTemps = new List<Test>();
            var today = DateTime.Now;
            // ReSharper disable once PossibleNullReferenceException
            foreach (var course in account.Courses)
            {
                foreach (var test in course.Tests.Where(s => s.TestStart <= today && s.TestEnd >= today && s.Actived))
                {
                    var submitNo = account.AnswerSheets.Where(s => s.TestId == test.TestId).ToList().Count;
                    if (submitNo == 0 || submitNo < test.SubmitNo )
                        testTemps.Add(test);
                }
            }
            var tests = testTemps.AsQueryable();
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
            if (!page.HasValue || page.Value < 1) page = 1;
            if (!pageSize.HasValue || pageSize < 10) pageSize = 10;
            ViewBag.Keyword = keyword;
            ViewBag.PageSize = new SelectList(new[] { 10, 25, 50, 100 }, pageSize);
            ViewBag.CurrentPageSize = pageSize.Value;
            var data = tests.OrderByDescending(x => x.TestId).ToPagedList(page.Value, pageSize.Value);
            return View(data);
        }

        public ActionResult Course(string keyword, int? page, int? pageSize)
        {
            var id = User.Identity.GetUserId();
            var account = DbContext.Accounts.FirstOrDefault(s => s.Id == id);
            // ReSharper disable once PossibleNullReferenceException
            var courses = account.Courses.Where(s => s.Actived);
            if (!string.IsNullOrEmpty(keyword))
            {
                courses = courses.Where(x => x.CourseName.Contains(keyword)
                                         || x.Description.Contains(keyword));

            }
            if (!page.HasValue || page.Value < 1) page = 1;
            if (!pageSize.HasValue || pageSize < 10) pageSize = 10;
            ViewBag.Keyword = keyword;
            ViewBag.PageSize = new SelectList(new[] { 10, 25, 50, 100 }, pageSize);
            ViewBag.CurrentPageSize = pageSize.Value;
            var data = courses.OrderByDescending(x => x.EndTime).ToPagedList(page.Value, pageSize.Value);
            return View(data);
        }

        public CheckRoleResult CheckCourseRole(long? id)
        {
            var result = new CheckRoleResult();
            if (id == null)
            {
                result.ActionResult = RedirectErrorPage(Url.Action("Index", "Home"));
                result.IsValid = false;
            }
            else
            {
                var userid = User.Identity.GetUserId();
                var account = DbContext.Accounts.FirstOrDefault(s => s.Id == userid);
                // ReSharper disable once PossibleNullReferenceException
                var course = account.Courses.FirstOrDefault(s => s.CourseId == id && s.Actived);
                if (course == null)
                {
                    result.ActionResult = RedirectErrorPage(Url.Action("Index", "Home"));
                    result.IsValid = false;
                }
                else
                {
                    result.Course = course;
                    result.Account = account;
                    result.IsValid = true;
                }
            }
            return result;
        }

        public ActionResult Tests(string keyword, int? page, int? pageSize, int? status, long? courseId)
        {
            var result = CheckCourseRole(courseId);
            if (!result.IsValid) return result.ActionResult;
            // ReSharper disable once PossibleNullReferenceException
            var tests = result.Course.Tests.Where(s => s.Actived);
            tests = GetSubmitNo(tests.ToList(), result).AsQueryable();
            tests = tests.OrderByDescending(x => x.TestId);
            if (!page.HasValue || page.Value < 1) page = 1;
            if (!pageSize.HasValue || pageSize < 10) pageSize = 10;
            ViewBag.Keyword = keyword;
            ViewBag.PageSize = new SelectList(new[] { 10, 25, 50, 100 }, pageSize);
            ViewBag.CurrentPageSize = pageSize.Value;
            ViewBag.CourseId = courseId;
            var data = tests.ToPagedList(page.Value, pageSize.Value);
            return View(data);
        }


        // Hiển thị thông tin bài test hoặc tiếp tục bài test
        public ActionResult Test(long? id)
        {
            if (!id.HasValue)
            {
                return RedirectErrorPage(Url.Action("Index", "Home"));
            }

            var userid = User.Identity.GetUserId();
            var account = DbContext.Accounts.First(s => s.Id == userid);

            var test = DbContext.Tests.FirstOrDefault(s => s.TestId == id.Value && s.Actived);
            if (test != null)
            {
                if (account.Courses.First(s => s.CourseId == test.CourseId).Tests.All(s => s.TestId != test.TestId) || !(test.TestStart <= DateTime.Now && DateTime.Now <= test.TestEnd))
                {
                    return RedirectAccessDeniedPage(Url.Action("Index", "Home"));
                }
                var answerSheets = account.AnswerSheets.Where(s => s.TestId == test.TestId).OrderByDescending(s => s.TestId).ToList();

                if (answerSheets.Count == 0)
                {
                    return View(test);
                }
                if (answerSheets.Count <= test.SubmitNo)
                {
                    var lastAnswerSheet = answerSheets.Last();
                    var now = lastAnswerSheet.SubmitTime.AddMinutes(test.TestTime);
                    var date = DateTime.Now.AddMinutes(2);
                    var minute = DateTime.Now.Subtract(lastAnswerSheet.SubmitTime).TotalMinutes;

                    if (lastAnswerSheet.IsDone || minute >= test.TestTime)
                    {
                        if (answerSheets.Count == test.SubmitNo)
                            return RedirectAccessDeniedPage(Url.Action("Index", "Home"));
                        test.SubmitNoUser = answerSheets.Count;
                        return View(test);
                    }
                    ViewBag.SubmitTime = test.TestTime / 10;
                    test.TestTime = test.TestTime - (int)minute;
                    return View("ContinueTest", lastAnswerSheet);
                }
            }
            return RedirectErrorPage(Url.Action("Index", "Home"));
        }

        [HttpPost]
        public ActionResult GetTestPreview(long? id)
        {
            if (!id.HasValue)
            {
                return RedirectErrorPage(Url.Action("Index", "Home"));
            }

            var userid = User.Identity.GetUserId();
            var account = DbContext.Accounts.First(s => s.Id == userid);

            var test = DbContext.Tests.FirstOrDefault(s => s.TestId == id.Value && s.Actived);
            if (test != null)
            {
                if (account.Courses.First(s => s.CourseId == test.CourseId).Tests.All(s => s.TestId != test.TestId) || !(test.TestStart <= DateTime.Now && DateTime.Now <= test.TestEnd))
                {
                    return RedirectAccessDeniedPage(Url.Action("Index", "Home"));
                }
                var answerSheets = account.AnswerSheets.Where(s => s.TestId == test.TestId).OrderByDescending(s => s.TestId).ToList();

                if (answerSheets.Count == 0)
                {
                    return PartialView("_TestPreview", test);
                }
                if (answerSheets.Count <= test.SubmitNo)
                {
                    var lastAnswerSheet = answerSheets.Last();
                    var now = lastAnswerSheet.SubmitTime.AddMinutes(test.TestTime);
                    var date = DateTime.Now.AddMinutes(2);
                    var minute = DateTime.Now.Subtract(lastAnswerSheet.SubmitTime).TotalMinutes;

                    if (lastAnswerSheet.IsDone || minute >= test.TestTime)
                    {
                        if (answerSheets.Count == test.SubmitNo)
                            return RedirectAccessDeniedPage(Url.Action("Index", "Home"));
                        return PartialView("_TestPreview", test);
                    }
                }
            }
            return RedirectErrorPage(Url.Action("Index", "Home"));
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GradeTest(string data)
        {
            var userid = User.Identity.GetUserId();
            var answerSheet = new AnswerSheet(data) { TestTakerId = userid };
            var score = 0f;
            var test = DbContext.Tests.FirstOrDefault(s => s.TestId == answerSheet.TestId && s.Actived);
            var account = DbContext.Accounts.First(s => s.Id == userid);

            if (test != null)
            {
                if (account.Courses.First(s => s.CourseId == test.CourseId && s.Actived).Tests.All(s => s.TestId != test.TestId) || !(test.TestStart <= DateTime.Now && DateTime.Now <= test.TestEnd))
                {
                    return RedirectAccessDeniedPage(Url.Action("Index", "Home"));
                }
                var answerSheets = account.AnswerSheets.Where(s => s.TestId == test.TestId).OrderByDescending(s => s.TestId).ToList();
                foreach (var answer in answerSheet.Answers)
                {
                    var testDetail = test.TestDetails.FirstOrDefault(s => s.QuestionId == answer.QuestionId);
                    if (testDetail != null) score += testDetail.GradeTest(answer);
                }
                answerSheet.Score = score;
                if (answerSheets.Count == 0)
                {
                    answerSheet.IsDone = true;
                    DbContext.AnswerSheets.Add(answerSheet);
                    DbContext.SaveChanges();
                }
                else
                {
                    if (answerSheets.Count <= test.SubmitNo)
                    {
                        var lastAnswerSheet = answerSheets.Last();
                        var minute = DateTime.Now.AddMinutes(2).Subtract(lastAnswerSheet.SubmitTime).TotalMinutes;

                        if (lastAnswerSheet.IsDone || minute >= test.TestTime)
                        {
                            if (answerSheets.Count == test.SubmitNo)
                                return RedirectAccessDeniedPage(Url.Action("Index", "Home"));
                            answerSheet.IsDone = true;
                            DbContext.AnswerSheets.Add(answerSheet);
                            DbContext.SaveChanges();
                        }
                        else
                        {
                            answerSheet.SubmitTime = lastAnswerSheet.SubmitTime;
                            answerSheet.IsDone = true;
                            lastAnswerSheet.Update(answerSheet);
                            DbContext.Entry(lastAnswerSheet).State = EntityState.Modified;
                            DbContext.SaveChanges();
                        }
                    }
                }
            }
            return PartialView("_TestDone", answerSheet);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AutoSubmit(string data)
        {
            var userid = User.Identity.GetUserId();
            var answerSheet = new AnswerSheet(data) { TestTakerId = userid };
            var score = 0f;
            var test = DbContext.Tests.FirstOrDefault(s => s.TestId == answerSheet.TestId && s.Actived);
            var account = DbContext.Accounts.First(s => s.Id == userid);

            if (test != null)
            {
                if (account.Courses.First(s => s.CourseId == test.CourseId).Tests.All(s => s.TestId != test.TestId) || !(test.TestStart <= DateTime.Now && DateTime.Now <= test.TestEnd))
                {
                    return Json("");
                }
                var answerSheets = account.AnswerSheets.Where(s => s.TestId == test.TestId).OrderByDescending(s => s.TestId).ToList();
                foreach (var answer in answerSheet.Answers)
                {
                    var testDetail = test.TestDetails.FirstOrDefault(s => s.QuestionId == answer.QuestionId);
                    if (testDetail != null) score += testDetail.GradeTest(answer);
                }
                answerSheet.Score = score;
                if (answerSheets.Count == 0)
                {
                    DbContext.AnswerSheets.Add(answerSheet);
                    DbContext.SaveChanges();
                }
                else
                {
                    if (answerSheets.Count <= test.SubmitNo)
                    {
                        var lastAnswerSheet = answerSheets.Last();
                        var minute = DateTime.Now.Subtract(lastAnswerSheet.SubmitTime).TotalMinutes;

                        if (lastAnswerSheet.IsDone || minute >= test.TestTime)
                        {
                            if (answerSheets.Count == test.SubmitNo)
                                return RedirectAccessDeniedPage(Url.Action("Index", "Home"));
                            DbContext.AnswerSheets.Add(answerSheet);
                            DbContext.SaveChanges();
                        }
                        answerSheet.SubmitTime = lastAnswerSheet.SubmitTime;
                        lastAnswerSheet.Update(answerSheet);
                        DbContext.Entry(lastAnswerSheet).State = EntityState.Modified;
                        DbContext.SaveChanges();
                    }
                }
            }
            return Json("");
        }


        private List<Test> GetSubmitNo(List<Test> tests, CheckRoleResult result)
        {
            var answerSheets = result.Account.AnswerSheets.ToList();
            foreach (var answersheet in answerSheets)
            {
                var test = tests.Find(s => s.TestId == answersheet.TestId);

                if (test != null)
                {
                    test.SubmitNoUser++;
                    test.Scores.Add(answersheet.Score);
                }
            }
            return tests;
        }
    }
}