using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ETest.Models
{
    public class Class
    {
        public long ClassId { get; set; }
        public string ClassName { get; set; }
        public string Description { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual List<Account> Students { get; set; }
    }
}