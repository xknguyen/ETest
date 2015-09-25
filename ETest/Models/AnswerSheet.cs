using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ETest.Models
{
    public class AnswerSheet
    {
        public long AnswerSheetId { get; set; }
        public long TestId { get; set; }
        public string TestTakerId { get; set; }
        public DateTime SubmitTime { get; set; }

        public virtual Test Test { get; set; }
        public virtual IList<Answer> Answers { get; set; }
        public virtual Account TestTaker { get; set; }
    }
}