using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETest.Models
{
    public class Test
    {
        public int TestId { get; set; }

        public string TestTitle { get; set; }

        public string TestDescription { get; set; }

        public DateTime TestStart { get; set; }

        public int TestTime { get; set; }

        public int CourseId { get; set; }

        public virtual Course Course { get; set; }

        public virtual List<Question> Questions { set; get; }
    }
}