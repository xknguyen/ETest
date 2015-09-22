using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ETest.Models
{
    public class AnswerSheet
    {
        public long AnswerSheetId { get; set; }
        public long TestId { get; set; }
        public long TestTakerId { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual Test Test { get; set; }
        public virtual List<Question> Answers { get; set; }
        public virtual Account TestTaker { get; set; }
    }
}