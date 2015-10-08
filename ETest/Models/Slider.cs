using System.ComponentModel.DataAnnotations.Schema;
using System.Web.UI.WebControls;
using Core.Utilities;
using Newtonsoft.Json.Linq;

namespace ETest.Models
{
    [NotMapped]
    public class Slider
    {
        public double Max { get; set; }
        public double Min { get; set; }

        public Slider()
        {
        }

        public Slider(JToken choice)
        {
            Max = DataUtil.ToDouble(choice["Max"]);
            Min = DataUtil.ToDouble(choice["Min"]);
        }
    }
}