using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Core.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ETest.Models
{
    [NotMapped]
    public class Answer
    {

        public long QuestionId { get; set; }

        public List<AnswerDetail> AnswerDetails { get; set; }

        
        public Answer(JToken jUser)
        {
            QuestionId = DataUtil.ToLong(jUser["id"]);
            var questions = jUser["questionDetail"].ToArray();
            AnswerDetails = new List<AnswerDetail>();
            foreach (var question in questions)
            {
                AnswerDetails.Add(new AnswerDetail(question));
            }
        }
    }
}