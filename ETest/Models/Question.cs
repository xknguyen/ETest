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

        [Required(ErrorMessage = "{0} không được để trống")]
        [StringLength(200, ErrorMessage = "{0} không vượt quá {2} kí tự.")]
        [Display(Name = "Tiêu đề")]
        public string QusetionTitle { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Nhóm câu hỏi")]
        public long GroupId { get; set; }

        [Display(Name = "Trạng thái")]
        public bool Active { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual IList<Test> Tests { get; set; }
        public virtual IList<TestDetail> TestDetails { get; set; }
        public virtual Group Group { get; set; }
        public virtual QuestionType QuestionType { get; set; }
        public virtual IList<Answer> Answers { get; set; }
        public virtual IList<QuestionDetail> QuestionDetails { get; set; }
    }
}