using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ETest.Models
{
    public class UserProfile
    {
        [Key]
        [ForeignKey("Account")]
        [StringLength(128)]
        [DisplayName("Mã tài khoản")]
        public string AccountId { get; set; }       // Mã tài khoản

        [Required(ErrorMessage = "{0} không được để trống")]
        [StringLength(10, ErrorMessage = "{0} không vượt quá {2} kí tự.")]
        [Display(Name = "MSSV")]
        public string Identity { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [DisplayName("Họ và tên lót")]
        [StringLength(100, ErrorMessage = "{0} không vượt quá {2} kí tự.")]
        public string LastName { get; set; }       // Họ và tên lót

        [Required(ErrorMessage = "{0} không được để trống")]
        [StringLength(100, ErrorMessage = "{0} không vượt quá {2} kí tự.")]
        [DisplayName("Tên")]
        public string FirstName { get; set; }        // Tên

        [StringLength(1000, ErrorMessage = "{0} không vượt quá {2} kí tự.")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Ghi chú")]
        public string Notes { get; set; }       // Ghi chú

        [DisplayName("Lớp")]
        public long? ClassId { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Ngày sinh")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? BirthDate { get; set; }

        [DisplayName("Họ tên")]
        public string FullName      // Họ tên đầy đủ
        {
            // ReSharper disable once ConvertPropertyToExpressionBody
            get { return string.Format("{1} {0}", FirstName, LastName); }
        }

        public virtual Account Account { get; set; }
        public virtual Class Class { get; set; }
    }
}