using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppEngine.Models.DataObject
{
    public class TrainingAnswer
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int TrainingAnswerID { get; set; }
        public int TrainingQuestionID { get; set; }
        public string Text { get; set; }
        public string Answers { get; set; }
        public string Score { get; set; }
        public bool IsSelected { get; set; }
    }
}
