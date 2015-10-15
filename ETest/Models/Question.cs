using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Core.Utilities;
using Newtonsoft.Json.Linq;

namespace ETest.Models
{
    public enum QuestionType
    {
        Choice,
        Order,
        Associate,
        Gap,
        Slider
    }

    public class Question
    {
        public long QuestionId { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Tiêu đề")]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string QuestionTitle { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Nhóm câu hỏi")]
        public long GroupId { get; set; }

        [Display(Name = "Trạng thái")]
        public bool Actived { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual IList<Test> Tests { get; set; }
        public virtual IList<TestDetail> TestDetails { get; set; }
        public virtual Group Group { get; set; }
        
        public virtual IList<Answer> Answers { get; set; }
        public virtual IList<QuestionDetail> QuestionDetails { get; set; }

        

        public Question()
        {
                
        }

        public Question(string data)
        {
            var questionDetails = new List<QuestionDetail>();
            JObject jObject = JObject.Parse(data);
            JToken jUser = jObject["question"];
            QuestionId = DataUtil.ToLong(jUser["QuestionId"]);
            QuestionTitle = (string)jUser["QuestionTitle"];
            GroupId = DataUtil.ToLong(jUser["GroupId"]);
            Actived = DataUtil.ToBool(jUser["Actived"]);
            var questions = jUser["Questions"].ToArray();

            foreach (var question in questions)
            {
                questionDetails.Add(new QuestionDetail(question));
            }

            // ReSharper disable once DoNotCallOverridableMethodsInConstructor
            QuestionDetails = questionDetails;
        }
    }
}