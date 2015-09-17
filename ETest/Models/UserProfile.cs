using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETest.Models
{
    public class UserProfile
    {
        [Key, ForeignKey("Account"), StringLength(128), DisplayName("Mã tài khoản")]
        public string AccountId { get; set; }       // Mã tài khoản

        [Required, StringLength(30), DisplayName("Họ và tên lót")]
        public string FirstName { get; set; }       // Họ và tên lót

        [Required, StringLength(20), DisplayName("Tên")]
        public string LastName { get; set; }        // Tên
       
        [StringLength(1000), DataType(DataType.MultilineText), DisplayName("Ghi chú")]
        public string Notes { get; set; }       // Ghi chú

        [DisplayName("Họ tên")]
        public string FullName      // Họ tên đầy đủ
        {
            // ReSharper disable once ConvertPropertyToExpressionBody
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }

        [StringLength(10), DisplayName("Lớp")]
        public string ClassName { get; set; }

        public virtual Account Account { get; set; }
    }
}