using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ETest.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class Account : IdentityUser
    {
        [StringLength(10), Display(Name = "MSSV")]
        public string Identity { get; set; }

        [Required, StringLength(30), DisplayName("Họ và tên lót")]
        public string LastName { get; set; }       // Họ và tên lót

        [Required]
        [StringLength(20)]
        [DisplayName("Tên")]
        public string FirstName { get; set; }        // Tên

        [StringLength(1000)]
        [DataType(DataType.MultilineText)]
        [DisplayName("Ghi chú")]
        public string Notes { get; set; }       // Ghi chú

        [DisplayName("Lớp")]
        public int ClassId { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Ngày sinh")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? BirthDate { get; set; }

        [DisplayName("Họ tên")]
        public string FullName      // Họ tên đầy đủ
        {
            // ReSharper disable once ConvertPropertyToExpressionBody
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Account> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

}