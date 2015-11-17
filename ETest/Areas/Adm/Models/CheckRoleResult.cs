using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ETest.Models;

namespace ETest.Areas.Adm.Models
{
    public class CheckRoleResult
    {
        public Group Group { get; set; }
        public Course Course { get; set; }
        public Account Account { get; set; }
        public ActionResult ActionResult { get; set; }
        public bool IsValid { get; set; }

        public CheckRoleResult()
        {
            
        }

        public CheckRoleResult(Course course, ActionResult actionResult, bool isValid)
        {
            Course = course;
            ActionResult = actionResult;
            IsValid = isValid;
        }
    }
}