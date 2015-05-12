using AppEngine.Models.Common;
using AppEngine.Models.DataBusiness;
using AppEngine.Models.DataObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEngine.ViewModels
{
    public class TrainingViewModel : BaseViewModel
    {
        public List<Training> Trainings { get; set; }
        public Training Current { get; set; }
        public List<TrainingDetail> Details { get; set; }
        public List<TrainingQuestion> Questions { get; set; }
        public List<ProfileGroup> Groups { get; set; }
        public int trainingID { get; set; }
    }
}
