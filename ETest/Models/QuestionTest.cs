
namespace ETest.Models
{
    public class QuestionTest
    {
        public long TestId { get; set; }
        public long QuestionId { get; set; }
        public float Score { set; get; }

        public virtual Test Test { get; set; }
        public virtual Question Question { get; set; }
    }
}