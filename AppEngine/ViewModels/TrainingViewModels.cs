using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppEngine.Models.ViewModels.Training
{
    public class CheckTrainingDateModel
    {
        [Required]
        [Display(Name = "Data Wygenerowania")]
        public DateTime GenereateDate { get; set; }

        [Required]
        [Display(Name = "ID szkolenia")]
        public int TrainingID { get; set; }
    }

    public class StartTrainingModel
    {
        [Required]
        [Display(Name = "ID szkolenia")]
        public int TrainingID { get; set; }
    }

    public class SaveTrainingAnswersModel
    {
        [Required]
        [Display(Name = "Data Wygenerowania")]
        public DateTime GenereateDate { get; set; }

        [Required]
        [Display(Name = "ID szkolenia")]
        public int TrainingID { get; set; }

        [Required]
        [Display(Name = "Ocena szkolenia")]
        public int TrainingRate { get; set; }

        [Required]
        [Display(Name = "Odpowiedzi")]
        public string TrainingAnswers { get; set; }
    }
}
