using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ETest.Models;
using Microsoft.AspNet.Identity;

namespace ETest.Areas.Adm.Controllers
{
    public class GroupController : AdminController
    {
        // GET: Adm/Group
        public ActionResult Index()
        {
            var groups = PopulateGroups();
            return View(groups);
        }

        private List<Group> PopulateGroups()
        {
            var userId = User.Identity.GetUserId();
            var allCates = DbContext.Groups
                .Where(s=>s.TeacherId == userId)
                .OrderBy(x => x.ParentGroupId)
                .ThenBy(x => x.OrderNo)
                .ToList();
            var groupCates = allCates.Where(x => !x.ParentGroupId.HasValue || x.ParentGroupId == 0)
                .ToList();
            foreach (var group in groupCates)
            {
                AddSubgroup(group, allCates);
            }
            return groupCates;
        }

        private void AddSubgroup(Group group, List<Group> allCates)
        {
            group.ChildGroups = allCates.Where(x => x.ParentGroupId == group.GroupId).ToList();
            foreach (var subCate in group.ChildGroups)
            {
                AddSubgroup(subCate, allCates);
            }
        }


        [HttpPost]
        public JsonResult Reorder(int cid, int pid, int[] siblings)
        {
            bool success;
            try
            {
                StringBuilder query = new StringBuilder();
                //Tạo các chuỗi truy vấn thuộc tính OrderNo
                for (int i = 0; i < siblings.Length; i++)
                {
                    query.AppendFormat("UPDATE dbo.Groups SET OrderNo = {0} " + "WHERE GroupId = {1};", i + 1, siblings[i]);
                    query.AppendLine();
                }
                if (pid > 0)
                    query.AppendFormat("UPDATE dbo.Groups SET ParentGroupId = {0} " + "WHERE GroupId = {1}", pid, cid);
                else
                    query.AppendFormat("UPDATE dbo.Groups SET ParentGroupId = NULL " + "WHERE GroupId = {0}", cid);
                success = DbContext.Database.ExecuteSqlCommand(query.ToString()) > 0;
            }
            catch (Exception)
            {
                success = false;
            }
            return Json(success);
        }


        /// <summary>
        /// Get: Adm/group/Create
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var emptyGroup = new Group() {Actived = true};
            InitFormData(emptyGroup);
            return View(emptyGroup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "GroupName, Description, ParentGroupId, OrderNo, Actived")] Group group)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    group.TeacherId = User.Identity.GetUserId();
                    DbContext.Groups.Add(group);
                    if (DbContext.SaveChanges() > 0)
                        return Redirect(null);
                    ModelState.AddModelError("", "Đã có lỗi xảy ra. Vui lòng thử lại sau.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            InitFormData(group);
            return View(group);
        }

        private void InitFormData(Group group)
        {
            //Lấy tất cả các nhóm câu hỏi và gom nhóm chúng
            var groupedGroups = PopulateGroups();
            var disableIds = new List<long>();
            var groups = new List<Group>();
            ConvertToTreeStructure(groupedGroups, disableIds, 0, groups);
            //Tạo danh sách chọn làm dữ liệu nguồn cho DropDownList
            ViewBag.ParentGroupId = group.ParentGroupId > 0 ? new SelectList(groups, "GroupId", "GroupName", group.ParentGroupId, disableIds) : new SelectList(groups, "GroupId", "GroupName", (object)null, disableIds);
        }

        private void ConvertToTreeStructure(IEnumerable<Group> source, List<long> disableIds, int level,
            List<Group> result)
        {
            foreach (var group in source)
            {
                var groupName = (level > 0) ? " ".PadLeft(level + 1, '-') : "";
                groupName += group.GroupName;

                result.Add(new Group
                {
                    GroupId = group.GroupId,
                    GroupName = groupName
                });

                if (level > 1) disableIds.Add(group.GroupId);
                ConvertToTreeStructure(group.ChildGroups, disableIds, level + 1, result);
            }

        }

        private void CheckValidated(Group group)
        {
            
            // Kiểm tra Parent không phải là chính nó
            if (group.ParentGroupId == group.GroupId)
                ModelState.AddModelError("ParentGroupId", "Bạn không thể chọn nhóm câu hỏi làm cha của chính nó");

            // kiểm tra cha của nó không phải là con của nó
            CheckAllChildCategories(group);
        }

        private void CheckAllChildCategories(Group group)
        {
            var list = DbContext.Groups.Where(s => s.ParentGroupId == group.GroupId).ToList();
            foreach (var item in list)
            {
                if (item.GroupId == group.ParentGroupId)
                {
                    ModelState.AddModelError("ParentGroupId", "Bạn không thể chọn danh mục con làm cha của nó");
                    return;
                }
                CheckAllChildCategories(item);
            }
        }

        protected override bool OnUpdateToggle(string propName, bool value, object[] keys)
        {
            string query = string.Format("UPDATE dbo.Groups SET {0} = @p0 WHERE GroupId = @p1", propName);
            return DbContext.Database.ExecuteSqlCommand(query, value, keys[0]) > 0;
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectErrorPage(Url.Action("Index"));
            }
            Group group = DbContext.Groups.Find(id);
            if (group == null)
            {
                return RedirectErrorPage(Url.Action("Index"));
            }
            if (group.TeacherId != User.Identity.GetUserId())
            {
                return RedirectAccessDeniedPage(Url.Action("Index"));
            }
            InitFormData(group);
            return View(group);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "GroupId, GroupName, Description, ParentGroupId, OrderNo, Actived, RowVersion")] Group group)
        {
            var groupOld = DbContext.Groups.Find(group.GroupId);
            if (groupOld == null)
            {
                return RedirectErrorPage(Url.Action("Index"));
            }
            if (groupOld.TeacherId != User.Identity.GetUserId())
            {
                return RedirectAccessDeniedPage(Url.Action("Index"));
            }
             
            CheckValidated(group);
            
            
            
            if (ModelState.IsValid)
            {
                try
                {
                    TryUpdateModel(groupOld,
                        new[]
                        {"GroupId", "GroupName", "Description", "ParentGroupId", "OrderNo", "Actived", "RowVersion"});
                    DbContext.Entry(groupOld).OriginalValues["RowVersion"] = group.RowVersion;
                    //groupOld.GroupName = group.GroupName;
                    //groupOld.Description = group.Description;
                    //groupOld.ParentGroupId = group.ParentGroupId;
                    //groupOld.OrderNo = group.OrderNo;
                    //groupOld.Actived = group.Actived;
                    DbContext.Entry(groupOld).State = EntityState.Modified;
                    if (DbContext.SaveChanges() > 0)
                    {
                        return Redirect(group.GroupId);
                    }
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var clientValues = (Group)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty, "Không thể lưu thay đổi, nhóm đã bị xóa bởi người dùng khác");
                    }
                    else
                    {
                        var databaseValues = (Group)databaseEntry.ToObject();

                        if (databaseValues.GroupName != clientValues.GroupName)
                            ModelState.AddModelError("GroupName", "Giá trị hiện tại: " + databaseValues.GroupName);
                        if (databaseValues.Description != clientValues.Description)
                            ViewBag.CurrentDescription = "Giá trị hiện tại: " + databaseValues.Description;
                        if (databaseValues.ParentGroupId != clientValues.ParentGroupId)
                            ModelState.AddModelError("ParentGroupId", "Giá trị hiện tại: " + databaseValues.ParentGroupId);
                        if (databaseValues.OrderNo != clientValues.OrderNo)
                            ModelState.AddModelError("OrderNo", "Giá trị hiện tại: " + databaseValues.OrderNo);
                        if (databaseValues.Actived != clientValues.Actived)
                            ModelState.AddModelError("Actived", "Giá trị hiện tại: " + databaseValues.Actived);
                        ModelState.AddModelError(string.Empty, "Nội dung bạn đang cập nhật đã được cập nhật bởi một người khác. "
                           + "Những giá trị hiện tại trong CSDL sẽ được hiển thị trên màn hình. "
                           + "Nếu bạn vẫn muốn cập nhật nó thì nhấn lại nút Lưu. ");
                        group.RowVersion = databaseValues.RowVersion;
                    }
                }
            }
            InitFormData(group);
            return View(group);
        }

    }
}