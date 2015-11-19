using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ETest.Areas.Adm.Models
{
    public class AccountViewModel
    {
        [Required(ErrorMessage = "{0} không được để trống")]
        [StringLength(100, ErrorMessage = "{0} từ {2} đến {1} kí tự.", MinimumLength = 6)]
        [Display(Name = "Tên người dùng")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [EmailAddress(ErrorMessage = "{0} không đúng định dạng. Vd: example@gmail.com")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [StringLength(10, ErrorMessage = "{0} không vượt quá {1} kí tự.")]
        [Display(Name = "Mã tài khoản")]
        public string Identity { get; set; }

        [Phone(ErrorMessage = "{0} không đúng định dạng.")]
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [DisplayName("Họ và tên lót")]
        [StringLength(100, ErrorMessage = "{0} không vượt quá {1} kí tự.")]
        public string LastName { get; set; }       // Họ và tên lót

        [Required(ErrorMessage = "{0} không được để trống")]
        [StringLength(100, ErrorMessage = "{0} không vượt quá {1} kí tự.")]
        [DisplayName("Tên")]
        public string FirstName { get; set; }        // Tên

        [StringLength(1000, ErrorMessage = "{0} không vượt quá {1} kí tự.")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        [DisplayName("Ghi chú")]
        public string Notes { get; set; }       // Ghi chú

        [DataType(DataType.Date)]
        [DisplayName("Ngày sinh")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "Trạng thái")]
        public bool Actived { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Loại tài khoản")]
        public string Role { get; set; }

        public SelectList AccountRoles { get; set; }
    }

    public class CreateAccountModel : AccountViewModel
    {
        [Required(ErrorMessage = "{0} không được để trống")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "{0} từ {2} đến {1} kí tự.", MinimumLength = 6)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
    }

    public class EditAccountModel : AccountViewModel
    {
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "{0} từ {2} đến {1} kí tự.", MinimumLength = 6)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

    }
}