using System.Collections.Generic;

namespace ETest.Areas.Adm.Models
{
    public class FolderModel
    {
        public string href { get; set; }
        public string text { get; set; }

        public List<FolderModel> nodes { get; set; }

        public FolderModel(string text, string href)
        {
            this.href = href;
            this.text = text;
        }

        public FolderModel()
        {
            
        }
    }
}