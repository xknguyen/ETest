using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Core.Utilities;
using Newtonsoft.Json.Linq;

namespace ETest.Models
{
    [NotMapped]
    public class AssociateItem
    {
        public string ChoiceId { get; set; }
        public string Content { get; set; }
        public string AssociateId { get; set; }
        public int OrderNo { get; set; }

        public AssociateItem Associate { get; set; }

        public AssociateItem()
        {
            
        }

        public AssociateItem(string choiceId, string content)
        {
            ChoiceId = choiceId;
            Content = content;
        }

        public AssociateItem(JToken choice, int orderNo, List<AssociateItem> list)
        {
            OrderNo = orderNo;
            ChoiceId = DataUtil.ToString(choice["LeftId"]);
            ChoiceId = string.IsNullOrEmpty(ChoiceId) ? GetNewId(list) : ChoiceId;
            AssociateId = DataUtil.ToString(choice["RightId"]);
            AssociateId = string.IsNullOrEmpty(AssociateId) ? GetNewId(list) : ChoiceId;

            Content = DataUtil.ToString(choice["LeftContent"]);
            var rightContent = DataUtil.ToString(choice["RightContent"]);
            Associate = new AssociateItem(AssociateId, rightContent);
        }

        private string GetNewId(List<AssociateItem> list)
        {
            string result;
            for (;;)
            {
                result = Guid.NewGuid().ToString();
                var search = list.FirstOrDefault(s => s.AssociateId == result || s.AssociateId == result);
                if (search == null)
                    break;
            }
            return result;
        }
    }
}