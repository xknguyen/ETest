
namespace ETest.Models
{
    public class Answer
    {
        public long AnswerId { get; set; }
        public long AnswerSheetId { get; set; }
        public long QuestionId { get; set; }
        public string AnswerString { get; set; }

        public virtual AnswerSheet AnswerSheet { get; set; }
        public virtual Question Question { get; set; }
    }
}