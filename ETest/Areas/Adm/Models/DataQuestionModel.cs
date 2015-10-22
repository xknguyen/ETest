using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETest.Models;
using Newtonsoft.Json;

namespace ETest.Areas.Adm.Models
{
    public class DataQuestionModel
    {
        public long QuestionId { get; set; }
        public string QuestionTitle { get; set; }
        public string QuestionDetails { get; set; }

        public DataQuestionModel(Question question)
        {
            QuestionId = question.QuestionId;
            QuestionTitle = question.QuestionTitle;
            var questionDetails = new List<DataDetailModel>();
            foreach (var detail in question.QuestionDetails)
            {
                questionDetails.Add(new DataDetailModel(detail));
            }

            QuestionDetails = JsonConvert.SerializeObject(questionDetails,
                Formatting.Indented,
                new JsonSerializerSettings {DefaultValueHandling = DefaultValueHandling.Ignore});
        }
    }
}