using System;
using System.Linq;
using System.Web.Mvc;
using ETest.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Data.Entity;
using ETest.Areas.Adm.Models;
using PagedList;

namespace ETest.Areas.Adm.Controllers
{
    public class QuestionController : AdminController
    {
        public CheckRoleResult CheckGroupRole(long? id)
        {
            var result = new CheckRoleResult();
            if (id == null)
            {
                result.ActionResult = RedirectErrorPage(Url.Action("Index", "Dashboard"));
                result.IsValid = false;
            }
            else
            {
                var group = DbContext.Groups.Find(id);
                if (group == null)
                {
                    result.ActionResult = RedirectErrorPage(Url.Action("Index", "Dashboard"));
                    result.IsValid = false;
                }
                else
                {
                    if (group.Course.TeacherId != User.Identity.GetUserId())
                    {
                        result.ActionResult = RedirectAccessDeniedPage(Url.Action("Index", "Dashboard"));
                        result.IsValid = false;
                    }
                    else
                    {
                        result.Group = group;
                        result.IsValid = true;
                    }
                }
            }
            return result;
        }

        // GET: Adm/Question
        public ActionResult Index(string keyword, int? page, int? pageSize, int? status, int? groupId)
        {
            var questions = DbContext.Questions.AsQueryable();

            // Tìm nhà cung cấp theo từ khóa (keyword) bằng cách kiểm
            // tra nó có xuất hiện trong tên, mô tả hay địa chỉ của NCC
            if (!string.IsNullOrEmpty(keyword))
                questions = questions.Where(x => x.QuestionTitle.Contains(keyword)||x.Group.GroupName.Contains(keyword));

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
                var data1 = questions.OrderByDescending(x => x.QuestionId)
                    .ToPagedList(page.Value, pageSize.Value);
                return View(data1);
            }
            var isActived = status.Value == 1;
            var data = questions.OrderByDescending(x => x.QuestionId).Where(x => x.Actived == isActived)
                                .ToPagedList(page.Value, pageSize.Value);
            return View(data);
        }

        public ActionResult Create()
        {
            var question = new Question {Actived = true, QuestionDetails = new List<QuestionDetail>()};
            InitFormData(question);
            return View(question);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(string data)
        {
            try
            {
                var questiontemp = new Question(data);
                // kiểm tra hợp lệ dữ liệu

                var group = DbContext.Groups.FirstOrDefault(s => s.GroupId == questiontemp.GroupId);
                if (group == null)
                {
                    return Json(new
                    {
                        Message = "Nhóm bạn chọn đã bị xóa. Vui lòng chọn nhóm khác!",
                        Success = false
                    });
                }

                if (group.Course.TeacherId != User.Identity.GetUserId())
                {
                    return Json(new
                    {
                        Message = "Bạn không có quyền sử dụng nhóm này. Vui lòng chọn nhóm khác!",
                        Success = false
                    });
                }
                var question = new Question
                {
                    Actived = questiontemp.Actived,
                    GroupId = questiontemp.GroupId,
                    QuestionTitle = questiontemp.QuestionTitle,
                    QuestionDetails = questiontemp.QuestionDetails
                };

                DbContext.Questions.Add(question);
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
            var question = DbContext.Questions.FirstOrDefault(s => s.QuestionId == id.Value);
            if (question == null)
            {
                return RedirectErrorPage(Url.Action("Index"));
            }
            if (question.Group.Course.TeacherId != User.Identity.GetUserId())
            {
                return RedirectAccessDeniedPage(Url.Action("Index"));
            }
            InitFormData(question);
            return View(question);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(string data)
        {
            try
            {
                var questiontemp = new Question(data);
                var questionDb = DbContext.Questions.FirstOrDefault(s => s.QuestionId == questiontemp.QuestionId);



                if (questionDb != null)
                {
                    if (questionDb.Group.Course.TeacherId != User.Identity.GetUserId())
                    {
                        return Json(new
                        {
                            Message = "Bạn không có quyền sử dụng nhóm này. Vui lòng chọn nhóm khác!",
                            Success = false
                        });
                    }
                    questionDb.Actived = questiontemp.Actived;
                    questionDb.GroupId = questiontemp.GroupId;
                    questionDb.QuestionTitle = questiontemp.QuestionTitle;
                    UpdateDetail(questionDb.QuestionDetails);
                    questionDb.QuestionDetails = questiontemp.QuestionDetails;
                }
                DbContext.Entry(questionDb).State = EntityState.Modified;
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
            catch (Exception)
            {
                //
            }

            return Json(new
            {
                Message = "Đã có lỗi xảy ra! Vui lòng thử lại sau.",
                Success = false
            });
        }

        private void InitFormData(Question question)
        {
            var userId = User.Identity.GetUserId();
            var groups = DbContext.Groups.Where(s => s.Course.TeacherId == userId).ToList();
            ViewBag.GroupId = question.GroupId > 0
                ? new SelectList(groups, "GroupId", "GroupName", question.GroupId)
                : new SelectList(groups, "GroupId", "GroupName", null);
        }

        private void UpdateDetail(IList<QuestionDetail> oldList)
        {
            DbContext.QuestionDetails.RemoveRange(oldList.ToList());
            //foreach (var detai in oldList.ToList())
            //{
                
            //    // Nếu như old có trong new list thì cập nhật
            //    //// Ngược lại thì remove
            //    //if (newList.FirstOrDefault(s => s.QuestionDetailId == detai.QuestionDetailId) == null)
            //    //{
            //    //    DbContext.QuestionDetails.Remove(detai);
            //    //}
            //    //else
            //    //{
            //    //    detai.Update(detai);
            //    //}
            //    // trả về danh sách chưa có trong database
            //    //return newList.Where(s => s.QuestionId == 0).ToList();
            //}
        }


        protected override bool OnUpdateToggle(string propName, bool value, object[] keys)
        {
            var query = string.Format("UPDATE dbo.Questions SET {0} = @p0 WHERE QuestionId = @p1", propName);
            return DbContext.Database.ExecuteSqlCommand(query, value, keys[0]) > 0;
        }

        [HttpPost]
        public ActionResult Preview(long? id)
        {
            var question = DbContext.Questions.FirstOrDefault(s => s.QuestionId == id);
            return question != null ? PartialView("_QuestionView", question) : null;
        }


        public ActionResult TestView()
        {
            var question = DbContext.Questions.FirstOrDefault();
            return View(question);
        }
    }
}