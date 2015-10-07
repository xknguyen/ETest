using System;
using System.Linq;
using System.Web.Mvc;
using ETest.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Data.Entity;

namespace ETest.Areas.Adm.Controllers
{
    public class QuestionController : AdminController
    {
        // GET: Adm/Question
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            var question = new Question {Actived = true};
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

                if (group.TeacherId != User.Identity.GetUserId())
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
            if (question.Group.TeacherId != User.Identity.GetUserId())
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
                        Message = "Thêm thành công.",
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
            var groups = DbContext.Groups.Where(s => s.TeacherId == userId).ToList();
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
    }
}