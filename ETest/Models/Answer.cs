﻿
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETest.Models
{
    public class Answer
    {
        [Key]
        [Column(Order = 0)]
        public long AnswerSheetId { get; set; }

        [Key]
        [Column(Order = 1)]
        public long QuestionId { get; set; }

        [Required]
        [Column(TypeName = "ntext")]
        public string AnswerString { get; set; }

        public virtual AnswerSheet AnswerSheet { get; set; }
        public virtual Question Question { get; set; }
    }
}