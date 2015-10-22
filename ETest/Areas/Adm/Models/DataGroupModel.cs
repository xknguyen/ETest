using System.Collections.Generic;
using ETest.Models;
using Newtonsoft.Json;

namespace ETest.Areas.Adm.Models
{
    public class DataGroupModel
    {
        public string href { get; set; }
        public string text { get; set; }

        public List<DataGroupModel> nodes { get; set; }

        public DataGroupModel(Group group)
        {
            text = group.GroupName.Length > 30 ? group.GroupName.Substring(0, 30) +"..." : group.GroupName;
            href = group.GroupId.ToString();
            if (group.ChildGroups != null && group.ChildGroups.Count!= 0)
            {
                nodes = new List<DataGroupModel>();
                foreach (var child in group.ChildGroups)
                {
                    nodes.Add(new DataGroupModel(child));
                }
            }
        }

        public DataGroupModel()
        {

        }
    }
}