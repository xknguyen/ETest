using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Core.Utilities;
using Newtonsoft.Json.Linq;

namespace ETest.Models
{
    public class QuestionDetail
    {
        public long QuestionDetailId { get; set; }

        [Required(ErrorMessage = "{0} không được để trống")]
        [Display(Name = "Tiêu đề")]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string QuestionTitle { get; set; }

        [Display(Name = "Lựa chọn")]
        public string Choice { get; set; }

        [Display(Name = "Đáp án")]
        public string Answer { get; set; }

        public int OrderNo { get; set; }

        public long QuestionId { get; set; }

        public QuestionType QuestionType { get; set; }

        public virtual Question Question { get; set; }

        [NotMapped] private List<Choice> _choices;

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

        [NotMapped] private List<ItemOrder> _itemOrders;

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

        [NotMapped] private SliderLimit _sliderLimit;

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

        [NotMapped] private List<AssociateItem> _associateItems;

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

        [NotMapped] private List<GapItem> _gapItems;

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
                case QuestionType.Choice:
                    return new JavaScriptSerializer().Serialize(Choices);
                case QuestionType.Order:
                    return new JavaScriptSerializer().Serialize(ItemOrders);
                case QuestionType.Associate:
                    break;
                case QuestionType.Gap:
                    break;
                case QuestionType.Inline:
                    break;
                case QuestionType.Upload:
                    break;
                case QuestionType.Slider:
                    return new JavaScriptSerializer().Serialize(SliderLimit);
                default:
                    return "";
            }
            return "";
        }

        public QuestionDetail(JToken detail)
        {
            QuestionType = (QuestionType) DataUtil.ToInt(detail["QuestionType"]);
            QuestionDetailId = DataUtil.ToLong(detail["QuestionDetailId"]);
            QuestionTitle = (string) detail["QuestionTitle"];
            OrderNo = DataUtil.ToInt(detail["OrderNo"]);

            switch (QuestionType)
            {
                case QuestionType.Choice:
                    Choices = new List<Choice>();
                    int i = 0;
                    foreach (var choice in detail["Choice"].ToArray())
                    {
                        Choices.Add(new Choice(choice, i++));
                    }
                    Choice = ConvertChoiceToString();
                    break;
                case QuestionType.Order:
                    ItemOrders = new List<ItemOrder>();
                    foreach (var choice in detail["Items"].ToArray())
                    {
                        ItemOrders.Add(new ItemOrder(choice));
                    }
                    Choice = ConvertChoiceToString();
                    break;
                case QuestionType.Associate:
                    break;
                case QuestionType.Gap:
                    break;
                case QuestionType.Inline:
                    break;
                //case QuestionType.Upload:
                //    break;
                case QuestionType.Slider:
                    SliderLimit = new SliderLimit(detail["Limit"]);
                    break;
            }
        }

        public void Update(QuestionDetail detai)
        {
            QuestionType = detai.QuestionType;
            QuestionDetailId = detai.QuestionDetailId;
            QuestionTitle = detai.QuestionTitle;
            OrderNo = detai.OrderNo;
        }
    }
}