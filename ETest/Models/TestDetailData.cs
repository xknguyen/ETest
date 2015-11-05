using System.ComponentModel.DataAnnotations.Schema;
using Core.Utilities;
using Newtonsoft.Json.Linq;

namespace ETest.Models
{
    [NotMapped]
    public class TestDetailData
    {

        public TestDetailData(JToken detail)
        {
            QuestionDetailId = DataUtil.ToLong(detail["id"]);
            Score = DataUtil.ToFloat(detail["score"]);
        }

        public TestDetailData()
        {
            
        }
        public long QuestionDetailId { get; set; }
        public float Score { get; set; }
    }
}