﻿using System;
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

        [NotMapped]
        private List<Choice> _choices { get; set; }

        [NotMapped]
        public List<Choice> Choices {
            get
            {
                if (_choices == null)
                {
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    _choices = jss.Deserialize<List<Choice>>(Choice);
                }
                return _choices;
            }
            set { _choices = value; }
        }

        public QuestionDetail()
        {
                
        }

        public string ConvertChoiceToString()
        {
           return new JavaScriptSerializer().Serialize(Choices);
        }


        public QuestionDetail(JToken detail)
        {
            QuestionType = (QuestionType) DataUtil.ToInt(detail["QuestionType"]);
            QuestionDetailId = DataUtil.ToLong(detail["QuestionDetailId"]);
            QuestionTitle = (string)detail["QuestionTitle"];
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
                    break;
                case QuestionType.Associate:
                    break;
                case QuestionType.Gap:
                    break;
                case QuestionType.Inline:
                    break;
                case QuestionType.Upload:
                    break;
                case QuestionType.Slider:
                    break;
                    
            }
            //

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