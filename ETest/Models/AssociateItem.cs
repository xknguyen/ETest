using System.ComponentModel.DataAnnotations.Schema;

namespace ETest.Models
{
    [NotMapped]
    public class AssociateItem
    {
        public string ChoiceId { get; set; }
        public string Content { get; set; }
        public string AssociateId { get; set; }
        public int OrderNo { get; set; }

        public virtual AssociateItem Associate { get; set; }
    }
}