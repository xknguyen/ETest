using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETest.Models
{
    public class Group
    {
        public long GroupId { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [StringLength(100, ErrorMessage = "{0} không vượt quá {2} kí tự.")]
        [Display(Name = "Tên nhóm")]
        public string GroupName { get; set; }

        [StringLength(1000, ErrorMessage = "{0} không vượt quá {2} kí tự.")]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Nhóm cha")]
        public long? ParentGroupId { get; set; }

        public string TeacherId { get; set; }

        [Display(Name = "Trạng thái")]
        public bool Active { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual Group ParentGroup { get; set; }

        
        public virtual Account Teacher { get; set; }
        public virtual IList<Group> ChildGroups { get; set; }
        public virtual IList<Question> Questions { get; set; }
        
    }
}