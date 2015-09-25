
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETest.Models
{
    public class TestDetail
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long TestId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long QuestionId { get; set; }

        public float Score { set; get; }

        public virtual Test Test { get; set; }
        public virtual Question Question { get; set; }
    }
}