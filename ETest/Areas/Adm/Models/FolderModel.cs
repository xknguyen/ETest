using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETest.Areas.Adm.Models
{
    public class FolderModel
    {
        public string href { get; set; }
        public string text { get; set; }

        public FolderModel(string href, string text)
        {
            this.href = href;
            this.text = text;
        }
    }
}