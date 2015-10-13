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
            ChoiceId = orderNo;
            Content = (string)choice["Content"];
            IsCorrect = DataUtil.ToBool(choice["IsCorrect"]);
        }

        public int ChoiceId { get; set; }
        public string Content { get; set; }
        public bool IsCorrect { get; set; }
    }
}