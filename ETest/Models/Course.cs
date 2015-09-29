using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Core.Extensions;

namespace ETest.Models
{
    public class Course
    {
        public long CourseId { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [StringLength(200, ErrorMessage = "{0} không vượt quá {2} kí tự.")]
        [Display(Name = "Tên khóa học")]
        public string CourseName { get; set; }

        [StringLength(1000, ErrorMessage = "{0} không vượt quá {2} kí tự.")]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        public string GetDescription
        {
            get { return Description.GetHtmlContent(); }
        }

        [Required(ErrorMessage = "{0} không được để trống")]
        [DataType(DataType.Date)]
        [DisplayName("Ngày bắt đầu")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartTime { get; set; }


        [DataType(DataType.Date)]
        [DisplayName("Ngày kết thúc")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndTime { get; set; }

        public string TeacherId { get; set; }

        [Display(Name = "Trạng thái")]
        public bool Actived { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
        
        public virtual Account Teacher { get; set; }
        public virtual IList<Account> Students { get; set; }
        public virtual IList<Test> Tests { get; set; }
    }
}