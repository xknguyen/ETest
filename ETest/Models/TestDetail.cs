
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ETest.Areas.Adm.Models;
using Newtonsoft.Json;

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

        public string QuestionDetails
        {
            get
            {
                var questionDetails = Question.QuestionDetails.Select(detail => new DataDetailModel(detail)).ToList();
                return JsonConvert.SerializeObject(questionDetails,
                    Formatting.Indented,
                    new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });
            }
        }

        public virtual Test Test { get; set; }
        public virtual Question Question { get; set; }
    }
}