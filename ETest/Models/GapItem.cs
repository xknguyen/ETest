using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Core.Utilities;
using Newtonsoft.Json.Linq;

namespace ETest.Models
{
    [NotMapped]
    public class GapItem
    {
        public int ItemId { get; set; }
        public string ItemContent { get; set; }
        public int OrderNo { get; set; }

        public GapItem(JToken choice, int orderNo)
        {
            OrderNo = orderNo;
            ItemId = DataUtil.ToInt(choice["id"]);
            ItemContent = DataUtil.ToString(choice["content"]);
        }

        public GapItem()
        {
            
        }
    }
}