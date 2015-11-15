using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Core.Utilities;
using Newtonsoft.Json.Linq;

namespace ETest.Models
{
    public class AnswerSheet
    {
        public long AnswerSheetId { get; set; }
        public long TestId { get; set; }
        public string TestTakerId { get; set; }
        public DateTime SubmitTime { get; set; }
        public string Answer { get; set; }


        public virtual Test Test { get; set; }

        [NotMapped]
        public List<Answer> Answers { get; set; }

        public virtual Account TestTaker { get; set; }


        public AnswerSheet()
        {
            
        }

        public AnswerSheet(string data)
        {
            var jUser = JObject.Parse(data);
            TestId = DataUtil.ToLong(jUser["id"]);
            SubmitTime = DateTime.Now;
            var questions = jUser["testDetails"].ToArray();
            Answers = new List<Answer>();
            foreach (var question in questions)
            {
                Answers.Add(new Answer(question));
            }

        }
    }
}