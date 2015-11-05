using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETest.Models;

namespace ETest.Areas.Adm.Models
{
    public class DataDetailModel
    {
        public long QuestionDetailId { get; set; }
        public string Score { get; set; }

        public DataDetailModel(QuestionDetail detail)
        {
            QuestionDetailId = detail.QuestionDetailId;
            Score = "";
        }
    }
}