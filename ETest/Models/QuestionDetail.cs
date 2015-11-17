using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Core.Utilities;
using Newtonsoft.Json.Linq;

namespace ETest.Models
{
    public class QuestionDetail
    {
        public long QuestionDetailId { get; set; }

        [Display(Name = "Tiêu đề")]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string QuestionTitle { get; set; }

        [Display(Name = "Đáp án")]
        public string Choice { get; set; }

        [Display(Name = "Đáp án")]
        public string Answer { get; set; }

        public int OrderNo { get; set; }

        public long QuestionId { get; set; }

        public QuestionType QuestionType { get; set; }

        public virtual Question Question { get; set; }

        [NotMapped]
        private List<Choice> _choices;

        [NotMapped]
        public List<Choice> Choices
        {
            get
            {
                if (_choices == null)
                {
                    if (string.IsNullOrEmpty(Choice))
                    {
                        _choices = new List<Choice>();
                    }
                    else
                    {
                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        _choices = jss.Deserialize<List<Choice>>(Choice);
                    }
                }
                return _choices;
            }
            set { _choices = value; }
        }

        [NotMapped]
        private List<ItemOrder> _itemOrders;

        [NotMapped]
        public List<ItemOrder> ItemOrders
        {
            get
            {
                if (_itemOrders == null)
                {
                    if (string.IsNullOrEmpty(Choice))
                    {
                        _itemOrders = new List<ItemOrder>();
                    }
                    else
                    {
                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        _itemOrders = jss.Deserialize<List<ItemOrder>>(Choice);
                    }

                }
                return _itemOrders;
            }
            set { _itemOrders = value; }
        }

        [NotMapped]
        private SliderLimit _sliderLimit;

        [NotMapped]
        public SliderLimit SliderLimit
        {
            get
            {
                if (_sliderLimit == null)
                {
                    if (string.IsNullOrEmpty(Choice))
                    {
                        _sliderLimit = new SliderLimit();
                    }
                    else
                    {
                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        _sliderLimit = jss.Deserialize<SliderLimit>(Choice);
                    }
                }
                return _sliderLimit;
            }
            set { _sliderLimit = value; }
        }

        [NotMapped]
        private List<AssociateItem> _associateItems;

        [NotMapped]
        public List<AssociateItem> AssociateItems
        {
            get
            {
                if (_associateItems == null)
                {
                    if (string.IsNullOrEmpty(Choice))
                    {
                        _associateItems = new List<AssociateItem>();
                    }
                    else
                    {
                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        _associateItems = jss.Deserialize<List<AssociateItem>>(Choice);
                    }
                }
                return _associateItems;
            }
            set { _associateItems = value; }
        }

        [NotMapped]
        private List<GapItem> _gapItems;

        [NotMapped]
        public List<GapItem> GapItems
        {
            get
            {
                if (_gapItems == null)
                {
                    if (string.IsNullOrEmpty(Choice))
                    {
                        _gapItems = new List<GapItem>();
                    }
                    else
                    {
                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        _gapItems = jss.Deserialize<List<GapItem>>(Choice);
                    }
                }
                return _gapItems;
            }
            set { _gapItems = value; }
        }

        public QuestionDetail()
        {

        }

        public string ConvertChoiceToString()
        {
            switch (QuestionType)
            {
                case QuestionType.ChoiceMedia:
                case QuestionType.Choice:
                    return new JavaScriptSerializer().Serialize(Choices);
                case QuestionType.Order:
                    return new JavaScriptSerializer().Serialize(ItemOrders);
                case QuestionType.Associate:
                    return new JavaScriptSerializer().Serialize(AssociateItems);
                case QuestionType.Fill:
                case QuestionType.Gap:
                    return new JavaScriptSerializer().Serialize(GapItems);
                case QuestionType.Slider:
                    return new JavaScriptSerializer().Serialize(SliderLimit);
                default:
                    return "";
            }
        }

        public QuestionDetail(JToken detail)
        {
            QuestionType = (QuestionType)DataUtil.ToInt(detail["QuestionType"]);
            QuestionDetailId = DataUtil.ToLong(detail["QuestionDetailId"]);
            QuestionTitle = (string)detail["QuestionTitle"];
            OrderNo = DataUtil.ToInt(detail["OrderNo"]);
            int i;
            switch (QuestionType)
            {
                case QuestionType.ChoiceMedia:
                case QuestionType.Choice:
                    Choices = new List<Choice>();
                    i = 0;
                    foreach (var choice in detail["Choices"].ToArray())
                    {
                        Choices.Add(new Choice(choice, i++, Choices));
                    }
                    break;
                case QuestionType.Order:
                    ItemOrders = new List<ItemOrder>();
                    foreach (var choice in detail["Choices"].ToArray())
                    {
                        ItemOrders.Add(new ItemOrder(choice, ItemOrders));
                    }
                    break;
                case QuestionType.Associate:
                    i = 0;
                    AssociateItems = new List<AssociateItem>();
                    foreach (var choice in detail["Choices"].ToArray())
                    {
                        AssociateItems.Add(new AssociateItem(choice, i++, AssociateItems));
                    }
                    break;
                case QuestionType.Fill:
                case QuestionType.Gap:
                    i = 0;
                    GapItems = new List<GapItem>();
                    foreach (var choice in detail["Choices"].ToArray())
                    {
                        GapItems.Add(new GapItem(choice, i++));
                    }
                    break;
                case QuestionType.Slider:
                    SliderLimit = new SliderLimit(detail["Choices"]);
                    break;
            }
            Choice = ConvertChoiceToString();
        }

        public bool IsSingleChoice
        {
            get
            {
                var choiceCorrect = Choices.Where(s => s.IsCorrect).ToList();
                return choiceCorrect.Count <= 1;
            }
        }

        public void Update(QuestionDetail detai)
        {
            QuestionType = detai.QuestionType;
            QuestionDetailId = detai.QuestionDetailId;
            QuestionTitle = detai.QuestionTitle;
            OrderNo = detai.OrderNo;
        }

        public bool CheckCorrect(AnswerDetail detail)
        {
            switch (QuestionType)
            {
                case QuestionType.ChoiceMedia:
                case QuestionType.Choice:
                    var correctId = Choices.Where(s => s.IsCorrect).ToList();
                    if (detail.Choices == null || detail.Choices.Count == 0)
                    {
                        return false;
                    }
                    foreach (var id in detail.Choices)
                    {
                        if (correctId.All(s => s.ChoiceId != id))
                        {
                            return false;
                        }
                    }
                    break;
                case QuestionType.Order:
                    var itemOrders = ItemOrders.OrderBy(s => s.Result).ToList();
                    for (var i = 0; i < ItemOrders.Count; i++)
                    {
                        if (itemOrders[i].ChoiceId != detail.Choices[i])
                        {
                            return false;
                        }
                    }
                    break;
                case QuestionType.Associate:
                    foreach (var items in AssociateItems)
                    {
                        var answerItem = detail.AssociateItems.Find(s => s.ChoiceId == items.ChoiceId);
                        if (answerItem.AssociateId != items.AssociateId)
                        {
                            return false;
                        }
                    }
                    break;
                case QuestionType.Fill:
                case QuestionType.Gap:
                    detail.GapItems = GapItems.OrderBy(s => s.ItemId).ToList();
                    var resultGaps = new List<string>();
                    var inputMatch = Regex.Match(QuestionTitle, @"<input name=""gapField"" type=""text"" value=""(?<inputId>[\d]+)"" />");
                    while (inputMatch.Success)
                    {
                        var id = DataUtil.ToInt(inputMatch.Groups["inputId"].Value);
                        var gapFill = GapItems.Find(s => s.ItemId == id);
                        if (gapFill != null)
                            resultGaps.Add(gapFill.ItemContent.Trim());
                        inputMatch = inputMatch.NextMatch();
                    }
                    for (var i = 0; i < resultGaps.Count; i++)
                    {
                        if (resultGaps[i] != detail.GapItems[i].ItemContent)
                        {
                            return false;
                        }
                    }
                    break;
                case QuestionType.Slider:
                    if (detail.Choices == null || detail.Choices.Count != 1 || string.IsNullOrEmpty(detail.Choices[0]))
                    {
                        return false;
                    }
                    var value = DataUtil.ToFloat(detail.Choices[0]);
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if (value != SliderLimit.Value)
                        return false;
                    break;
            }
            return true;
        }

    }
}