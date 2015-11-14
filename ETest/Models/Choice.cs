using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Core.Utilities;
using Newtonsoft.Json.Linq;

namespace ETest.Models
{
    [NotMapped]
    public class Choice
    {
        public Choice()
        {
        }

        public Choice(JToken choice, int orderNo, List<Choice> list)
        {
            ChoiceId = DataUtil.ToString(choice["ChoiceId"]);
            ChoiceId = string.IsNullOrEmpty(ChoiceId) ? GetNewId(list) : ChoiceId;
            OrderNo = orderNo;
            Content = (string)choice["Content"];
            IsCorrect = DataUtil.ToBool(choice["IsCorrect"]);
        }

        public string ChoiceId { get; set; }
        public int OrderNo { get; set; }
        public string Content { get; set; }
        public bool IsCorrect { get; set; }


        private string GetNewId(List<Choice> list)
        {
            string result;
            for (;;)
            {
                result = Guid.NewGuid().ToString();
                var search = list.FirstOrDefault(s => s.ChoiceId == result );
                if (search == null)
                    break;
            }
            return result;
        }
    }
}