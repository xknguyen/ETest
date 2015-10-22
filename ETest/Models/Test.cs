using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ETest.Models
{
    public enum TestType
    {
        [Description("Bài kiểm tra nộp tập tin")]
        Upload,
        [Description("Bài kiểm tra trắc nghiệm")]
        Test
    }

    public class Test
    {
        public long TestId { get; set; }
        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Tiêu đề")]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string TestTitle { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Mô tả")]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Tiêu đề")]
        public DateTime TestStart { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Thời gian")]
        public int TestTime { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Tiêu đề")]
        public long CourseId { get; set; }

        public string TeacherId { get; set; }

        [Display(Name = "Trạng thái")]
        public bool Actived { get; set; }


        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Loại bài kiểm tra")]
        public TestType TestType { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public Account Teacher { get; set; }
        public virtual Course Course { get; set; }
        public virtual IList<TestDetail> TestDetails { set; get; }
        public virtual IList<AnswerSheet> AnswerSheets { get; set; }
         
    }
}