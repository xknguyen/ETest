using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETest.Models
{
    public class AnswerSheet
    {
        public int AnswerSheetId { get; set; }
        public int TestId { get; set; }
        public int TestTakerId { get; set; }

        public virtual Test Test { get; set; }
        public virtual List<Question> Answers { get; set; }
        public virtual Account TestTaker { get; set; }
    }
}