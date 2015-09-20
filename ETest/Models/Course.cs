using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETest.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public string Description { get; set; }
        public string TeacherId { get; set; }

        public virtual Account Teacher { get; set; }
        public virtual List<Account> Students { get; set; }
        public virtual List<Test> Tests { get; set; }
    }
}