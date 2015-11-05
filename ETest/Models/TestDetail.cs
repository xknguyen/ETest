using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Core.Utilities;
using ETest.Areas.Adm.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        public int OrderNo { get; set; }

        [NotMapped]
        public List<TestDetailData> Details { get; set; }

        public string QuestionDetails { get; set; }

        public virtual Test Test { get; set; }
        public virtual Question Question { get; set; }

        public TestDetail(JToken detail)
        {
            Details = new List<TestDetailData>();
            QuestionId = DataUtil.ToLong(detail["id"]);
            Score = DataUtil.ToFloat(detail["score"]);
            OrderNo = DataUtil.ToInt(detail["order"]);
            var details = detail["details"].ToArray();
            foreach (var de in details)
            {
                Details.Add(new TestDetailData(de));
            }
            QuestionDetails = JsonConvert.SerializeObject(Details,
                Formatting.Indented,
                new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });
        }

        public TestDetail()
        {
            
        }
    }
}