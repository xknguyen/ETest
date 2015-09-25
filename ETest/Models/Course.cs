using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        // public string TeacherId { get; set; }

        [Display(Name = "Trạng thái")]
        public bool Active { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // public virtual Account Teacher { get; set; }
        public virtual IList<Account> Students { get; set; }
        public virtual IList<Test> Tests { get; set; }
    }
}