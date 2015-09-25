using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ETest.Models
{
    // You can add profile data for the user by adding more properties to your Account class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class Account : IdentityUser
    {

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

        public virtual Class Class { get; set; }
        public virtual IList<Group> Groups { get; set; }
        public virtual IList<AnswerSheet> AnswerSheets { get; set; }
        public virtual IList<Course> Courses { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Account> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

}