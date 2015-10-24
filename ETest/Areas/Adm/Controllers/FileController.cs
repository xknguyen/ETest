using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ETest.Areas.Adm.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace ETest.Areas.Adm.Controllers
{
    public class FileController : AdminController
    {
        // GET: Adm/File
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult GetDirectory(string type)
        {
            var userName = User.Identity.GetUserName();
            //Lấy thư mục của tài khoản



            return Json(JsonConvert.SerializeObject("",
                Formatting.Indented,
                new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore }));
        }

    }
}