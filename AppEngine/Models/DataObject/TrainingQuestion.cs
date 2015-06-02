using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppEngine.Models.DataObject;

namespace AppEngine.Models.DataObject
{
    public class TrainingQuestion
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int TrainingQuestionID { get; set; }
        public int TrainingID { get; set; }
        [MinLength(10)]
        [MaxLength(100)]
        [Required]
        public string Text { get; set; }
        public int DisplayNo { get; set; }
        public QuestionType Type { get; set; }

        public List<TrainingAnswer> Answers { get; set; }
    }

    public enum QuestionType
    {
        Single = 0,
        Multi = 1,
        Text = 2
    }
}
