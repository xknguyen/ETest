using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Script.Serialization;
using Core.Utilities;
using Newtonsoft.Json;
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
        public bool IsDone { get; set; }
        public float Score { get; set; }

        public virtual Test Test { get; set; }

        private List<Answer> _answers;

        [NotMapped]
        public List<Answer> Answers
        {
            get
            {
                if (_answers == null)
                {
                    if (string.IsNullOrEmpty(Answer))
                    {
                        _answers = new List<Answer>();
                    }
                    else
                    {
                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        _answers = jss.Deserialize<List<Answer>>(Answer);
                    }
                }
                return _answers;
            }
            set { _answers = value; }
        }

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
            Answer = JsonConvert.SerializeObject(Answers,
                Formatting.Indented,
                new JsonSerializerSettings {DefaultValueHandling = DefaultValueHandling.Ignore});
        }

        public void Update(AnswerSheet sheet)
        {
            TestId = sheet.TestId;
            SubmitTime = sheet.SubmitTime;
            Answer = sheet.Answer;
            IsDone = sheet.IsDone;
            Score = sheet.Score;
        }
    }
}