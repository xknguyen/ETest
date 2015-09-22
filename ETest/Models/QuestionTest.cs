﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ETest.Models
{
    public class QuestionTest
    {
        public long TestId { get; set; }
        public long QuestionId { get; set; }
        public float Score { set; get; }

        public virtual Test Test { get; set; }
        public virtual Question Question { get; set; }
    }
}