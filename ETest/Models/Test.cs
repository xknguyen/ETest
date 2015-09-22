using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ETest.Models
{
    public class Test
    {
        public long TestId { get; set; }
        public string TestTitle { get; set; }
        public string TestDescription { get; set; }
        public DateTime TestStart { get; set; }
        public int TestTime { get; set; }
        public long CourseId { get; set; }
        public string TeacherId { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public Account Teacher { get; set; }
        public virtual Course Course { get; set; }
        public virtual List<QuestionTest> QuestionTests { set; get; }
        public virtual List<AnswerSheet> AnswerSheets { get; set; }
         
    }
}