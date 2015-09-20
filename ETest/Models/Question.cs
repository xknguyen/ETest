using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
        public int QuestionId { get; set; }

        public string QusetionTitle { get; set; }

        public string Choice { get; set; }

        public string Answer { get; set; }

        public int GroupId { get; set; }

        public QuestionType Type { get; set; }
    }
}