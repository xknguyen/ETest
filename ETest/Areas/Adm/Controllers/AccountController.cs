using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ETest.Areas.Adm.Models;
using ETest.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
                new {text = "Đang khóa", value = 2}
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

        protected override bool OnUpdateToggle(string propName, bool value, object[] keys)
        {
            var query = string.Format("UPDATE dbo.UserProfiles SET {0} = @p0 WHERE AccountId = @p1", propName);
            return DbContext.Database.ExecuteSqlCommand(query, value, keys[0]) > 0;
        }

        public ActionResult Edit(string username)
        {
            if (String.IsNullOrEmpty(username))
            {
                return RedirectErrorPage(Url.Action("Index"));
            }
            var userManager = new UserManager<Account>(new UserStore<Account>(DbContext));
            Account account = userManager.FindByName(username);

            if (account == null)
            {
                return RedirectErrorPage(Url.Action("Index"));
            }
            EditAccountModel accountModel = new EditAccountModel()
            {
                UserName = account.UserName,
                PhoneNumber = account.PhoneNumber,
                Email = account.UserName,
                Identity = account.Profile.Identity,
                LastName = account.Profile.LastName,
                FirstName = account.Profile.FirstName,
                Notes = account.Profile.Notes,
                BirthDate = account.Profile.BirthDate,
                Actived = account.Profile.Actived
            };
            InitFormData(accountModel);
            return View(accountModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditAccountModel editModel)
        {
            var userManager = new UserManager<Account>(new UserStore<Account>(DbContext));
            Account editAccount = userManager.FindByName(editModel.UserName);

            if (editAccount == null)
            {
                return RedirectErrorPage(Url.Action("Index"));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var acc = userManager.FindById(editModel.Id);
                    acc.Email = editModel.Email;
                    acc.PhoneNumber = editModel.PhoneNumber;
                    acc.Profile.BirthDate = editModel.BirthDate;
                    acc.Profile.FirstName = editModel.FirstName;
                    acc.Profile.LastName = editModel.LastName;
                    var result = userManager.Update(acc);
                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(editModel.Password))
                        {
                            userManager.RemovePassword(editModel.Id);
                            userManager.AddPassword(editModel.Id, editModel.Password);
                        }
                        if (editModel.Role == "Admin")
                        {
                            userManager.AddToRole(acc.Id, "Admin");
                            userManager.AddToRole(acc.Id, "Teacher");
                        }
                        else if (editModel.Role == "Teacher")
                        {
                            userManager.AddToRole(editModel.Id, "Teacher");
                        }
                        else
                        {
                            userManager.AddToRole(editModel.Id, "Student");
                        }

                        return Redirect(null);
                    }
                    ModelState.AddModelError("", "Đã có lỗi xảy ra. Vui lòng thử lại sau.");
                }
                catch
                {
                    ModelState.AddModelError("", "Đã có lỗi xảy ra. Vui lòng thử lại sau.");
                }
            }
            InitFormData(editModel);
            return View(editModel);
        }
        public ActionResult Create()
        {
            var account = new CreateAccountModel() { Actived = true };
            InitFormData(account);
            return View(account);
        }

        [HttpPost]
        public ActionResult Create(CreateAccountModel account)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Account newAccount = new Account()
                    {
                        UserName = account.UserName,
                        PhoneNumber = account.PhoneNumber,
                        Email = account.UserName,
                        Profile = new UserProfile()
                        {
                            Identity = account.Identity,
                            LastName = account.LastName,
                            FirstName = account.FirstName,
                            Notes = account.Notes,
                            BirthDate = account.BirthDate,
                            Actived = account.Actived
                        }
                    };
                    var userManager = new UserManager<Account>(new UserStore<Account>(DbContext));
                    var result = userManager.Create(newAccount, account.Password);
                    if (result.Succeeded)
                    {
                        if (account.Role == "Admin")
                        {
                            userManager.AddToRole(newAccount.Id, "Admin");
                            userManager.AddToRole(newAccount.Id, "Teacher");
                        } else if (account.Role == "Teacher")
                        {
                            userManager.AddToRole(newAccount.Id, "Teacher");
                        }
                        else
                        {
                            userManager.AddToRole(newAccount.Id, "Student");
                        }

                        return Redirect(null);
                    }
                    ModelState.AddModelError("", "Đã có lỗi xảy ra. Vui lòng thử lại sau.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            InitFormData(account);
            return View(account);
        }

        private void InitFormData(AccountViewModel account)
        {
            var role = new[]
            {
                new { Id = "Admin", Name = "Quản trị viên"},
                new { Id = "Teacher", Name = "Giáo viên"},
                new { Id = "Student", Name = "Sinh viên"}
            };
            account.AccountRoles = new SelectList(role,"Id","Name",account.Role);
        }
    }
}