﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ETest.Models
{
    public class Course
    {
        public long CourseId { get; set; }
        public string CourseName { get; set; }
        public string Description { get; set; }
        public string TeacherId { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual Account Teacher { get; set; }
        public virtual List<Account> Students { get; set; }
        public virtual List<Test> Tests { get; set; }
    }
}