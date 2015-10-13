using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ETest.Models
{
    [NotMapped]
    public class GapItem
    {
        public int ItemId { get; set; }
        public string ItemContent { get; set; }
        public int OrderId { get; set; }
    }
}