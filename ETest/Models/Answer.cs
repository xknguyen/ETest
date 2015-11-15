using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Core.Utilities;
using Newtonsoft.Json.Linq;

namespace ETest.Models
{
    [NotMapped]
    public class Answer
    {

        public long QuestionId { get; set; }

        public string QuestionType { get; set; }

        public string AnswerString { get; set; }

        public List<Answer> AnswerDetails { get; set; }

        public Answer()
        {
            
        }

        public Answer(JToken jUser)
        {
            QuestionId = DataUtil.ToLong(jUser["id"]);
            var questions = jUser["questionDetail"].ToArray();
            AnswerDetails = new List<Answer>();
            foreach (var question in questions)
            {
                AnswerDetails.Add(CreateDetail(question));
            }
        }

        private Answer CreateDetail(JToken jUser)
        {
            var answer = new Answer
            {
                QuestionId = DataUtil.ToLong(jUser["id"]),
                QuestionType = DataUtil.ToString(jUser["type"])
            };
            switch (answer.QuestionType)
            {
                case "ChoiceMedia":
                case "Choice":
                    break;
                case "Order":
                    break;
                case "Associate":
                    break;
                case "Gap":
                    break;
                case "Slider":
                    break;
                case "Fill":
                    break;
            }
            return answer;
        }
    }
}