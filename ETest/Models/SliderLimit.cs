using System.ComponentModel.DataAnnotations.Schema;
using System.Web.UI.WebControls;
using Core.Utilities;
using Newtonsoft.Json.Linq;

namespace ETest.Models
{
    [NotMapped]
    public class SliderLimit
    {
        public double Max { get; set; }
        public double Min { get; set; }
        public double Step { get; set; }
        public double Value { get; set; }

        public SliderLimit()
        {
            Max = 10;
            Min = 0;
            Step = 1;
            Value = 0;
        }

        public SliderLimit(JToken choice)
        {
            Max = DataUtil.ToDouble(choice["Max"]);
            Min = DataUtil.ToDouble(choice["Min"]);
            Step = DataUtil.ToDouble(choice["Step"]);
            Value = DataUtil.ToDouble(choice["Value"]);
        }
    }
}