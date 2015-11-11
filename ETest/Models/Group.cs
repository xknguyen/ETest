using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
namespace ETest.Models
{
    public class Group
    {
        public long GroupId { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [StringLength(100, ErrorMessage = "{0} không vượt quá {1} kí tự.")]
        [Display(Name = "Tên nhóm")]
        public string GroupName { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        public string GetDescription
        {
            get { return HttpUtility.HtmlDecode(Description); }
        }

        [Display(Name = "Nhóm cha")]
        public long? ParentGroupId { get; set; }

        public long CourseId { get; set; }

        [Display(Name = "Trạng thái")]
        public bool Actived { get; set; }

        [Display(Name = "Thứ tự")]
        public int OrderNo { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual Group ParentGroup { get; set; }
        public virtual Course Course { get; set; }
        public virtual IList<Group> ChildGroups { get; set; }
        public virtual IList<Question> Questions { get; set; }
    }
}