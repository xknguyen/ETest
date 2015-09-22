using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ETest.Models
{
    public enum QuestionType
    {
        Choice,
        Order,
        Associate,
        Gap,
        Inline,
        Upload,
        Slider
    }

    public class Question
    {
        public long QuestionId { get; set; }

        public string QusetionTitle { get; set; }

        public string Choice { get; set; }

        public string Answer { get; set; }

        public long GroupId { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual List<Test> Tests { get; set; }
        public virtual Group Group { get; set; }
        public virtual QuestionType QuestionType { get; set; }
    }
}