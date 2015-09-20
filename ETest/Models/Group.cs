using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETest.Models
{
    public class Group
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public string Description { get; set; }

        public int ParentGroupId { get; set; }

        public virtual Group ParentGroup { get; set; }
        public virtual List<Question> Questions { get; set; }
         
    }
}