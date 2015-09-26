using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ETest.Models
{
    public class QuestionDetail
    {
        public long QuestionDetailId { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [StringLength(200, ErrorMessage = "{0} không vượt quá {2} kí tự.")]
        [Display(Name = "Tiêu đề")]
        public string QusetionTitle { get; set; }

        [Display(Name = "Lựa chọn")]
        public string Choice { get; set; }

        [Display(Name = "Đáp án")]
        public string Answer { get; set; }

        public int OrderNo { get; set; }

        public long QuestionId { get; set; }

        public virtual Question Question { get; set; }
    }
}