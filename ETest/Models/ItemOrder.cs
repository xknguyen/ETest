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
    public class ItemOrder
    {
        public string ChoiceId { get; set; }
        public int OrderNo { get; set; }
        public string Content { get; set; }
        public int Result { get; set; }

        public ItemOrder()
        {
        }

        public ItemOrder(JToken choice)
        {
            ChoiceId = DataUtil.ToString(choice["ChoiceId"]);
            OrderNo = DataUtil.ToInt(choice["OrderNo"]);
            Content = (string)choice["Content"];
            Result = DataUtil.ToInt(choice["Result"]);
        }
    }
}