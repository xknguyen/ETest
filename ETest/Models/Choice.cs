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

        public Choice(JToken choice, int orderNo)
        {
            ChoiceId = DataUtil.ToLong(choice["ChoiceId"]);
            Content = (string)choice["Content"];
            OrderNo = orderNo;
            Scrore = DataUtil.ToFloat(choice["Scrore"]);
            IsCorrect = DataUtil.ToBool(choice["IsCorrect"]);
        }

        public long ChoiceId { get; set; }
        public string Content { get; set; }
        public float Scrore { get; set; }
        public bool IsCorrect { get; set; }
        public int OrderNo { get; set; }
    }
}