using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ETest.Models
{
    public class Test
    {
        public long TestId { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [StringLength(200, ErrorMessage = "{0} không vượt quá {1} kí tự.")]
        [Display(Name = "Tiêu đề")]
        public string TestTitle { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [StringLength(200, ErrorMessage = "{0} không vượt quá {1} kí tự.")]
        [Display(Name = "Tiêu đề")]
        public string TestDescription { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Tiêu đề")]
        public DateTime TestStart { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Thời gian")]
        public int TestTime { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Tiêu đề")]
        public long? CourseId { get; set; }

        public string TeacherId { get; set; }

        [Display(Name = "Trạng thái")]
        public bool Actived { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public Account Teacher { get; set; }
        public virtual Course Course { get; set; }
        public virtual IList<TestDetail> TestDetails { set; get; }
        public virtual IList<AnswerSheet> AnswerSheets { get; set; }
         
    }
}