using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ETest.Models
{
    public class Group
    {
        public long GroupId { get; set; }

        public string GroupName { get; set; }

        public string Description { get; set; }

        public long ParentGroupId { get; set; }

        public string TeacherId { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual Group ParentGroup { get; set; }
        public virtual List<Question> Questions { get; set; }
        public virtual Account Teacher { get; set; }
    }
}