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
    public class AnswerDetail
    {
        public long QuestionDetailId { get; set; }

        public string QuestionType { get; set; }

        public string AnswerString { get; set; }
        public List<string> Choices { get; set; }

        public List<AssociateItem> AssociateItems { get; set; }

        public List<GapItem> GapItems { get; set; }

        public QuestionDetail QuestionDetail { get; set; }
        public AnswerDetail()
        {

        }
        public AnswerDetail(JToken jUser)
        {
            QuestionDetailId = DataUtil.ToLong(jUser["id"]);
            QuestionType = DataUtil.ToString(jUser["type"]);
            var details = jUser["answer"].ToArray();
            switch (QuestionType)
            {
                case "ChoiceMedia":
                case "Choice":
                case "Order":
                case "Slider":
                    Choices = new List<string>();
                    foreach (var dt in details)
                    {
                        Choices.Add(DataUtil.ToString(dt.Value<string>()));
                    }
                    break;
                case "Associate":
                    AssociateItems = new List<AssociateItem>();
                    foreach (var dt in details)
                    {
                        var leftId = DataUtil.ToString(dt["leftId"]);
                        var rightId = DataUtil.ToString(dt["rightId"]);
                        AssociateItems.Add(new AssociateItem
                        {
                            ChoiceId = leftId,
                            AssociateId = rightId
                        });
                    }
                    break;
                case "Gap":
                case "Fill":
                    GapItems = new List<GapItem>();
                    foreach (var dt in details)
                    {
                        var id = DataUtil.ToInt(dt["index"]);
                        var content = DataUtil.ToString(dt["answer"]);
                        GapItems.Add(new GapItem
                        {
                            ItemId = id,
                            ItemContent = content
                        });
                    }
                    break;
            }
        }
    }
}