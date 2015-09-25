using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace ETest.Models
{
    public class Class
    {
        public long ClassId { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [StringLength(100, ErrorMessage = "{0} không vượt quá {2} kí tự.")]
        [Display(Name = "Tên lớp")]
        public string ClassName { get; set; }

        [StringLength(500, ErrorMessage = "{0} không vượt quá {2} kí tự.")]
        [AllowHtml]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Trạng thái")]
        public bool Active { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual IList<Account> Students { get; set; }
    }
}